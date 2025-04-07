using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ClientEvents
{
    public static readonly Event<CastEventArgs> OnCastStarted = new Event<CastEventArgs>();
    public static readonly Event<AbilityCastInfo> OnCastInterrupted = new Event<AbilityCastInfo>();
    public static readonly Event<CastEventArgs> OnCastCompleted = new Event<CastEventArgs>(); //ended regularly

    public static readonly Event<int, float> OnGCDStarted = new Event<int, float>();

    public static readonly Event<int, int> OnCooldownStarted = new Event<int, int>(); //playerId, AbilityId
    public static readonly Event<HealthChangedEventArgs> OnEntityHealthChanged = new Event<HealthChangedEventArgs>();
    public static readonly Event<ResourceChangedEventArgs> OnEntityResourceChanged = new Event<ResourceChangedEventArgs>();

    public static readonly Event<int, AuraInfo> OnAuraExpired = new Event<int, AuraInfo>(); //ownerId, auraId
    public static readonly Event<int, AuraInfo> OnAuraApplied = new Event<int, AuraInfo>(); //ownerId, auraId

    public static readonly Event<Entity> OnEntityInitialized = new Event<Entity>();
    public static readonly Event<Entity> OnEntityDied = new Event<Entity>();

    public static readonly Event<StatSnapshot> OnClientStatsUpdated = new();
    //public static readonly Event<UseAbilityArgs> OnAbilityUsed = new Event<UseAbilityArgs>();
}
