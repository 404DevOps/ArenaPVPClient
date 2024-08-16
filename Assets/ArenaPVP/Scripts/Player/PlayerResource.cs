using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using Logger = Assets.Scripts.Helpers.Logger;

public class PlayerResource : NetworkBehaviour
{
    public readonly SyncVar<float> MaxResource = new SyncVar<float>();
    public readonly SyncVar<float> CurrentResource = new SyncVar<float>();

    public PlayerStats Stats;

    public override void OnStartServer()
    {
        Stats = GetComponent<PlayerStats>();
        MaxResource.Value = Stats.MaxResource;
        CurrentResource.Value = Stats.MaxResource;
    }

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
        GameEvents.OnPlayerResourceChanged.Invoke(args);
    }
}
