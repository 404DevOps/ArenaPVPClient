using FishNet.Editing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public static class IdentifierService
{
    private static int nextPlayerId = 0;
    private static int nextAuraId = 0;

    public static int GetPlayerId() 
    {
        nextPlayerId++;
        return nextPlayerId;
    }

    public static int GetAuraId()
    {
        nextAuraId++;
        return nextAuraId;
    }
}
