using Assets.ArenaPVP.Scripts.Helpers;
using Assets.ArenaPVP.Scripts.Models.Enums;
using Assets.Scripts.Enums;
using FishNet;
using GameKit.Dependencies.Utilities;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.Image;
using Logger = Assets.Scripts.Helpers.Logger;

[Serializable]
public abstract class AbilityBase : ScriptableObject
{
    public int Id;
    public AbilityInfo AbilityInfo;
    public AbilityTargetType TargetingType;

    public AuraBase[] ApplyAuras;

    public bool NeedLineOfSight;
    public bool NeedTargetInFront;

    public void TryUseAbility(Player origin, Player target)
    {
        AbilityExecutor.Instance.TryUseAbilityClient(new UseAbilityArgs(Id, origin, target));
    }

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
        if (CastManager.Instance.Contains(ownerId, Id) && CastManager.Instance.GetRemainingCastTime(ownerId) > 0)
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

    /// <summary>
    /// Checks if Instance is Server, then Applies "OnCastFinished" Auras (anything additional should be in override)
    /// </summary>
    internal virtual void UseServer(Player origin, Player target) 
    {
        if (!InstanceFinder.IsServerStarted)
            throw new Exception("Tried execute Server function from Client");

        if (ApplyAuras != null)
        {
            foreach (var aura in ApplyAuras.Where(a => a.AuraApplyTiming == AuraApplyTiming.OnCastFinished))
            {
                if (aura.AuraTarget == AuraTargetType.Player)
                    aura.Apply(origin, origin);
                else if (aura.AuraTarget == AuraTargetType.Target)
                    aura.Apply(origin, target);
            }
        }
    }

    /// <summary>
    /// All Animations, Sounds and Particles should be instantiated here.
    /// </summary>
    internal virtual void UseClient(Player origin, Player target)
    {
        if (!InstanceFinder.IsClientStarted)
            throw new Exception("Tried execute Client function from Server");
    }

    /// <summary>
    /// Checks if Instance is Server, then Applies "OnHit" Auras (anything additional such as Heal/Damage application should be in override)
    /// </summary>
    internal virtual void ApplyEffectsServer(Player origin, Player target)
    {
        if (!InstanceFinder.IsServerStarted)
            throw new Exception("Tried execute Server function from Client");

        foreach (var aura in ApplyAuras.Where(a => a.AuraApplyTiming == AuraApplyTiming.OnHit))
        {
            if (aura.AuraTarget == AuraTargetType.Player)
                aura.Apply(origin, origin);
            else if (aura.AuraTarget == AuraTargetType.Target)
                aura.Apply(origin, target);
        }
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