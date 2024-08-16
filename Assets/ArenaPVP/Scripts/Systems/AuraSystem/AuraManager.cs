using FishNet;
using FishNet.CodeGenerating;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Logger = Assets.Scripts.Helpers.Logger;

public class AuraManager : NetworkBehaviour
{

    private static AuraManager _instance;
    public static AuraManager Instance => _instance;

    [AllowMutableSyncType]
    private SyncDictionary<int, List<AuraInfo>> _playerAurasDict = new SyncDictionary<int, List<AuraInfo>>();

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (InstanceFinder.IsServerStarted)
        {
            UpdateAuraExpiration();
        } 
    }

    [Server]
    private void UpdateAuraExpiration()
    {
        if (!InstanceFinder.IsServerStarted) { return; }

        for (int entryIndex = 0; entryIndex < _playerAurasDict.Count; entryIndex++)
        {
            var entry = _playerAurasDict.ElementAt(entryIndex);
            for (int i = 0; i < entry.Value.Count; i++)
            {
                entry.Value[i].RemainingDuration -= Time.deltaTime;
                _playerAurasDict[entry.Key] = entry.Value; //sync clients
                if (entry.Value[i].RemainingDuration <= 0)
                {
                    RemoveAura(entry.Key, i);
                    i--; //avoid skipping indexes
                }
            }
        }
    }


    [Server]
    public int AddAura(Player source, Player target, AuraBase aura)
    {
        var auraInfo = new AuraInfo(IdentifierService.GetAuraId(), source, target, aura);

        if (_playerAurasDict.ContainsKey(target.Id))
        {
            var playerEntry = _playerAurasDict[target.Id];
            var auraIndex = playerEntry.FindIndex(a => a.AuraId == aura.Id); // && a.AppliedBy == source);
            //aura already applied, refresh duration and addStack if possible
            if (auraIndex >= 0)
            {
                var newStacks = playerEntry[auraIndex].Stacks + 1;
                var existingAura = playerEntry[auraIndex];
                if (playerEntry[auraIndex].MaxStacks >= newStacks)
                {
                    existingAura.Stacks = newStacks;
                    existingAura.RemainingDuration = aura.Duration;
                    playerEntry[auraIndex] = existingAura;
                    _playerAurasDict[target.Id] = playerEntry; //set variable back to dict so it syncs
                    Logger.Log("Server: Refreshed Aura Duration of Aura. Id " + playerEntry[auraIndex].AuraInstanceId);
                }
                return playerEntry[auraIndex].AuraInstanceId;
            }
            else 
            {
                playerEntry.Add(auraInfo);
                _playerAurasDict[target.Id] = playerEntry; //set variable back to dict so it syncs
                AuraAppliedClient(target.Id, auraInfo);
                return auraInfo.AuraInstanceId;
            }
        }
        else 
        {
            _playerAurasDict.Add(target.Id, new List<AuraInfo> { auraInfo });
            AuraAppliedClient(target.Id, auraInfo);
            return auraInfo.AuraInstanceId;
        }
    }
    [Server]
    private void RemoveAura(int playerId, int auraIndex)
    {
        var playeEntry = _playerAurasDict[playerId];
        AbilityStorage.GetAura(playeEntry[auraIndex].AuraId).Fade(playeEntry[auraIndex].AppliedTo, playeEntry[auraIndex].AuraInstanceId);
        AuraRemovedClient(playerId, playeEntry[auraIndex]);

        playeEntry.RemoveAt(auraIndex);
        _playerAurasDict[playerId] = playeEntry;
    }

    [ObserversRpc]
    private void AuraAppliedClient(int targetId, AuraInfo aura)
    {
        //local event so UI can react.
        GameEvents.OnAuraApplied.Invoke(targetId, aura);
    }
    [ObserversRpc]
    private void AuraRemovedClient(int targetId, AuraInfo aura)
    {
        //local event so UI can react.
        GameEvents.OnAuraExpired.Invoke(targetId, aura);
    }

    public List<AuraInfo> GetAuraInfosForPlayer(int playerId)
    {
        if (_playerAurasDict.ContainsKey(playerId))
        {
            return _playerAurasDict[(playerId)];
        }
        return new List<AuraInfo>();
    }
    public AuraInfo GetAuraInfo(int playerId, int auraId)
    {
        if (_playerAurasDict.ContainsKey(playerId))
        {
            if (_playerAurasDict[playerId].Any(a => a.AuraInstanceId == auraId))
            {
                return _playerAurasDict[playerId].First(a => a.AuraInstanceId == auraId);
            }
            return AuraInfo.Null;
        }
        return AuraInfo.Null;
    }

    /// <summary>
    /// Returns Remaining Duration for given PlayerId and AuraId combination. Returns 0 if Aura is expired.
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="auraInstanceId"></param>
    /// <returns></returns>
    public float GetRemainingAuraDuration(int playerId, int auraInstanceId)
    {
        if (_playerAurasDict.ContainsKey(playerId))
        {
            if (!_playerAurasDict[playerId].Any(a => a.AuraInstanceId == auraInstanceId))
                return 0;
            else
            {
                int index = _playerAurasDict[playerId].FindIndex(a => a.AuraInstanceId == auraInstanceId);
                var auraInfo = _playerAurasDict[playerId][index];
                if (auraInfo.RemainingDuration > 0)
                {
                    return auraInfo.RemainingDuration;
                }
                else
                {
                    return 0;
                }
            }
        }
        return 0;
    }


    //this currently implies all auras can only be applied once to a player, maybe in the future change to isUnique Property and apply it once per player.
    public int GetStackAmount(int playerId, int auraId)
    {
        if (_playerAurasDict.ContainsKey(playerId))
        {
            if (!_playerAurasDict[playerId].Any(a => a.AuraId == auraId))
                return 0;
            else
            {
                int index = _playerAurasDict[playerId].FindIndex(a => a.AuraId == auraId);
                return _playerAurasDict[playerId][index].Stacks;
            }
        }
        return 0;
    }

    private void OnDisable()
    {
        _playerAurasDict.Clear();
    }
}



[Serializable]
public class AuraInfo
{
    public AuraInfo() { }
    public AuraInfo(int id, Player appliedById, Player appliedToId, AuraBase aura)
    {
        AuraInstanceId = id;
        AuraId = aura.Id;
        AppliedBy = appliedById;
        AppliedTo = appliedToId;
        Stacks = 1;
        Duration = aura.Duration;
        RemainingDuration = aura.Duration;
        MaxStacks = aura.MaxStacks;
    }

    public int AuraInstanceId;
    public int AuraId;
    public Player AppliedBy;
    public Player AppliedTo;
    public int Stacks;
    public float Duration;
    public float RemainingDuration;
    public int MaxStacks;

    public static AuraInfo Null = null;
}
