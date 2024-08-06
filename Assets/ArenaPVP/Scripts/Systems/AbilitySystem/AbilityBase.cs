using Assets.ArenaPVP.Scripts.Helpers;
using Assets.Scripts.Enums;
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
    public bool TryUseAbility(Player origin, Player target)
    {
        _wasInterrupted = false;
        _ownerId = origin.Id;

        GameEvents.OnCastInterrupted.AddListener(WasInterrupted);
        if (CanBeUsed(origin, target))
        {
            if (AbilityInfo.CastTime > 0)
            {
                GameEvents.OnCastStarted.Invoke(_ownerId, this);
                CastManager.Instance.AddOrUpdate(_ownerId, AbilityInfo.Name);
                CastManager.Instance.StartCastCoroutine(CastTimer(origin, target,AbilityInfo.CastTime));
            }
            else{
                Use(origin, target);
            }
            return true;
        }
        return false;
    }

    IEnumerator CastTimer(Player owner, Player target, float castTime)
    {
        if (_wasInterrupted)
        {
            _wasInterrupted = false;
            GameEvents.OnCastInterrupted.Invoke(_ownerId);
            GameEvents.OnCastInterrupted.RemoveListener(WasInterrupted);
            Logger.Log($"Ability {AbilityInfo.Name} was Interrupted while casting.");
        }

        yield return new WaitForSeconds(castTime);

        CastManager.Instance.Remove(_ownerId);
        //check line of sight again
        if (AbilityInfo.AbilityType != AbilityType.AreaOfEffect  && (!IsInFront(owner.transform,target.transform) || !IsLineOfSight(owner.transform,target.transform)))
        {
            _wasInterrupted = true;
        }
        if (!_wasInterrupted)
        {
            GameEvents.OnCastCompleted.Invoke(_ownerId);
            CooldownManager.Instance.AddOrUpdate(new AbilityWithOwner(_ownerId, AbilityInfo.Name));
            GameEvents.OnCooldownStarted.Invoke(_ownerId, AbilityInfo.Name);

            Use(owner, target);
        }
    }

    private void WasInterrupted(int ownerId)
    {
        if (_ownerId != ownerId) return;
        _wasInterrupted = true;
    }
    private bool CanBeUsed(Player owner, Player target)
    {
        bool canbeUse = true;

        if (!IsCooldownReady(owner.Id)) 
        {
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
            var ressourceName = AppearanceData.Instance().GetRessourceDescriptor(owner.ClassType);
            UIEvents.OnShowInformationPopup.Invoke($"Not enough {ressourceName}");
            return false;
        }
        //all checks for AoE abilities should be done at this point, so we can already return and use the ability which will select its own targets
        //or none if none meets criteria, but it will go off anyways, similar to a frost nova in wow.
        if (AbilityInfo.AbilityType == AbilityType.AreaOfEffect) 
        {
            return true;
        }

        if (target == null)
        {
            UIEvents.OnShowInformationPopup.Invoke("No valid Target selected");
            return false;
        }
        if (!IsInRange(owner.transform, target.transform))
        {
            UIEvents.OnShowInformationPopup.Invoke("Out of range");
            return false;
        }
        if (!IsLineOfSight(owner.transform, target.transform))
        {
            UIEvents.OnShowInformationPopup.Invoke("Target not line of sight");
            return false;
        }
        if (!IsInFront(owner.transform, target.transform))
        {
            UIEvents.OnShowInformationPopup.Invoke("Target is not in front of you");
            return false;
        }

        return canbeUse;
    }
    private bool HasEnoughResource(Player owner)
    {
        var resComp = owner.GetComponent<PlayerResource>();
        if (resComp.CurrentResource >= this.AbilityInfo.ResourceCost)
            return true;

        return false;
    }
    private bool IsAlreadyCasting(int ownerId)
    {
        if (CastManager.Instance.Contains(ownerId, AbilityInfo.Name))
        {
            return true;
        }
        return false;
    }
    private bool IsCooldownReady(int ownerId)
    {
        var abilityWithOwner = new AbilityWithOwner(ownerId, AbilityInfo.Name);
        
        if (!CooldownManager.Instance.Contains(abilityWithOwner.Identifier))
        {
            return true;
        }
        else if (CooldownManager.Instance.TimeSinceLastUse(abilityWithOwner) >= AbilityInfo.Cooldown)
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
    protected abstract void Use(Player owner, Player target);
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