using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{
    public static readonly Event onSettingsLoaded = new Event();

    public static readonly Event<AbilityInfo> OnCastStarted = new Event<AbilityInfo>();
    public static readonly Event OnCastStopped = new Event();
}
