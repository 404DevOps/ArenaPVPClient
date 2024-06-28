using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterAbility : ScriptableObject
{
    public AbilityInfo AbilityInfo;
    public AbilityClassType ClassType;
    public AbilityTargetType TargetingType;

    public float CastTime;
    public float Cooldown;
    public float ResourceCost;
    public float Range;

    //TODO: make ServerCharacter owner and target
    public void TryUseAbility(Transform owner, Transform[] target)
    {
        if (CanBeUsed(owner, target))
        {
            Use(owner, target);
        }
    }

    public bool CanBeUsed(Transform owner, Transform[] target)
    {
        bool canbeUse = true;
        if (!IsInRange(owner, target))
        {
            return false;
        }

        return canbeUse;
    }

    private bool IsInRange(Transform self, Transform[] Target)
    {
        switch (TargetingType)
        {
            case AbilityTargetType.Self:
                return true;
            case AbilityTargetType.TargetFriendly:
            case AbilityTargetType.TargetEnemy:
                //Vector3.Distance(self, target) <= Range
                break;
            case AbilityTargetType.AllEnemy:
            case AbilityTargetType.AllFriendly:
                //if(transform.Any(t => Vector3.Distance(self, t) <= Range))
                //return true
                break;
            default:
                break;
        }

        return false;
    }

    protected abstract void Use(object owner, object target);
}

public class AbilityInfo 
{
    public string Name;
    public string TooltipText;
    public Sprite Icon;
}