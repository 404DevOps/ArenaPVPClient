using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HealthChangedEventArgs
{
    public Player Player;
    public Player Source;

    public int AbilityId;

    public float HealthChangeAmount;
    public HealthChangeType HealthChangeType;

    public DamageType DamageType;
}

public enum HealthChangeType
{  
    Damage, 
    Heal
}
