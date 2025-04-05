using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilityStorage : MonoBehaviour
{
    public static List<AbilityBase> AllAbilities;
    public static List<AuraBase> AllAuras;

    public void Awake()
    {
        LoadAbilities();
        LoadAuras();
        ConsistencyCheck();
    }

    private void ConsistencyCheck()
    {
        foreach (var ability in AllAbilities)
        {
            if (AllAbilities.Count(a => a.Id == ability.Id) > 1)
            {
                throw new InvalidOperationException("More than one Ability have the same ID " + ability.Id);
            }
        }
        foreach (var aura in AllAuras)
        {
            if (AllAuras.Count(a => a.Id == aura.Id) > 1)
            {
                throw new InvalidOperationException("More than one Aura have the same ID " + aura.Id);
            }
        }
    }

    private void LoadAbilities()
    {
        AllAbilities = new List<AbilityBase>();
        var abilities = Resources.LoadAll("Abilities/", typeof(AbilityBase));
        foreach (var ability in abilities)
        {
            AllAbilities.Add((AbilityBase)ability);
        }
    }
    private void LoadAuras()
    {
        AllAuras = new List<AuraBase>();
        var abilities = Resources.LoadAll("Aura/", typeof(AuraBase));
        foreach (var ability in abilities)
        {
            AllAuras.Add((AuraBase)ability);
        }
    }


    public static AuraBase GetAura(int auraId)
    {
        var result = AllAuras.FirstOrDefault(a => a.Id == auraId);
        if (result == default)
            throw new ArgumentOutOfRangeException("Could not find Aura with Id " + auraId);
        else
            return result;

    }

    public static AbilityBase GetAbility(int abilityId)
    {
        var result = AllAbilities.FirstOrDefault(a => a.Id == abilityId);
        if (result == default)
            throw new ArgumentOutOfRangeException("Could not find Aura with Id " + abilityId);
        else
            return result;

    }
}
