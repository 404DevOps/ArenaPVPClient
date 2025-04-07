using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.ArenaPVP.Scripts.Models.Enums;
using System;
using FishNet.Object;
using UnityEditor;
using UnityEngine.Rendering;
using FishNet.Object.Synchronizing;
using FishNet.Demo.AdditiveScenes;
using FishNet.CodeGenerating;
using UnityEditorInternal.VersionControl;
using FishNet.Connection;

public class EntityStats : NetworkBehaviour
{
    private StatsMediator _mediator;
    private BaseStats _baseStats;
    public StatsMediator Mediator => _mediator;
    public StatSnapshot Snapshot;

    public void Awake()
    {
        _mediator = GetComponent<StatsMediator>();
    }
    public void Initialize(Entity player)
    {
        _baseStats = ClassStatMapping.Instance().GetBaseStats(player.ClassType);
        ServerEvents.OnEntityStatsInitialized.Invoke(player);
        if(IsServerStarted)
            PushSnapshotToClient();
    }

    [Server]
    public StatSnapshot BuildSnapshot()
    {
        return new StatSnapshot
        {
            AttackPower = Attackpower,
            Spellpower = Spellpower,
            MovementSpeed = MovementSpeed,
            ResourceRegenRate = ResourceRegenerationRate,
            SpellResistance = SpellResistance,
            Armor = Armor,
            MaxHealth = MaxHealth,
            MaxResource = MaxResource
        };
    }

    [TargetRpc]
    public void Target_UpdateClientSnapshot(NetworkConnection conn, StatSnapshot snapshot)
    {
        Snapshot = snapshot;
        ClientEvents.OnClientStatsUpdated?.Invoke(snapshot);
    }

    [Server]
    public void PushSnapshotToClient()
    {
        var snapshot = BuildSnapshot();

        foreach(var conn in Observers)
            Target_UpdateClientSnapshot(conn, snapshot);
    }

    public float Attackpower
    {
        get {
            var q = new StatQuery(StatType.AttackPower, _baseStats.AttackPower);
            _mediator.PerformQuery(this, q);
            return q.Value;
        }
    }
    public float Spellpower
    {
        get {
            var q = new StatQuery(StatType.Spellpower, _baseStats.Spellpower);
            _mediator.PerformQuery(this, q);
            return q.Value;
        }
    }
    public float MovementSpeed
    {
        get {
            var q = new StatQuery(StatType.MovementSpeed, _baseStats.MovementSpeed);
            _mediator.PerformQuery(this, q);
            return q.Value;
        }

    }
    public float ResourceRegenerationRate
    {
        get
        {
            var q = new StatQuery(StatType.RessourceRegenerationRate, _baseStats.RessourceRegenerationRate);
            _mediator.PerformQuery(this, q);
            return q.Value;
        }
    }
    public float SpellResistance
    {
        get
        {
            var q = new StatQuery(StatType.SpellResistance, _baseStats.SpellResistance);
            _mediator.PerformQuery(this, q);
            return q.Value;
        }

        }
    public float Armor
    {
        get
        {
            var q = new StatQuery(StatType.Armor, _baseStats.Armor);
            _mediator.PerformQuery(this, q);
            return q.Value;
        }

    }
    public float MaxHealth
    {
        get
        {
            var q = new StatQuery(StatType.Health, _baseStats.Health);
            _mediator.PerformQuery(this, q);
            return q.Value;
        }

    }
    public float MaxResource
    {
        get
        {
            var q = new StatQuery(StatType.Ressource, _baseStats.Ressource);
            _mediator.PerformQuery(this, q);
            return q.Value;
        }

    }
    public float MaxShield
    {
        get
        {
            var q = new StatQuery(StatType.Shield, _baseStats.MaxShield);
            _mediator.PerformQuery(this, q);
            return q.Value;
        }

    }
    public float MaxStamina
    {
        get
        {
            var q = new StatQuery(StatType.Stamina, _baseStats.MaxStamina);
            _mediator.PerformQuery(this, q);
            return q.Value;
        }

    }

}
