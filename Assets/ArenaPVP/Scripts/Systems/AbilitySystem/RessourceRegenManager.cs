using Assets.ArenaPVP.Scripts.Enums;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using Logger =Assets.ArenaPVP.Scripts.Helpers.ArenaLogger;

public class RessourceRegenManager : NetworkBehaviour
{
    private Dictionary<Player, ResourceType> _playerResourceDict = new Dictionary<Player, ResourceType>();
    
    public float TickInterval = 2;
    private float _timer = 0;

    public override void OnStartServer()
    {
        GameEvents.OnPlayerStatsInitialized.AddListener(AddPlayer);
    }
    public override void OnStopServer()
    {
        GameEvents.OnPlayerStatsInitialized.RemoveListener(AddPlayer);
    }
    public override void OnStartClient()
    {
        this.gameObject.SetActive(false);
    }

    [Server]
    private void AddPlayer(Player player)
    {
        var resourceType = AppearanceData.Instance().GetRessourceType(player.ClassType);
        _playerResourceDict.Add(player, resourceType);
    }

    private void Update()
    {
        if (InstanceFinder.IsServerStarted)
        {
            _timer += Time.deltaTime;
            if (_timer >= TickInterval)
            {
                foreach (var entry in _playerResourceDict)
                {
                    entry.Key.GetComponent<PlayerResource>().UpdateResourceServer(new ResourceChangedEventArgs() { Player = entry.Key, ResourceChangeAmount = entry.Key.GetComponent<PlayerStats>().ResourceRegenerationRate });
                }
                _timer = 0;
            }
        }
    }
}
