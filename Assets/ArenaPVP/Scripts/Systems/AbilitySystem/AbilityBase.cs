using Assets.Scripts.Enums;
using Logger = Assets.Scripts.Helpers.Logger;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.ArenaPVP.Scripts.Helpers;
using GameKit.Dependencies.Utilities;
using FishNet.Editing;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.GridLayoutGroup;
using System.Threading;

[Serializable]
public abstract class AbilityBase : ScriptableObject
{
    public AbilityInfo AbilityInfo;
    public AbilityTargetType TargetingType;

    public bool NeedLineOfSight;
    public bool NeedTargetInFront;

    private bool _wasInterrupted;
    private int _ownerId;

    //TODO: make ServerCharacter owner and target
    public bool TryUseAbility(Transform owner, Transform target)
    {
        _wasInterrupted = false;
        _ownerId = owner.GetInstanceID();

        GameEvents.OnCastInterrupted.AddListener(WasInterrupted);
        if (CanBeUsed(owner, target))
        {
            if (AbilityInfo.CastTime > 0)
            {
                GameEvents.OnCastStarted.Invoke(_ownerId, this);
                CastManager.Instance.AddOrUpdate(_ownerId, AbilityInfo.Name);
                CastManager.Instance.StartCastCoroutine(CastTimer(owner, target,AbilityInfo.CastTime));
            }
            else{
                Use(owner, target);
            }
            return true;
        }
        return false;
    }

    IEnumerator CastTimer(Transform owner, Transform target, float castTime)
    {
        yield return new WaitForSeconds(castTime);

        CastManager.Instance.Remove(_ownerId);
        //check line of sight again
        if (!IsInFront(owner,target) || !IsLineOfSight(owner,target))
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
        else 
        {
            _wasInterrupted = false;
            GameEvents.OnCastInterrupted.Invoke(_ownerId);
            GameEvents.OnCastInterrupted.RemoveListener(WasInterrupted);
            Logger.Log($"Ability {AbilityInfo.Name} was Interrupted while casting.");
        }
    }

    private void WasInterrupted(int ownerId)
    {
        if (_ownerId != ownerId) return;
        _wasInterrupted = true;
    }

    private bool CanBeUsed(Transform owner, Transform target)
    {
        bool canbeUse = true;
        if (!IsInRange(owner, target))
        {
            return false;
        }
        if (!IsLineOfSight(owner, target))
        {
            Logger.Log("Target not Line of Sight");
            return false;
        }
        if (!IsInFront(owner, target))
        {
            Logger.Log("Target not in Front");
            return false;
        }
        if (!IsCooldownReady(owner.GetInstanceID())) 
        {
            Logger.Log("Cooldown is not ready.");
            return false;
        }
        if (IsAlreadyCasting(owner.GetInstanceID()))
        {
            Logger.Log("Already Casting that Spell.");
            return false;
        }

        return canbeUse;
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

    private bool IsLineOfSight(Transform owner, Transform target)
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

    public CharacterClassType ClassType;
}