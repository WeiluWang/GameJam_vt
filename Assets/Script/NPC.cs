using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPC : MonoBehaviour
{
    public GameObject textObject;
    private static TMP_Text text;
    public static int Baddie;
    public static float SwapRange = 1f;
    public static float SwapCoolDown = 10f;
    private static float SwapCountDown = 0f;
    public static float DyingTime = 3f;
    public static float ScaredTime = 1f;
    public static List<NPCcontrol> NPCs = new List<NPCcontrol>();
    public enum NPCstate
    {
        Noble,
        Scared,
        Dying,
        Dead
    }
    public enum NPCAction
    {
        Up,
        Down,
        Left,
        Right,
        Stop
    }
    public static float nobleSpeed = 5f;
    public static float scaredSpeed = 10f;
    public static int deadCounter = 0;
    // Start is called before the first frame update

    private void Awake()
    {
        text = textObject.GetComponent<TMP_Text>();
    }

    void Start()
    {
        Baddie = Random.Range(0, NPCs.Count);
    }

    // Update is called once per frame
    void Update()
    {
        score();
        swap();
    }

    // Scoring or win text
    public void score()
    {
        if (NPCs[Baddie].state == NPCstate.Dead)
        {
            text.text = "SNIPER WINS";
            text.transform.localScale = Vector3.one * 10f;
            text.color = Color.white;
        }
        else if (deadCounter >= 20)
        {
            text.text = "BADDIE WINS";
            text.transform.localScale = Vector3.one * 10f;
        }
        else
        {
            text.text = deadCounter.ToString();
            text.transform.localScale = Vector3.one * 5f + new Vector3(Mathf.Sqrt(deadCounter), Mathf.Sqrt(deadCounter), Mathf.Sqrt(deadCounter)) * 10f;
        }
    }

    // Swaping
    public void swap()
    {
        if (SwapCountDown >= 0f)
        {
            SwapCountDown -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && SwapCountDown <= 0f)
        {
            SwapCountDown = SwapCoolDown;
            float minimum = float.PositiveInfinity;
            int closest = -1;
            for (int i = 0; i < NPCs.Count; i++)
            {
                if (i != Baddie && NPCs[i].state != NPCstate.Dead && NPCs[i].state != NPCstate.Dying)
                {
                    float distance = Vector3.Distance(NPCs[i].transform.position, NPCs[Baddie].transform.position);
                    if (distance < SwapRange && distance < minimum)
                    {
                        closest = i;
                        minimum = distance;
                    }
                }
            }
            if (closest != -1)
            {
                NPCs[Baddie].state = NPCstate.Dying;
                Baddie = closest;
            }
        }
    }
    
    //public static void scare(Vector2 pos)
    //{
    //    Collider2D[] targets = Physics2D.OverlapCircleAll(pos, 5f);
    //    foreach (Collider2D target in targets)
    //    {
    //        target.GetComponent<NPCcontrol>().scared(pos);
    //    }
    //}
}