using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{
    public static readonly Event<CastEventArgs> OnCastStarted = new Event<CastEventArgs>();
    public static readonly Event<AbilityCastInfo> OnCastInterrupted = new Event<AbilityCastInfo>();
    public static readonly Event<CastEventArgs> OnCastCompleted = new Event<CastEventArgs>(); //ended regularly

    public static readonly Event<int, float> OnGCDStarted = new Event<int, float>();

    public static readonly Event<int, int> OnCooldownStarted = new Event<int, int>(); //playerId, AbilityId
    public static readonly Event<HealthChangedEventArgs> OnPlayerHealthChanged = new Event<HealthChangedEventArgs>();
    public static readonly Event<ResourceChangedEventArgs> OnPlayerResourceChanged = new Event<ResourceChangedEventArgs>();

    public static readonly Event<int, AuraInfo> OnAuraExpired = new Event<int, AuraInfo>(); //ownerId, auraId
    public static readonly Event<int, AuraInfo> OnAuraApplied = new Event<int, AuraInfo>(); //ownerId, auraId

    public static readonly Event<Player> OnPlayerInitialized = new Event<Player>();

    public static readonly Event<Player> OnPlayerStatsInitialized = new Event<Player>();
    //public static readonly Event<UseAbilityArgs> OnAbilityUsed = new Event<UseAbilityArgs>();
}
