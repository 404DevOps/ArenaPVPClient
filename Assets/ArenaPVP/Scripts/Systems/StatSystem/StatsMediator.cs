using Assets.ArenaPVP.Scripts.Models.Enums;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;

public class StatsMediator
{
    readonly LinkedList<StatModifier> modifiers = new();

    public event EventHandler<StatQuery> Queries;
    public void PerformQuery(object sender, StatQuery query) => Queries?.Invoke(sender, query);

    public void AddModifier(StatModifier modifier)
    {
        modifiers.AddLast(modifier);
        Queries += modifier.Handle;
    }

    public void RemoveAuraModifiers(int auraId)
    {
        var node = modifiers.First;
        while (node != null)
        {
            var nextNode = node.Next;
            if (node.Value.SourceAuraId == auraId)
            {
                modifiers.Remove(node);
                Queries -= node.Value.Handle;
                node.Value.Dispose();
            }
            node = nextNode;
        }
    }
}

public class StatQuery
{
    public readonly StatType StatType;
    public float Value;

    public StatQuery(StatType statType, float value)
    {
        StatType = statType;
        Value = value;
    }
}