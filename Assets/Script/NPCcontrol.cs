using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class NPCcontrol : NPC
{
    public NPCstate state;
    public NPCAction action;
    public float dyingCountDown = DyingTime;
    public float scareCountDown = 0;
    [SerializeField] private Animator anim;
    //[SerializeField] public bool isMoving = false;
    [SerializeField] public bool isShooted = false;
    void Awake()
    {
        NPCs.Add(this);
        state = NPCstate.Noble;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (state != NPCstate.Dead)
        {
            if (NPCs[Baddie] == this)
            {
                PlayerControl();
            }
            else if (state == NPCstate.Dying)
            {
                dyingCountDown -= Time.deltaTime;
                if (dyingCountDown < 0f)
                {
                    die();
                    //scare(transform.position);
                }
            }
            Movement();
        }
    }

    public void die()
    {
        if (state != NPCstate.Dead)
        {
            state = NPCstate.Dead;
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            if (NPCs[Baddie] != this)
            {
                deadCounter++;
            }
        }
    }


    public void PlayerControl()
    {
        
        if (Input.GetKey("w"))
        {
            
            action = NPCAction.Up;
        }
        else if (Input.GetKey("s"))
        {
            
            action = NPCAction.Down;
        }
        else if (Input.GetKey("a"))
        {
            
            action = NPCAction.Left;
        }
        else if (Input.GetKey("d"))
        {
            
            action = NPCAction.Right;
        }
        else
        {
            action = NPCAction.Stop;
        }
        
    }

    public void Movement()
    {
        Vector3 pos = transform.position;
        Vector3 scale = transform.localScale;
        switch (action) 
        { 
            case NPCAction.Up:
                if (pos.y < 9f)
                {
                    pos.y += nobleSpeed * Time.deltaTime;
                }
                break; 
            case NPCAction.Down:
                if (pos.y > -9f)
                {
                    pos.y -= nobleSpeed * Time.deltaTime;
                }
                break;
            case NPCAction.Left:
                if (pos.x > -17f)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    pos.x -= nobleSpeed * Time.deltaTime;
                }
                break; 
            case NPCAction.Right:
                if (pos.x < 17f)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    pos.x += nobleSpeed * Time.deltaTime;
                }
                break;
            case NPCAction.Stop:
                break;
        }
        transform.position = pos;
        if (action == NPCAction.Stop) {
            anim.SetBool("isMove", false);
        }
        else
        {
            anim.SetBool("isMove", true);
        }
        
    }

    //public void scared(Vector2 sourcePos)
    //{
    //    if (state != NPCstate.Dead || state != NPCstate.Dying)
    //    {
    //        scareCountDown = ScaredTime;
    //        Vector2 sourceVector = new Vector2(transform.position.x, transform.position.y) - sourcePos;
    //        sourceVector = sourceVector.normalized;
    //    }
    //}
}
