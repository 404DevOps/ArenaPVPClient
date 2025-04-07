using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServerEvents
{
    public static readonly Event<Entity> OnEntityStatsInitialized = new Event<Entity>();
    public static readonly Event<Entity> OnEntityDied = new Event<Entity>();
    public static readonly Event<Entity> OnEntityInitialized = new Event<Entity>();
}
