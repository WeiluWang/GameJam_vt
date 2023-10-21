using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BaddieMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float speed = 10f;
    [SerializeField] private Animator anim;
    [SerializeField] public bool isMoving;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isMoving = false;
        Vector3 pos = transform.position;
        Vector3 scale = transform.localScale;
        if (Input.GetKey("w"))
        {
            isMoving = true;
            pos.y += speed * Time.deltaTime;
        }
        else if(Input.GetKey("s"))
        {
            isMoving = true;
            
            pos.y -= speed * Time.deltaTime;
        }
        else if (Input.GetKey("a"))
        {
            isMoving = true;
            transform.localScale = new Vector3(1, 1, 1);
            pos.x -= speed * Time.deltaTime;
        }
        else if (Input.GetKey("d"))
        {
            isMoving = true;
            transform.localScale = new Vector3(-1, 1, 1);
            pos.x += speed * Time.deltaTime;
        }
        if (isMoving == false)
        {
            anim.SetBool("isMove", false);
        }
        else 
        {
            anim.SetBool("isMove", true);
        }
        transform.position = pos;
    }
}
