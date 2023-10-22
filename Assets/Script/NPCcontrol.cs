using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class NPCcontrol : NPC
{
    public NPCstate state;
    public NPCAction action;
    public float dyingCountDown;
    public float scareCountDown;
    [SerializeField] private Animator anim;
    //[SerializeField] public bool isMoving = false;
    [SerializeField] public bool isScared = false;
    public float BotActionTime = 0;
    void Awake()
    {
        NPCs.Add(this);
        state = NPCstate.Noble;
        action = NPCAction.Stop;
        anim = GetComponent<Animator>();
        GetComponent<SpriteRenderer>().enabled = true;
        dyingCountDown = DyingTime;
        scareCountDown = 0;
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
            else
            {
                BotControl();
            }
            Movement();
        }
    }

    public void die()
    {
        if (state != NPCstate.Dead)
        {
            anim.SetBool("isShooted", true);

            Invoke("hide", 1.6f);

            state = NPCstate.Dead;
            
            //transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            if (NPCs[Baddie] != this)
            {
                deadCounter++;
            }
        }
    }

    public void jump() { 
         if (state != NPCstate.Dead)
        {
            anim.SetBool("isJump", true);
            Invoke("stopJump", 2f);
        }
    
    }

    private void hide()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
    private void stopJump()
    {
        anim.SetBool("isJump", false);
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
        else if (Input.GetKeyDown("e"))
        {
            action = NPCAction.Stop;
            jump();
                Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, 5f);
                foreach (Collider2D target in targets)
                {
                    target.GetComponent<NPCcontrol>().jump();
                }
            Invoke("stopJump", 2f);
            
        }
        else
        {
            action = NPCAction.Stop;
        }
        
    }

    public void BotControl() 
    {
        BotActionTime -= Time.deltaTime;
        if (BotActionTime <= 0f)
        {
            action = GetRandomEnum<NPCAction>();
            BotActionTime = Random.Range(0.1f, 1.5f);
        }
    }

    public void Movement()
    {
        Vector3 pos = transform.position;
        Vector3 scale = transform.localScale;
        switch (action) 
        { 
            case NPCAction.Up:
                pos.y += nobleSpeed * Time.deltaTime;
                break; 
            case NPCAction.Down:
                pos.y -= nobleSpeed * Time.deltaTime;
                break;
            case NPCAction.Left:
                transform.localScale = new Vector3(1, 1, 1);
                pos.x -= nobleSpeed * Time.deltaTime;
                break; 
            case NPCAction.Right:
                transform.localScale = new Vector3(-1, 1, 1);
                pos.x += nobleSpeed * Time.deltaTime;
                break;
            case NPCAction.Stop:
                break;
        }
        transform.position = pos;
        WraparoundCamera.current.WrapMeIfNeeded(GetComponent<SpriteRenderer>());
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



            //if (isScared == true)
            //{
            //    anim.SetBool("isScared", true);
            //}
            //count down 3 seconds
            //anim.SetBool("isScared", false);
    //    }
    //}



}
