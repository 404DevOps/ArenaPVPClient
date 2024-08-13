using Assets.ArenaPVP.Scripts.Helpers;
using Assets.Scripts.Enums;
using FishNet;
using GameKit.Dependencies.Utilities;
using System;
using System.Collections;
using UnityEngine;
using Logger = Assets.Scripts.Helpers.Logger;

[Serializable]
public abstract class AbilityBase : ScriptableObject
{
    public int Id;
    public AbilityInfo AbilityInfo;
    public AbilityTargetType TargetingType;

    public bool NeedLineOfSight;
    public bool NeedTargetInFront;

    private bool _wasInterrupted;
    private int _ownerId;

    //TODO: make ServerCharacter owner and target
    public void TryUseAbility(Player origin, Player target)
    {
        _wasInterrupted = false;
        _ownerId = origin.Id;

        AbilityExecutor.Instance.TryUseAbilityClient(new UseAbilityArgs(Id, origin, target));
    }

    //private void WasInterrupted(int ownerId)
    //{
    //    if (_ownerId != ownerId) return;
    //    _wasInterrupted = true;
    //}
    public bool CanBeUsed(Player owner, Player target)
    {
        bool canbeUse = true;

        if (!IsCooldownReady(owner.Id)) 
        {
            if(InstanceFinder.IsClientStarted)
                UIEvents.OnShowInformationPopup.Invoke("This ability is not ready yet");

            return false;
        }
        if (IsAlreadyCasting(owner.Id))
        {
            Logger.Log("Already casting.");
            return false;
        }
        if (!HasEnoughResource(owner))
        {
            if (InstanceFinder.IsClientStarted)
            {
                var ressourceName = AppearanceData.Instance().GetRessourceDescriptor(owner.ClassType);
                UIEvents.OnShowInformationPopup.Invoke($"Not enough {ressourceName}");
            }

            return false;
        }
        //all checks for AoE abilities should be done at this point, so we can already return and Use() the ability which will select its own targets
        //or none if none meets criteria, but it will go off anyways, without hitting anything.
        if (AbilityInfo.AbilityType == AbilityType.AreaOfEffect) 
        {
            return true;
        }

        if (target == null)
        {
            if (InstanceFinder.IsClientStarted)
                UIEvents.OnShowInformationPopup.Invoke("No valid Target selected");
            return false;
        }
        if (!IsInRange(owner.transform, target.transform))
        {
            if (InstanceFinder.IsClientStarted)
                UIEvents.OnShowInformationPopup.Invoke("Out of range");
            return false;
        }
        if (!IsLineOfSight(owner.transform, target.transform))
        {
            if (InstanceFinder.IsClientStarted)
                UIEvents.OnShowInformationPopup.Invoke("Target not line of sight");
            return false;
        }
        if (!IsInFront(owner.transform, target.transform))
        {
            if (InstanceFinder.IsClientStarted)
                UIEvents.OnShowInformationPopup.Invoke("Target is not in front of you");
            return false;
        }

        return canbeUse;
    }
    private bool HasEnoughResource(Player owner)
    {
        var resComp = owner.GetComponent<PlayerResource>();
        if (resComp.CurrentResource.Value >= this.AbilityInfo.ResourceCost)
            return true;

        return false;
    }
    private bool IsAlreadyCasting(int ownerId)
    {
        if (CastManager.Instance.Contains(ownerId, Id) && CastManager.Instance.GetRemainingCastTime(ownerId, Id) > 0)
        {
            return true;
        }
        return false;
    }
    private bool IsCooldownReady(int ownerId)
    {
        var abilityWithOwner = new AbilityCooldownInfo(ownerId, Id);
        
        if (!CooldownManager.Instance.Contains(abilityWithOwner.Identifier))
        {
            return true;
        }
        else if (CooldownManager.Instance.GetRemainingCooldown(abilityWithOwner) <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    internal bool IsInFront(Transform owner, Transform target)
    {
        if (!NeedTargetInFront)
            return true;
        return PositionHelper.IsInFront(owner, target);
    }
    internal bool IsLineOfSight(Transform owner, Transform target)
    {
        if (!NeedLineOfSight)
            return true;

        var dir = target.position - owner.position;
        var ray = new Ray(owner.position, dir);
        if (Physics.Raycast(ray, out RaycastHit hit, AbilityInfo.Range))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                Debug.DrawRay(owner.position, dir, Color.red);
                return false;
            }
        }
        else 
        {
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
    internal virtual void UseServer(Player owner, Player target) 
    {
        if (!InstanceFinder.IsServerStarted)
            throw new Exception("Tried execute Server function from Client");
    }
    internal virtual void UseClient(Player owner, Player target)
    {
        if (!InstanceFinder.IsClientStarted)
            throw new Exception("Tried execute Client function from Server");
    }
    internal virtual void ApplyEffectsServer(Player owner, Player target)
    {
        if (!InstanceFinder.IsServerStarted)
            throw new Exception("Tried execute Server function from Client");
    }
}

[Serializable]
public class AbilityInfo 
{
    public string Name;
    [TextArea]
    public string Description;
    public float CastTime;
    public float Cooldown;
    public float ResourceCost;
    public float Range;

    public Sprite Icon;

    public DamageType DamageType;
    public float Damage;

    public CharacterClassType ClassType;
    public AbilityType AbilityType;
}

public enum AbilityType
{ 
    Targeted,
    AreaOfEffect
}