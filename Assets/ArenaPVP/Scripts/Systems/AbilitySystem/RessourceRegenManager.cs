using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logger = Assets.Scripts.Helpers.Logger;

public class RessourceRegenManager : MonoBehaviour
{
    private Dictionary<Player, ResourceType> _playerResourceDict = new Dictionary<Player, ResourceType>();
    
    public float TickInterval;
    private float _timer = 0;


    private void OnEnable()
    {
        GameEvents.OnPlayerInitialized.AddListener(AddPlayer);
    }

    private void AddPlayer(Player player)
    {
        var resourceType = AppearanceData.Instance().GetRessourceType(player.ClassType);
        _playerResourceDict.Add(player, resourceType);
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= TickInterval)
        {
            foreach (var entry in _playerResourceDict)
            {
                GameEvents.OnPlayerResourceChanged.Invoke(new ResourceChangedEventArgs() { Player = entry.Key, ResourceChangeAmount = entry.Key.Stats.ResourceRegenerationRate });
            }
            //Logger.Log("Resource Tick processed.");
            _timer = 0;
        }
    }
}
