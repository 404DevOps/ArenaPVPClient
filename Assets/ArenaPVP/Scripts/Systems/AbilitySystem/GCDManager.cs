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
using Logger =Assets.ArenaPVP.Scripts.Helpers.ArenaLogger;

public class GCDManager : NetworkBehaviour
{
    private static GCDManager _instance;
    public static GCDManager Instance => _instance;

    private Dictionary<int, float> _gcdDict = new Dictionary<int, float>(); //playerId, 
    public static float GCD_TIME = 1.5f;

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
            UpdateGCDTimers();
        }
    }

    [ObserversRpc]
    private void OnGCDStartedClient(int ownerId, float duration)
    {
        GameEvents.OnGCDStarted.Invoke(ownerId, duration);
    }

    [Server]
    private void UpdateGCDTimers()
    {
        for (int i = 0; i < _gcdDict.Count; i++)
        {
            var entry = _gcdDict.ElementAt(i);
            if (entry.Value > 0f)
            {
                var newValue = entry.Value - Time.deltaTime;
                _gcdDict[entry.Key] = newValue;
            }
            else 
            {
                _gcdDict.Remove(entry.Key);
            }
        }
    }

    [Server]
    public void AddOrUpdate(int owner)
    {
        if (_gcdDict.ContainsKey(owner))
        {
            _gcdDict[owner] = GCD_TIME;
        }
        else 
        {
            _gcdDict.Add(owner, GCD_TIME);
        }

        OnGCDStartedClient(owner, GCD_TIME);
    }
    public bool IsOnGCD(int owner)
    {
        return _gcdDict.ContainsKey(owner) && _gcdDict[owner] > 0f;
    }

    [Server]
    public void Remove(int owner)
    {
        if (_gcdDict.ContainsKey(owner))
        {
            _gcdDict.Remove(owner);
        }
    }

    private void OnDisable()
    {
        _gcdDict.Clear();
    }
}
