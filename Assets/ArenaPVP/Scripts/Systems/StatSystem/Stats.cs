using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.ArenaPVP.Scripts.Models.Enums;

public class Stats 
{
    readonly StatsMediator _mediator;
    readonly BaseStats _baseStats;
    public StatsMediator Mediator => _mediator;
    public Stats(StatsMediator mediator, BaseStats baseStats)
    { 
        _mediator = mediator;
        _baseStats = baseStats;
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
        get
        {
            var q = new StatQuery(StatType.Spellpower, _baseStats.Spellpower);
            _mediator.PerformQuery(this, q);
            return q.Value;
        }

    }
    public float MovementSpeed
    {
        get
        {
            var q = new StatQuery(StatType.MovementSpeed, _baseStats.MovementSpeed);
            _mediator.PerformQuery(this, q);
            return q.Value;
        }

    }
    public float ResourceRegenerationSpeed
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
    public float Health
    {
        get
        {
            var q = new StatQuery(StatType.Health, _baseStats.Health);
            _mediator.PerformQuery(this, q);
            return q.Value;
        }

    }
    public float Resource
    {
        get
        {
            var q = new StatQuery(StatType.Ressource, _baseStats.Ressource);
            _mediator.PerformQuery(this, q);
            return q.Value;
        }

    }

}
