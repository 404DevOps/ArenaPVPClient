using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ActionBarMapping
{
    [SerializeField]
    private List<ActionBarMappingEntry> mappings;

    public ActionBarMapping() { 
        mappings = new List<ActionBarMappingEntry> { };
    }

    public AbilityBase GetSlot(int slotId)
    {
        var map = mappings.FirstOrDefault(m => m.SlotId == slotId);
        if (map != null)
            return map.Ability;
        else return null;
    }
    public void AddOrUpdateSlot(int slotId, AbilityBase ability)
    {
        if (mappings.Any(m => m.SlotId == slotId))
        {
            mappings.FirstOrDefault(m => m.SlotId == slotId).Ability = ability;
        }
        else
        {
            mappings.Add(new ActionBarMappingEntry(slotId, ability));
        }
    }
}

[Serializable]
public class ActionBarMappingEntry
{
    public ActionBarMappingEntry(int slotId, AbilityBase ability)
    { 
        SlotId = slotId;
        Ability = ability;
    }
    public int SlotId;
    public AbilityBase Ability;
}