using FishNet.CodeGenerating;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logger =Assets.ArenaPVP.Scripts.Helpers.ArenaLogger;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField][AllowMutableSyncType] public  SyncVar<float> MaxHealth = new SyncVar<float>();
    [SerializeField][AllowMutableSyncType] public  SyncVar<float> CurrentHealth = new SyncVar<float>();

    public PlayerStats Stats;

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

    public override void OnStartServer()
    {
        base.OnStartServer();
        Stats = GetComponent<PlayerStats>();
        MaxHealth.Value = Stats.MaxHealth;
        CurrentHealth.Value = Stats.MaxHealth;
    }
}
