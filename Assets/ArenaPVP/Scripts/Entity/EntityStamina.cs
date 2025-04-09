using FishNet.Object;
using FishNet.Object.Synchronizing;
using System;
using UnityEngine;
using Logger =Assets.ArenaPVP.Scripts.Helpers.ArenaLogger;

public class EntityStamina : NetworkBehaviour
{
    public readonly SyncVar<float> MaxStamina = new SyncVar<float>();
    public readonly SyncVar<float> CurrentStamina = new SyncVar<float>();

    private EntityStats _stats;

    [Server]
    public void UpdateStaminaServer(StaminaChangedEventArgs args)
    {
        CurrentStamina.Value += args.StaminaChangedAmount;
        if (CurrentStamina.Value > MaxStamina.Value)
            CurrentStamina.Value = MaxStamina.Value;

        StaminaUpdatedClient(args);
    }

    [ObserversRpc]
    public void StaminaUpdatedClient(StaminaChangedEventArgs args)
    {
        ClientEvents.OnEntityStaminaChanged?.Invoke(args);
    }

    internal void Initialize()
    {
        _stats = GetComponent<EntityStats>();
        MaxStamina.Value = _stats.MaxStamina;
        CurrentStamina.Value = _stats.MaxStamina;
    }
}
