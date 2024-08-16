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

public class PlayerStats : NetworkBehaviour
{
    private StatsMediator _mediator;
    private readonly SyncVar<BaseStats> _baseStats = new SyncVar<BaseStats>();
    public StatsMediator Mediator => _mediator;

    public void Awake()
    {
        _mediator = GetComponent<StatsMediator>();
    }
    public override void OnStartServer()
    {
        var player = GetComponent<Player>();
        _baseStats.Value = ClassStatMapping.Instance().GetBaseStats(player.ClassType);

        GameEvents.OnPlayerStatsInitialized.Invoke(player);
    }

    public float Attackpower
    {
        get {
            var q = new StatQuery(StatType.AttackPower, _baseStats.Value.AttackPower);
            _mediator.PerformQuery(this, q);
            return q.Value;
        }
    }
    public float Spellpower
    {
        get {
            var q = new StatQuery(StatType.Spellpower, _baseStats.Value.Spellpower);
            _mediator.PerformQuery(this, q);
            return q.Value;
        }
    }
    public float MovementSpeed
    {
        get
        {
            var q = new StatQuery(StatType.MovementSpeed, _baseStats.Value.MovementSpeed);
            _mediator.PerformQuery(this, q);
            return q.Value;
        }

    }
    public float ResourceRegenerationRate
    {
        get
        {
            var q = new StatQuery(StatType.RessourceRegenerationRate, _baseStats.Value.RessourceRegenerationRate);
            _mediator.PerformQuery(this, q);
            return q.Value;
        }
    }
    public float SpellResistance
    {
        get
        {
            var q = new StatQuery(StatType.SpellResistance, _baseStats.Value.SpellResistance);
            _mediator.PerformQuery(this, q);
            return q.Value;
        }

        }
    public float Armor
    {
        get
        {
            var q = new StatQuery(StatType.Armor, _baseStats.Value.Armor);
            _mediator.PerformQuery(this, q);
            return q.Value;
        }

    }
    public float MaxHealth
    {
        get
        {
            var q = new StatQuery(StatType.Health, _baseStats.Value.Health);
            _mediator.PerformQuery(this, q);
            return q.Value;
        }

    }
    public float MaxResource
    {
        get
        {
            var q = new StatQuery(StatType.Ressource, _baseStats.Value.Ressource);
            _mediator.PerformQuery(this, q);
            return q.Value;
        }

    }

}
