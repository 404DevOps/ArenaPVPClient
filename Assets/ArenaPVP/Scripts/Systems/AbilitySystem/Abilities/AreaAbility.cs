using GameKit.Dependencies.Utilities.ObjectPooling.Examples;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Logger = Assets.Scripts.Helpers.Logger;

[CreateAssetMenu(menuName = "Abilities/AreaAbility", fileName = "AreaAbility")]
public class AreaAbility : AbilityBase
{
    public AuraBase[] ApplyAuras;
    private List<Player> _targets;
    private Player _owner;
    [SerializeField] private AreaSelectorBase _areaSelector; 

    protected override void Use(Player origin, Player tar = null)
    {
        _targets = _areaSelector.GetTargetsInArea(origin);
        GameEvents.OnCastCompleted.Invoke(origin.Id);
        CooldownManager.Instance.AddOrUpdate(new AbilityWithOwner(origin.Id, AbilityInfo.Name));
        GameEvents.OnCooldownStarted.Invoke(origin.Id, AbilityInfo.Name);

        _owner = origin;
        GameEvents.OnPlayerResourceChanged.Invoke(new ResourceChangedEventArgs() { Player = origin, ResourceChangeAmount = -AbilityInfo.ResourceCost });
        
        foreach (var target in _targets)
        {
            if (target.IsOwnedByMe)
                continue;
            //TODO: check if target is enemy or friendly and check TargetingType to match that.
            if (IsLineOfSight(origin.transform, target.transform))
            {
                ApplyAbilityEffectAndDamage(origin, target);
            }
        }
    }

    public virtual void ApplyAbilityEffectAndDamage(Player origin, Player target) 
    {
        var args = new HealthChangedEventArgs()
        {
            Player = target.GetComponent<Player>(),
            Source = _owner.GetComponent<Player>(),
            HealthChangeAmount = -DamageCalculator.CalculateDamage(_owner, target, this),
            HealthChangeType = HealthChangeType.Damage,
            DamageType = AbilityInfo.DamageType,
            AbilityId = Id
        };

        foreach (var aura in ApplyAuras)
        {
            aura.Apply(_owner, target);
        }

        GameEvents.OnPlayerHealthChanged.Invoke(args);
    }
}
