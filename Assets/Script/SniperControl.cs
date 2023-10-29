using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SniperControl : MonoBehaviour
{
    public GameObject textObject;
    private TMP_Text bulletText;
    public AudioSource shootAudio;
    public AudioSource reloadAudio;
    [SerializeField] public Camera mainCamera;
    [SerializeField] public float reloadTime;
    [SerializeField] public float reloadCountDown;
    [SerializeField] public float bulletCount;
    public float shootRadius = 0.5f;

    private void Awake()
    {
        bulletText = textObject.GetComponent<TMP_Text>();
    }
    // Start is called before the first frame update
    void Start()
    {
        reloadCountDown = reloadTime;
        Invoke("Reload", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        bulletText.text = bulletCount.ToString();
        if (reloadCountDown <= 0f)
        {
            bulletText.color = Color.blue;
        }
        else
        {
            bulletText.color = Color.grey;
        }
        //Debug.Log(Input.mousePosition);
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        transform.position = mouseWorldPosition;
        reloadCountDown -= Time.deltaTime;

        if (Input.GetMouseButton(0) && reloadCountDown <= 0f && bulletCount >= 1)
        {
            shootAudio.Play();
            mainCamera.GetComponent<WraparoundCamera>().StartShake();
            reloadCountDown = reloadTime;
            if (reloadTime > 0.5f)
            {
                Invoke("Reload", 0.5f);
            }
            bulletCount--;
            Collider2D[] targets = NPC.WarpedOverlapCircleAll(mouseWorldPosition, shootRadius);
            foreach (Collider2D target in targets)
            {
                target.GetComponent<NPCcontrol>().die();
            }
        }
    }

    public void Reload()
    {
        reloadAudio.Play();
    }
}
