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
    public static readonly Event<int, float> OnPlayerHealthChanged = new Event<int, float>(); //playerId, healthChanged

    public static readonly Event<int, int> OnAuraExpired = new Event<int, int>(); //ownerId, auraId
    public static readonly Event<int, int> OnAuraApplied = new Event<int, int>(); //ownerId, auraId
}
