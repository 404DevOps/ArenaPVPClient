using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using Logger = Assets.Scripts.Helpers.Logger;

public class PlayerResource : NetworkBehaviour
{
    public readonly SyncVar<float> MaxResource = new SyncVar<float>();
    public readonly SyncVar<float> CurrentResource = new SyncVar<float>();

    public Player Player;

    void Awake()
    {
        Player = GetComponent<Player>();
        MaxResource.Value = Player.Stats.Resource;
        CurrentResource.Value = Player.Stats.Resource;
    }

    [ServerRpc]
    public void UpdateResourceServer(ResourceChangedEventArgs args)
    {
        Logger.Log("UpdateResource Server called.");

        CurrentResource.Value += args.ResourceChangeAmount;
        if (CurrentResource.Value > MaxResource.Value)
            CurrentResource.Value = MaxResource.Value;

        ResourceUpdated(args);
    }

    [ObserversRpc]
    public void ResourceUpdated(ResourceChangedEventArgs args)
    {
        GameEvents.OnPlayerResourceChanged.Invoke(args);
    }
}
