using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPC : MonoBehaviour
{


    public static int Baddie;
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
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Baddie = Random.Range(0, NPCs.Count);
        }
    }
}