using FishNet.CodeGenerating;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logger =Assets.ArenaPVP.Scripts.Helpers.ArenaLogger;

public class EntityHealth : NetworkBehaviour
{
    [SerializeField][AllowMutableSyncType] public  SyncVar<float> MaxHealth = new SyncVar<float>();
    [SerializeField][AllowMutableSyncType] public  SyncVar<float> CurrentHealth = new SyncVar<float>();

    private EntityStats Stats;

    [Server]
    public void UpdateHealthServer(HealthChangedEventArgs args)
    {
        CurrentHealth.Value += args.HealthChangeAmount;
        if (CurrentHealth.Value > MaxHealth.Value)
            CurrentHealth.Value = MaxHealth.Value;
        if (CurrentHealth.Value <= 0)
        {
            CurrentHealth.Value = 0;
            ServerEvents.OnEntityDied.Invoke(args.Player);
        }

        HealthUpdatedClientRPC(args);
    }

    [ObserversRpc]
    public void HealthUpdatedClientRPC(HealthChangedEventArgs args)
    {
        ClientEvents.OnEntityHealthChanged.Invoke(args);

        if (CurrentHealth.Value <= 0)
            ClientEvents.OnEntityDied.Invoke(args.Player);
    }

    internal void Initialize()
    {
        Stats = GetComponent<EntityStats>();
        MaxHealth.Value = Stats.MaxHealth;
        CurrentHealth.Value = Stats.MaxHealth;
    }
}
