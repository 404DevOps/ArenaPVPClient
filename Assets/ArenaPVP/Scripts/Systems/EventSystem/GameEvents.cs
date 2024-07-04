using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{
    public static readonly Event onSettingsLoaded = new Event();

    public static readonly Event<AbilityBase> OnCastStarted = new Event<AbilityBase>();
    public static readonly Event OnCastInterrupted = new Event();
    public static readonly Event OnCastCompleted = new Event(); //ended regularly


    public static readonly Event<int, string> OnCooldownStarted = new Event<int, string>();
    
}
