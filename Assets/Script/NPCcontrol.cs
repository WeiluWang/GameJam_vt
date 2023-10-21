using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPCcontrol : NPC
{
    public NPCstate state;
    void Awake()
    {
        NPCs.Add(this);
    }

    void Update()
    {
        if (NPCs[Baddie] == this)
        {
            Vector3 pos = transform.position;
            Vector3 scale = transform.localScale;
            if (Input.GetKey("w"))
            {
                if (pos.y < 9f)
                {
                    pos.y += nobleSpeed * Time.deltaTime;
                }
            }
            else if (Input.GetKey("s"))
            {
                if (pos.y > -9f)
                {
                    pos.y -= nobleSpeed * Time.deltaTime;
                }
            }
            else if (Input.GetKey("a"))
            {
                if (pos.x > -17f)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    pos.x -= nobleSpeed * Time.deltaTime;
                }
            }
            else if (Input.GetKey("d"))
            {
                if (pos.x < 17f)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    pos.x += nobleSpeed * Time.deltaTime;
                }
            }
            transform.position = pos;
        }
    }
}
