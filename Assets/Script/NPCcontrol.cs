using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCcontrol : NPC
{
    public NPCstate state;
    private void Awake()
    {
        NPCs.Add(this);
    }
}
