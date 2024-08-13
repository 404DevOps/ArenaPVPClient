using FishNet;
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

    internal override void UseServer(Player origin, Player target)
    {
        if (InstanceFinder.IsServerStarted)
        {
            _targets = _areaSelector.GetTargetsInArea(origin);
            _owner = origin;

            foreach (var tar in _targets)
            {
                if (tar.IsOwnedByMe)
                    continue;
                //TODO: check if target is enemy or friendly and check TargetingType to match that.
                if (IsLineOfSight(origin.transform, tar.transform))
                {
                    ApplyEffectsServer(origin, tar);
                }
            }
        }
    }

    internal override void ApplyEffectsServer(Player origin, Player target) 
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
