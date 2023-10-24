using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPC : MonoBehaviour
{
    public GameObject textObject;
    private static TMP_Text text;
    public GameObject CountDownObj;
    private static TMP_Text CountDownText;
    public static float CountDown;
    public GameObject sniper;
    public GameObject npc;
    public int npcCount;
    public static int Baddie;
    public float SwapRange = 1.5f;
    public static float SwapCoolDown = 5f;
    public int winCount;
    private float SwapCountDown = 0f;
    public static float DyingTime = 3f;
    public static float ScareRange = 5f;
    public static float ScaredTime = 1.5f;
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
    public static float nobleSpeed = 4f;
    public static float scaredSpeed = 6f;
    public static int deadCounter = 0;
    public static bool BaddieWins = false;
    // Start is called before the first frame update

    private void Awake()
    {
        text = textObject.GetComponent<TMP_Text>();
        CountDownText = CountDownObj.GetComponent<TMP_Text>();
        for (int i = 0; i < npcCount; i++)
        {
            Instantiate(npc);
        }
    }

    void Start()
    {
        Baddie = Random.Range(0, NPCs.Count);
        CountDown = SwapCoolDown * 4.5f;
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
        if (NPCs[Baddie].state == NPCstate.Dead && !BaddieWins)
        {
            text.text = "SNIPER WINS";
            text.transform.localScale = Vector3.one * 10f;
            text.color = Color.white;
            sniper.GetComponent<SniperControl>().bulletCount = 99999;
            sniper.GetComponent<SniperControl>().reloadTime = 0.15f;
        }
        else if (deadCounter >= winCount || sniper.GetComponent<SniperControl>().bulletCount <= 0 || BaddieWins)
        {
            text.text = "BADDIE WINS";
            text.transform.localScale = Vector3.one * 10f;
            sniper.GetComponent<SniperControl>().bulletCount = 99999;
            sniper.GetComponent<SniperControl>().reloadTime = 0.15f;
            BaddieWins = true;
        }
        else
        {
            CountDownText.text = CountDown.ToString("0.0");
            CountDown -= Time.deltaTime;
            if (CountDown <= 0)
            {
                NPCs[Baddie].die();
            }
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

    public static T GetRandomEnum<T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(UnityEngine.Random.Range(0, A.Length));
        return V;
    }

    public static void scare(Vector2 pos)
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(pos, ScareRange);
        foreach (Collider2D target in targets)
        {
            target.GetComponent<NPCcontrol>().scared(pos);
        }
        targets = Physics2D.OverlapCircleAll(new Vector2(pos.x + WraparoundCamera.halfViewWidth * 2, pos.y), ScareRange);
        foreach (Collider2D target in targets)
        {
            target.GetComponent<NPCcontrol>().scared(new Vector2(pos.x + WraparoundCamera.halfViewWidth * 2, pos.y));
        }
        targets = Physics2D.OverlapCircleAll(new Vector2(pos.x - WraparoundCamera.halfViewWidth * 2, pos.y), ScareRange);
        foreach (Collider2D target in targets)
        {
            target.GetComponent<NPCcontrol>().scared(new Vector2(pos.x - WraparoundCamera.halfViewWidth * 2, pos.y));
        }
        targets = Physics2D.OverlapCircleAll(new Vector2(pos.x, pos.y + WraparoundCamera.halfViewHeight * 2), ScareRange);
        foreach (Collider2D target in targets)
        {
            target.GetComponent<NPCcontrol>().scared(new Vector2(pos.x, pos.y + WraparoundCamera.halfViewHeight * 2));
        }
        targets = Physics2D.OverlapCircleAll(new Vector2(pos.x, pos.y - WraparoundCamera.halfViewHeight * 2), ScareRange);
        foreach (Collider2D target in targets)
        {
            target.GetComponent<NPCcontrol>().scared(new Vector2(pos.x, pos.y - WraparoundCamera.halfViewHeight * 2));
        }
    }

    public static Collider2D[] WarpedOverlapCircleAll(Vector2 pos, float radius)
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(pos, radius);
        targets = targets.Concat(Physics2D.OverlapCircleAll(new Vector2(pos.x + WraparoundCamera.halfViewWidth * 2, pos.y), radius)).ToArray();
        targets = targets.Concat(Physics2D.OverlapCircleAll(new Vector2(pos.x - WraparoundCamera.halfViewWidth * 2, pos.y), radius)).ToArray();
        targets = targets.Concat(Physics2D.OverlapCircleAll(new Vector2(pos.x, pos.y + WraparoundCamera.halfViewHeight * 2), radius)).ToArray();
        targets = targets.Concat(Physics2D.OverlapCircleAll(new Vector2(pos.x, pos.y - WraparoundCamera.halfViewHeight * 2), radius)).ToArray();
        return targets;
    }
}