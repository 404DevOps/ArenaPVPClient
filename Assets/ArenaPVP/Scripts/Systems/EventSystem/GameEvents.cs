using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{
    public static readonly Event<int, AbilityBase> OnCastStarted = new Event<int, AbilityBase>();
    public static readonly Event<int> OnCastInterrupted = new Event<int>();
    public static readonly Event<int> OnCastCompleted = new Event<int>(); //ended regularly

    public static readonly Event<int, string> OnCooldownStarted = new Event<int, string>(); //playerId, Ability
    public static readonly Event<HealthChangedEventArgs> OnPlayerHealthChanged = new Event<HealthChangedEventArgs>(); //playerId, healthChanged

    public static readonly Event<int, AuraInfo> OnAuraExpired = new Event<int, AuraInfo>(); //ownerId, auraId
    public static readonly Event<int, AuraInfo> OnAuraApplied = new Event<int, AuraInfo>(); //ownerId, auraId

    public static readonly Event<Player> OnPlayerInitialized = new Event<Player>();
}
