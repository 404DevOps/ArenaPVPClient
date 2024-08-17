using Assets.ArenaPVP.Scripts.Models.Enums;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using ArenaLogger =Assets.ArenaPVP.Scripts.Helpers.ArenaLogger;

public class ProjectileSpawner : NetworkBehaviour
{
    private static ProjectileSpawner _instance;
    public static ProjectileSpawner Instance => _instance;

    private NetworkObject _nob;

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);

        _nob = GetComponent<NetworkObject>();
    }

    [SerializeField]
    private ProjectileCollection Projectiles;

    private const float MAX_PASSED_TIME = 0.3f;

    public void ClientFireTargeted(FireTargetedProjectileArgs args)
    {
        ArenaLogger.Log("ObserverFireTargeted Client");
        SpawnProjectile(args, 0f);
        ServerFireTargeted(args, base.TimeManager.Tick);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ServerFireTargeted(FireTargetedProjectileArgs args, uint tick, NetworkConnection sender = null)
    {
        ArenaLogger.Log("ServerFireTargeted called");
        float passedTime = (float)base.TimeManager.TimePassed(tick, false);
        passedTime = Mathf.Min(MAX_PASSED_TIME / 2f, passedTime);
        SpawnProjectile(args, passedTime);

        foreach (var conn in _nob.Observers)
        {
            if (conn != sender)
                ObserverFireTargeted(conn, args, tick);
        }
    }

    [TargetRpc]
    private void ObserverFireTargeted(NetworkConnection target, FireTargetedProjectileArgs args, uint tick)
    {
        ArenaLogger.Log("ObserverFireTargeted Client");
        float passedTime = (float)base.TimeManager.TimePassed(tick, false);
        passedTime = Mathf.Min(MAX_PASSED_TIME, passedTime);

        SpawnProjectile(args, passedTime);
    }

    private void SpawnProjectile(FireTargetedProjectileArgs args, float passedTime)
    {
        var prefab = Projectiles.Collection.FirstOrDefault(p => p.Identifier == args.Identifier).ProjectilePrefab;
        if (prefab == null)
        {
            throw new IndexOutOfRangeException("ProjectileCollection does not contain any Projectile with identifier " + args.Identifier);
        }
        var projectile = Instantiate(prefab);
        projectile.transform.position = new Vector3(args.Origin.transform.position.x, args.Origin.transform.position.y, args.Origin.transform.position.z);
        var projectileScript = projectile.GetComponent<ProjectileTargeted>();
        projectileScript.Initialize(args.AbilityId, args.Origin, args.Target, passedTime);
    }
}

[Serializable]
public struct FireTargetedProjectileArgs
{
    public FireTargetedProjectileArgs(int abilityId, Player origin, Player target, ProjectileIdentifier identifier)
    {
        AbilityId = abilityId;
        Origin = origin;
        Target = target;
        Identifier = identifier;
    }

    public int AbilityId;
    public Player Origin;
    public Player Target;
    public ProjectileIdentifier Identifier;
}


