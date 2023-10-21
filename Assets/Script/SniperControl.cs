using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SniperControl : MonoBehaviour
{
    [SerializeField] public Camera mainCamera;
    public static bool shootablel;
    public float reloadTime;
    private float reloadCountDown;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Input.mousePosition);
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        transform.position = mouseWorldPosition;

        if (Input.GetMouseButtonDown(0))
        {
            Collider2D[] targets = Physics2D.OverlapCircleAll(mouseWorldPosition, 0.5f);
            foreach (Collider2D target in targets)
            {
                target.GetComponent<NPCcontrol>().die();
                //NPC.scare(mouseWorldPosition);
            }
        }
    }
}
