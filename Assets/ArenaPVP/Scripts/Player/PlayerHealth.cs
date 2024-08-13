using FishNet.CodeGenerating;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logger = Assets.Scripts.Helpers.Logger;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField][AllowMutableSyncType] public  SyncVar<float> MaxHealth = new SyncVar<float>();
    [SerializeField][AllowMutableSyncType] public  SyncVar<float> CurrentHealth = new SyncVar<float>();

    public Player Player;

    [Server]
    public void UpdateHealthServer(HealthChangedEventArgs args)
    {
        CurrentHealth.Value += args.HealthChangeAmount;
        if (CurrentHealth.Value > MaxHealth.Value)
            CurrentHealth.Value = MaxHealth.Value;

        HealthUpdatedClient(args);
    }

    [ObserversRpc]
    public void HealthUpdatedClient(HealthChangedEventArgs args)
    {
        GameEvents.OnPlayerHealthChanged.Invoke(args);
    }

    private void Awake()
    {
        Player = GetComponent<Player>();
        MaxHealth.Value = Player.Stats.Health;
        CurrentHealth.Value = Player.Stats.Health;
    }
}
