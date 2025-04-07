using Assets.ArenaPVP.Scripts.Enums;
using FishNet;
using FishNet.Object;
using System.Collections.Generic;
using UnityEngine;

public class RessourceRegenManager : NetworkBehaviour
{
    private Dictionary<Entity, ResourceContext> _playerResourceDict = new Dictionary<Entity, ResourceContext>();
    
    public float TickInterval = 2;
    private float _timer = 0;

    public override void OnStartServer()
    {
        ServerEvents.OnEntityStatsInitialized.AddListener(AddPlayer);
    }
    public override void OnStopServer()
    {
        ServerEvents.OnEntityStatsInitialized.RemoveListener(AddPlayer);
    }
    public override void OnStartClient()
    {
        this.gameObject.SetActive(false);
    }

    [Server]
    private void AddPlayer(Entity player)
    {
        var resourceType = AppearanceData.Instance().GetRessourceType(player.ClassType);
        _playerResourceDict.Add(player, new ResourceContext(player.GetComponent<EntityResource>(), player.GetComponent<EntityStats>(), resourceType));
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
                    entry.Value.PlayerResource.UpdateResourceServer(new ResourceChangedEventArgs() { Player = entry.Key, ResourceChangeAmount = entry.Value.PlayerStats.ResourceRegenerationRate });
                }
                _timer = 0;
            }
        }
    }
}

public class ResourceContext
{
    public ResourceContext(EntityResource playerResource, EntityStats playerStats, ResourceType type)
    {
        ResourceType = type;
        PlayerResource = playerResource;
        PlayerStats = playerStats;
    }

    public ResourceType ResourceType;
    public EntityResource PlayerResource;
    public EntityStats PlayerStats;
}
