using FishNet;
using FishNet.CodeGenerating;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class CastManager : NetworkBehaviour
{
    private static CastManager _instance;
    public static CastManager Instance => _instance;

    [AllowMutableSyncType]
    private SyncDictionary<int, AbilityCastInfo> _castTimerDict = new SyncDictionary<int, AbilityCastInfo>();

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Update()
    {
        if (InstanceFinder.IsServerStarted) 
        {
            UpdateCastTimers();
        }
    }

    [ObserversRpc]
    private void OnCastStartedClient(int ownerId, int abilityId)
    {
        GameEvents.OnCastStarted.Invoke(ownerId, abilityId);
    }

    [Server]
    private void UpdateCastTimers()
    {
        for (int i = 0; i < _castTimerDict.Count; i++)
        {
            var entry = _castTimerDict.ElementAt(i);
            if (entry.Value.CastTimeRemaining > 0f)
            {
                var newValue = entry.Value.CastTimeRemaining - Time.deltaTime;
                _castTimerDict[entry.Key] = new AbilityCastInfo(entry.Value.AbilityId, newValue);
            }
        }
    }

    [Server]
    public void AddOrUpdate(int owner, AbilityCastInfo castInfo)
    {
        if (_castTimerDict.ContainsKey(owner))
        {
            _castTimerDict[owner] = castInfo;
        }
        else 
        {
            _castTimerDict.Add(owner, castInfo);
        }

        OnCastStartedClient(owner, castInfo.AbilityId);
    }
    public AbilityCastInfo? GetCastInfo(int owner)
    {
        if (_castTimerDict.ContainsKey(owner))
        {
            return _castTimerDict[owner];
        }
        return null;
    }
    public bool Contains(int owner, int abilityId)
    {
        if (_castTimerDict.ContainsKey(owner))
        {
            return _castTimerDict[owner].AbilityId == abilityId;
        }
        return false;
    }
    public float GetRemainingCastTime(int owner, int abilityId)
    {
        if (_castTimerDict.ContainsKey(owner))
        {
            return _castTimerDict[owner].CastTimeRemaining;
        }
        return 0f;
    }

    [Server]
    public void Remove(int owner)
    {
        if (_castTimerDict.ContainsKey(owner))
        {
            _castTimerDict.Remove(owner);
        }
    }

    public void StartCastCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
