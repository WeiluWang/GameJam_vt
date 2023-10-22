using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class NPCcontrol : NPC
{
    public NPCstate state;
    public NPCAction action;
    public AudioSource deathAudio;
    public AudioSource scareAudio;
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
            else if (state != NPCstate.Scared)
            {
                BotControl();
            }
            Movement();
            if (state != NPCstate.Dying && scareCountDown > 0f)
            {
                state = NPCstate.Scared;
                scareCountDown -= Time.deltaTime;
            }
            if (state == NPCstate.Scared && scareCountDown <= 0f)
            {
                state = NPCstate.Noble;
            }
        }
    }

    public void die()
    {
        if (state != NPCstate.Dead)
        {
            anim.SetBool("isShooted", true);
            anim.SetBool("isScared", false);

            Invoke("hide", 1.6f);
            deathAudio.Play();
            state = NPCstate.Dead;


            scare(transform.position);

            if (NPCs[Baddie] != this)
            {
                deadCounter++;
            }
        }
    }

    public void jump()
    {
        if (state == NPCstate.Noble)
        {
            action = NPCAction.Stop;
            BotActionTime = 0.1f;
            anim.SetBool("isJump", true);
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
        Vector2 inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (inputVector == Vector2.zero)
        {
            action = NPCAction.Stop;
        }
        else
        {
            action = GetNearestCardinalDirection(inputVector);
        }
        Collider2D[] targets = WarpedOverlapCircleAll(transform.position, 5f);
        if (Input.GetKey("e") && state == NPCstate.Noble)
        {
            jump();
            foreach (Collider2D target in targets)
            {
                if (target.GetComponent<NPCcontrol>().state == NPCstate.Noble)
                {
                    target.GetComponent<NPCcontrol>().jump();
                }
            }
        }
        else
        {
            stopJump();
            foreach (Collider2D target in targets)
            {
                target.GetComponent<NPCcontrol>().stopJump();
            }
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
        float speed = nobleSpeed;
        if (state == NPCstate.Scared)
        {
            speed = scaredSpeed;
        }
        switch (action)
        {
            case NPCAction.Up:
                pos.y += speed * Time.deltaTime;
                break;
            case NPCAction.Down:
                pos.y -= speed * Time.deltaTime;
                break;
            case NPCAction.Left:
                transform.localScale = new Vector3(1, 1, 1);
                pos.x -= speed * Time.deltaTime;
                break;
            case NPCAction.Right:
                transform.localScale = new Vector3(-1, 1, 1);
                pos.x += speed * Time.deltaTime;
                break;
            case NPCAction.Stop:
                break;
        }
        transform.position = pos;
        WraparoundCamera.current.WrapMeIfNeeded(GetComponent<SpriteRenderer>());
        if (action == NPCAction.Stop)
        {
            anim.SetBool("isMove", false);
        }
        else
        {
            anim.SetBool("isMove", true);
        }
        if (state == NPCstate.Scared)
        {
            anim.SetBool("isScared", true);
        }
        else
        {
            anim.SetBool("isScared", false);
        }
    }


    public NPCAction GetNearestCardinalDirection(Vector2 vector)
    {
        // Calculate the angles between the input vector and the cardinal direction vectors
        float angleToUp = Vector2.Angle(vector, Vector2.up);
        float angleToDown = Vector2.Angle(vector, Vector2.down);
        float angleToLeft = Vector2.Angle(vector, Vector2.left);
        float angleToRight = Vector2.Angle(vector, Vector2.right);

        // Find the minimum angle to determine the nearest cardinal direction
        float minAngle = Mathf.Min(angleToUp, angleToDown, angleToLeft, angleToRight);

        if (minAngle == angleToUp)
        {
            return NPCAction.Up;
        }
        else if (minAngle == angleToDown)
        {
            return NPCAction.Down;
        }
        else if (minAngle == angleToLeft)
        {
            return NPCAction.Left;
        }
        else
        {
            return NPCAction.Right;
        }
    }

    public void scared(Vector2 sourcePos)
    {
        if (state != NPCstate.Dead && state != NPCstate.Dying)
        {
            scareAudio.Play();
            stopJump();
            scareCountDown = ScaredTime;
            state = NPCstate.Scared;
            action = GetNearestCardinalDirection(new Vector2(transform.position.x, transform.position.y) - sourcePos);
        }
    }
}
