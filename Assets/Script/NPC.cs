using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public static NPC Baddie;
    public static List<NPC> NPCs;
    public enum NPCstate
    {
        Noble,
        Scared,
        Dying,
        Dead
    }
    public float nobleSpeed;
    public float scaredSpeed;
    public int dead;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
