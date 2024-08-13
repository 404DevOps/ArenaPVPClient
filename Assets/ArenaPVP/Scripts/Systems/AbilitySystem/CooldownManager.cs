using FishNet;
using FishNet.CodeGenerating;
using FishNet.Editing;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEditor.Playables;
using UnityEngine;
using static UnityEngine.UI.Image;

public class CooldownManager : NetworkBehaviour
{
    private static CooldownManager _instance;
    public static CooldownManager Instance => _instance;
    [AllowMutableSyncType]
    private SyncDictionary<string, float> _abilityCooldownDictionary = new SyncDictionary<string, float>();
    // Start is called before the first frame update
    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);
    }

    public void Update()
    {
        if (InstanceFinder.IsServerStarted)
        {
            UpdateAbilityCooldowns();
        }
    }

    [Server]
    private void UpdateAbilityCooldowns()
    {
        for (int i = 0; i < _abilityCooldownDictionary.Count; i++)
        {
            var ability = _abilityCooldownDictionary.ElementAt(i);
            //only countdown if cooldown isnt 0 already.
            if (ability.Value > 0)
            {
                var newValue = ability.Value - Time.deltaTime;
                _abilityCooldownDictionary[ability.Key] = newValue;
            }
        }
    }

    [Server]
    public void AddOrUpdate(AbilityCooldownInfo abilityCDInfo)
    {
        //store identifier and lastTime used.
        if (Contains(abilityCDInfo.Identifier))
            _abilityCooldownDictionary[abilityCDInfo.Identifier] = abilityCDInfo.CooldownRemaining;
        else
            _abilityCooldownDictionary.Add(abilityCDInfo.Identifier, abilityCDInfo.CooldownRemaining);

        OnCooldownStartedClient(abilityCDInfo.OwnerId, abilityCDInfo.AbilityId);
    }

    [ObserversRpc]
    public void OnCooldownStartedClient(int ownerId, int abilityId)
    {
        GameEvents.OnCooldownStarted.Invoke(ownerId, abilityId);
    }

    public float GetRemainingCooldown(AbilityCooldownInfo ability)
    {
        if (_abilityCooldownDictionary.ContainsKey(ability.Identifier))
        {
            return _abilityCooldownDictionary[ability.Identifier];
        }
        else
        {
            return 0f;
        }
    }

    [Server]
    public void Remove(AbilityCooldownInfo ability)
    {
        _abilityCooldownDictionary.Remove(ability.Identifier);
    }

    public bool Contains(AbilityCooldownInfo ability)
    {
        return _abilityCooldownDictionary.ContainsKey(ability.Identifier);
    }
    public bool Contains(string identifier)
    {
        return _abilityCooldownDictionary.ContainsKey(identifier);
    }
}


