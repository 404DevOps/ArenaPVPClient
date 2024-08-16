using Assets.ArenaPVP.Scripts.Models.Enums;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;

public class StatsMediator : NetworkBehaviour
{
    readonly SyncList<StatModifier> modifiers = new();

    public event EventHandler<StatQuery> Queries;
    public void PerformQuery(object sender, StatQuery query) => Queries?.Invoke(sender, query);

    [Server]
    public void AddModifier(StatModifier modifier)
    {
        modifiers.Add(modifier);
        Queries += modifier.Handle;
    }

    [Server]
    public void RemoveAuraModifiers(int auraId)
    {
        for (int i = 0; i < modifiers.Count; i++)
        {
            var node = modifiers[i];
            if (node.SourceAuraId == auraId)
            {
                Queries -= node.Handle;
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