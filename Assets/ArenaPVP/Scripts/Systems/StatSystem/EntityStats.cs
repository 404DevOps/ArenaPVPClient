using Assets.ArenaPVP.Scripts.Models.Enums;
using FishNet.CodeGenerating;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System;

public class EntityStats : NetworkBehaviour
{
    private StatsMediator _mediator;
    private BaseStats _baseStats;
    public StatsMediator Mediator => _mediator;
    [AllowMutableSyncType]
    public SyncVar<StatSnapshot> Snapshot;

    public void Awake()
    {
        _mediator = GetComponent<StatsMediator>();
    }
    private void OnEnable()
    {
        if (IsServerStarted)
            _mediator.OnModifiersChanged += OnModifiersChanged;
    }
    private void OnDisable()
    {
        if (IsServerStarted)
            _mediator.OnModifiersChanged += OnModifiersChanged;
    }

    [Server]
    private void OnModifiersChanged(StatType type)
    {
        BuildSnapshot();
    }

    public void Initialize(Entity player)
    {
        _baseStats = ClassStatMapping.Instance().GetBaseStats(player.ClassType);
        if (IsServerStarted)
        {
            ServerEvents.OnEntityStatsInitialized.Invoke(player);
            BuildSnapshot();
        }
    }

    [Server]
    public void BuildSnapshot()
    {
        Snapshot.Value = new StatSnapshot()
        {
            AttackPower = Attackpower,
            Spellpower = Spellpower,
            MovementSpeed = MovementSpeed,
            ResourceRegenRate = ResourceRegenerationRate,
            SpellResistance = SpellResistance,
            Armor = Armor,
            MaxHealth = MaxHealth,
            MaxResource = MaxResource,
            MaxStamina = MaxStamina,
            MaxShield = MaxShield,
            StaminaRegenerationRate = StaminaRegenerationRate
        };
        //    Snapshot.Value.Attackpower = Attackpower;
        //    Snapshot.Value.Spellpower = Spellpower;
        //    Snapshot.Value.MovementSpeed = MovementSpeed;
        //    Snapshot.Value.ResourceRegenRate = ResourceRegenerationRate;
        //    Snapshot.Value.SpellResistance = SpellResistance;
        //    Snapshot.Value.Armor = Armor;
        //    Snapshot.Value.MaxHealth = MaxHealth;
        //    Snapshot.Value.MaxResource = MaxResource;
        //    Snapshot.Value.MaxStamina = MaxStamina;
        //    Snapshot.Value.MaxShield = MaxShield; 
    }




    //[TargetRpc]
    //public void Target_UpdateClientSnapshot(NetworkConnection conn, StatSnapshot snapshot)
    //{
    //    Snapshot = snapshot;
    //    ClientEvents.OnClientStatsUpdated?.Invoke(snapshot);
    //}

    //[Server]
    //public IEnumerator PushSnapshotToClient()
    //{
    //    var snapshot = BuildSnapshot();

    //    foreach (var conn in Observers)
    //    {
    //        yield return new WaitUntil(() => conn.IsValid);
    //        Target_UpdateClientSnapshot(conn, snapshot);
    //    }
    //    ClientEvents.OnClientStatsUpdated?.Invoke(snapshot);
    //}

    public float Attackpower
    {
        get
        {
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
    public float StaminaRegenerationRate
    {
        get
        {
            var q = new StatQuery(StatType.Stamina, _baseStats.StaminaRegenerationRate);
            _mediator.PerformQuery(this, q);
            return q.Value;
        }

    }
    

}
