using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
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
    

    private void OnEnable()
    {
        Id = IdentifierService.GetPlayerId();
        GameEvents.OnPlayerInitialized.Invoke(this);
    }
}
