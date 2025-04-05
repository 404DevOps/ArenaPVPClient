using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServerEvents
{
    public static readonly Event<Player> OnPlayerStatsInitialized = new Event<Player>();
    public static readonly Event<Player> OnPlayerDied = new Event<Player>();
}
