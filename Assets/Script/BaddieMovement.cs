using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BaddieMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float speed = 10f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        if (Input.GetKey("w"))
        {
            pos.y += speed * Time.deltaTime;
        }
        else if(Input.GetKey("s"))
        {
            pos.y -= speed * Time.deltaTime;
        }
        else if (Input.GetKey("a"))
        {
            pos.x -= speed * Time.deltaTime;
        }
        else if (Input.GetKey("d"))
        {
            pos.x += speed * Time.deltaTime;
        }
        transform.position = pos;
    }
}
