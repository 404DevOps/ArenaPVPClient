using Assets.Scripts.Enums;
using Logger = Assets.Scripts.Helpers.Logger;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using Assets.ArenaPVP.Scripts.Helpers;
using GameKit.Dependencies.Utilities;
using FishNet.Editing;

[Serializable]
public abstract class AbilityBase : ScriptableObject
{
    public AbilityInfo AbilityInfo;
    public AbilityTargetType TargetingType;

    public bool NeedLineOfSight;
    public bool NeedTargetInFront;


    //TODO: make ServerCharacter owner and target
    public void TryUseAbility(Transform owner, Transform target)
    {
        if (CanBeUsed(owner, target))
        {
            CooldownManager.Instance.AddOrUpdate(new AbilityWithOwner(owner.GetInstanceID(), AbilityInfo.Name));
            Use(owner, target);
        }
    }

    public bool CanBeUsed(Transform owner, Transform target)
    {
        bool canbeUse = true;
        if (!IsInRange(owner, target))
        {
            return false;
        }
        if (NeedLineOfSight && !IsLineOfSight(owner, target))
        {
            Logger.Log("Target not Line of Sight");
            return false;
        }
        if (NeedTargetInFront && !IsInFront(owner, target))
        {
            Logger.Log("Target not in Front");
            return false;
        }
        if (!IsCooldownReady(owner.GetInstanceID())) 
        {
            Logger.Log("Cooldown is not ready.");
            return false;
        }

        return canbeUse;
    }

    private bool IsCooldownReady(int ownerId)
    {
        var abilityWithOwner = new AbilityWithOwner(ownerId, AbilityInfo.Name);
        
        if (!CooldownManager.Instance.Contains(abilityWithOwner.Identifier))
        {
            return true;
        }
        else if (CooldownManager.Instance.TimeSinceLastUse(abilityWithOwner) + AbilityInfo.Cooldown >= 0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsInFront(Transform owner, Transform target)
    {
        return PositionHelper.IsInFront(owner, target);
    }

    private bool IsLineOfSight(Transform owner, Transform target)
    {
        var dir = target.position - owner.position;
        var ray = new Ray(owner.position, dir);
        if (Physics.Raycast(ray, out RaycastHit hit, AbilityInfo.Range))
        {
            Logger.Log("Owner: " + owner);
            Logger.Log("TargetPos: " + target);

            if (hit.collider.CompareTag("Wall"))
            {
                Debug.DrawRay(owner.position, dir, Color.red);
                return false;
            }
        }
        else 
        {
            Logger.Log("Owner: " + owner);
            Logger.Log("TargetPos: " + target);
            
            Debug.DrawRay(owner.position, target.GetPosition(false), Color.green);
        }

        return true;
    }

    private bool IsInRange(Transform self, Transform target)
    {
        switch (TargetingType)
        {
            case AbilityTargetType.Self:
                return true;
            case AbilityTargetType.TargetFriendly:
            case AbilityTargetType.TargetEnemy:
                return Vector3.Distance(self.position, target.position) <= AbilityInfo.Range;
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

    protected abstract void Use(Transform owner, Transform target);
}

[Serializable]
public class AbilityInfo 
{
    public string Id => Guid.NewGuid().ToString();
    public string Name;
    [TextArea]
    public string Description;
    public float CastTime;
    public float Cooldown;
    public float ResourceCost;
    public float Range;
    public Sprite Icon;

    public AbilityClassType ClassType;
}