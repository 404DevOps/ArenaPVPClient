using FishNet.Object;
using FishNet.Object.Synchronizing;
using System;
using UnityEngine;
using Logger =Assets.ArenaPVP.Scripts.Helpers.ArenaLogger;

public class EntityResource : NetworkBehaviour
{
    public readonly SyncVar<float> MaxResource = new SyncVar<float>();
    public readonly SyncVar<float> CurrentResource = new SyncVar<float>();

    private EntityStats Stats;

    [Server]
    public void UpdateResourceServer(ResourceChangedEventArgs args)
    {
        CurrentResource.Value += args.ResourceChangeAmount;
        if (CurrentResource.Value > MaxResource.Value)
            CurrentResource.Value = MaxResource.Value;

        ResourceUpdatedClient(args);
    }

    [ObserversRpc]
    public void ResourceUpdatedClient(ResourceChangedEventArgs args)
    {
        ClientEvents.OnEntityResourceChanged.Invoke(args);
    }

    internal void Initialize()
    {
        Stats = GetComponent<EntityStats>();
        MaxResource.Value = Stats.MaxResource;
        CurrentResource.Value = Stats.MaxResource;
    }
}
