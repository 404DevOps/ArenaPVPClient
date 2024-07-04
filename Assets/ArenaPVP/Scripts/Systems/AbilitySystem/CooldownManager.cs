using FishNet.Editing;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CooldownManager : MonoBehaviour
{
    private static CooldownManager _instance;
    public static CooldownManager Instance => _instance;
    private Dictionary<string, float> abilityLastUsedDictionary = new Dictionary<string, float>();
    // Start is called before the first frame update
    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);
    }

    public void AddOrUpdate(AbilityWithOwner abilityWithOwner)
    {
        //store identifier and lastTime used.
        if (Contains(abilityWithOwner.Identifier))
            abilityLastUsedDictionary[abilityWithOwner.Identifier] = Time.time;
        else
            abilityLastUsedDictionary.Add(abilityWithOwner.Identifier, Time.time);
    }
    public float TimeSinceLastUse(AbilityWithOwner ability)
    {
        if (abilityLastUsedDictionary.ContainsKey(ability.Identifier))
        {
            var lastUsedTime = abilityLastUsedDictionary[ability.Identifier];
            var timeSinceLastUse = Time.time - lastUsedTime;
            return timeSinceLastUse;
        }
        else
        {
            return 0f;
        }
    }
    public void Remove(AbilityWithOwner ability)
    {
        abilityLastUsedDictionary.Remove(ability.Identifier);
    }

    public bool Contains(AbilityWithOwner ability)
    {
        return abilityLastUsedDictionary.ContainsKey(ability.Identifier);
    }
    public bool Contains(string identifier)
    {
        return abilityLastUsedDictionary.ContainsKey(identifier);
    }
}


