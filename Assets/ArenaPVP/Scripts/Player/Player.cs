using Assets.Scripts.Enums;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;

public class Player : NetworkBehaviour
{
    public string Name;
    public bool IsOwnedByMe;
    public CharacterClassType ClassType;

    public int Id = 0;

    BaseStats baseStats;
    public Stats Stats { get; private set; }

    void Awake()
    {
        baseStats = ClassStatMapping.Instance().GetBaseStats(ClassType);
        Stats = new Stats(new StatsMediator(), baseStats);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        Id = IdentifierService.GetPlayerId();
        IsOwnedByMe = this.IsOwner;
        GameEvents.OnPlayerInitialized.Invoke(this);
        GetComponent<Targetable>().IsSelf =  this.IsOwner;
    }
}
