using Assets.ArenaPVP.Scripts.Enums;
using FishNet;
using FishNet.Object;
using System.Collections.Generic;
using UnityEngine;

public class RessourceRegenManager : NetworkBehaviour
{
    private Dictionary<Entity, ResourceContext> _entityResourceDict = new Dictionary<Entity, ResourceContext>();
    
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
        _entityResourceDict.Add(player, new ResourceContext(player.GetComponent<EntityResource>(), player.GetComponent<EntityStats>(), resourceType));
    }

    private void Update()
    {
        if (InstanceFinder.IsServerStarted)
        {
            _timer += Time.deltaTime;
            if (_timer >= TickInterval)
            {
                foreach (var entry in _entityResourceDict)
                {
                    entry.Value.EntityResource.UpdateResourceServer(new ResourceChangedEventArgs() { Entity = entry.Key, ResourceChangeAmount = entry.Value.EntityStats.ResourceRegenerationRate });
                    entry.Value.EntityStamina.UpdateStaminaServer(new StaminaChangedEventArgs() { Entity = entry.Key, StaminaChangedAmount = entry.Value.EntityStats.StaminaRegenerationRate });
                }
                _timer = 0;
            }
        }
    }
}

public class ResourceContext
{
    public ResourceContext(EntityResource entityResource, EntityStats entityStats, ResourceType type)
    {
        ResourceType = type;
        EntityResource = entityResource;
        EntityStats = entityStats;
    }

    public ResourceType ResourceType;
    public EntityResource EntityResource;
    public EntityStamina EntityStamina;
    public EntityStats EntityStats;
}
