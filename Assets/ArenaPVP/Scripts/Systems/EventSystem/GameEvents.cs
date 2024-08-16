using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{
    public static readonly Event<int, int> OnCastStarted = new Event<int, int>();
    public static readonly Event<int> OnCastInterrupted = new Event<int>();
    public static readonly Event<int> OnCastCompleted = new Event<int>(); //ended regularly

    public static readonly Event<int, int> OnCooldownStarted = new Event<int, int>(); //playerId, AbilityId
    public static readonly Event<HealthChangedEventArgs> OnPlayerHealthChanged = new Event<HealthChangedEventArgs>();
    public static readonly Event<ResourceChangedEventArgs> OnPlayerResourceChanged = new Event<ResourceChangedEventArgs>();

    public static readonly Event<int, AuraInfo> OnAuraExpired = new Event<int, AuraInfo>(); //ownerId, auraId
    public static readonly Event<int, AuraInfo> OnAuraApplied = new Event<int, AuraInfo>(); //ownerId, auraId

    public static readonly Event<Player> OnPlayerInitialized = new Event<Player>();

    public static readonly Event<Player> OnPlayerStatsInitialized = new Event<Player>();
    //public static readonly Event<UseAbilityArgs> OnAbilityUsed = new Event<UseAbilityArgs>();
}
