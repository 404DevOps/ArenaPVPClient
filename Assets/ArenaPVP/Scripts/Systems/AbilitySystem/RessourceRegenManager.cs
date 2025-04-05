using Assets.ArenaPVP.Scripts.Enums;
using FishNet;
using FishNet.Object;
using System.Collections.Generic;
using UnityEngine;

public class RessourceRegenManager : NetworkBehaviour
{
    private Dictionary<Player, ResourceContext> _playerResourceDict = new Dictionary<Player, ResourceContext>();
    
    public float TickInterval = 2;
    private float _timer = 0;

    public override void OnStartServer()
    {
        ServerEvents.OnPlayerStatsInitialized.AddListener(AddPlayer);
    }
    public override void OnStopServer()
    {
        ServerEvents.OnPlayerStatsInitialized.RemoveListener(AddPlayer);
    }
    public override void OnStartClient()
    {
        this.gameObject.SetActive(false);
    }

    [Server]
    private void AddPlayer(Player player)
    {
        var resourceType = AppearanceData.Instance().GetRessourceType(player.ClassType);
        _playerResourceDict.Add(player, new ResourceContext(player.GetComponent<PlayerResource>(), player.GetComponent<PlayerStats>(), resourceType));
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
    public ResourceContext(PlayerResource playerResource, PlayerStats playerStats, ResourceType type)
    {
        ResourceType = type;
        PlayerResource = playerResource;
        PlayerStats = playerStats;
    }

    public ResourceType ResourceType;
    public PlayerResource PlayerResource;
    public PlayerStats PlayerStats;
}
