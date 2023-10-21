using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPC : MonoBehaviour
{


    public static int Baddie;
    public float SwapRange;
    public float SwapCoolDown;
    private float SwapCountDown = 0f;
    public static List<NPC> NPCs = new List<NPC>();
    public enum NPCstate
    {
        Noble,
        Scared,
        Dying,
        Dead
    }
    public static float nobleSpeed = 5f;
    public static float scaredSpeed = 10f;
    public static int dead;
    // Start is called before the first frame update
    void Start()
    {
        Baddie = Random.Range(0, NPCs.Count);
    }

    // Update is called once per frame
    void Update()
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
                if (i != Baddie)
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
                Baddie = closest;
            }
        }
    }
}