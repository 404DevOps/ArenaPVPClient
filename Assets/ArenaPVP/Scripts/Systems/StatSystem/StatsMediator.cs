using Assets.ArenaPVP.Scripts.Models.Enums;
using FishNet.CodeGenerating;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Logger =Assets.ArenaPVP.Scripts.Helpers.ArenaLogger;

public class StatsMediator : NetworkBehaviour
{
    [AllowMutableSyncType]
    [SerializeField]
    private SyncList<StatModifier> modifiers = new();

    public void PerformQuery(object sender, StatQuery query) 
    {
        foreach (StatModifier mod in modifiers) 
        {
            query.Value += mod.Modify(query);
        }
    }

    [Server]
    public void AddModifierServer(StatModifier modifier)
    {
        modifiers.Add(modifier);
    }

    [Server]
    public void RemoveModifiersServer(int auraId)
    {
        for (int i = 0; i < modifiers.Count; i++)
        {
            var node = modifiers[i];
            if (node.SourceAuraId == auraId)
            {
                modifiers.RemoveAt(i);
                node.Dispose();
                if (i > 0)
                {
                    i--; //since index shifts on a remove action, we count it back to make sure we dont skip any element.
                }
            }
        }
    }
}

[Serializable]
public class StatQuery
{
    public readonly StatType StatType;
    public float Value;
    public float BaseValue;

    public StatQuery(StatType statType, float value)
    {
        StatType = statType;
        Value = value;
        BaseValue = value;
    }
}