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
    public float EnergyTickValue;
    public float ManaTickValue;
    public float FocusTickValue;

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
            float resourceValue = 0;
            foreach (var entry in _playerResourceDict)
            {
                switch (entry.Value)
                {
                    case ResourceType.Mana: resourceValue = ManaTickValue; break;
                    case ResourceType.Energy: resourceValue = EnergyTickValue; break;
                    case ResourceType.Focus: resourceValue = FocusTickValue; break;
                    case ResourceType.Adrenaline: continue;
                }

                GameEvents.OnPlayerResourceChanged.Invoke(new ResourceChangedEventArgs() { Player = entry.Key, ResourceChangeAmount = resourceValue });
                
            }
            Logger.Log("Resource Tick processed.");
            _timer = 0;
        }
    }
}
