using Assets.ArenaPVP.Scripts.Models.Enums;
using FishNet;
using FishNet.Object;
using GameKit.Dependencies.Utilities.ObjectPooling.Examples;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.Image;
using Logger = Assets.Scripts.Helpers.Logger;

[CreateAssetMenu(menuName = "Abilities/ProjectileAbility", fileName = "ProjectileAbility")]
public class TargetedProjectileAbility : AbilityBase
{
    public ProjectileIdentifier ProjectileIdentifier;
    public AuraBase[] ApplyAuras;

    protected override void Use(Player origin, Player target)
    {
        ProjectileSpawner.Instance.ClientFireTargeted(new FireTargetedProjectileArgs(Id, origin, target, ProjectileIdentifier));
        origin.GetComponent<PlayerResource>().UpdateResourceServer(new ResourceChangedEventArgs() { Player = origin, ResourceChangeAmount = -AbilityInfo.ResourceCost });

        if (ApplyAuras != null)
        {
            foreach (var aura in ApplyAuras.Where(a => a.AuraApplyTiming == AuraApplyTiming.OnCastFinished))
            {
                if(aura.AuraTarget == AuraTargetType.Player)
                    aura.Apply(origin, origin);
                else if (aura.AuraTarget == AuraTargetType.Target)
                    aura.Apply(origin, target);
            }
        }
    }

    public override void ApplyEffects(Player origin,Player target)
    {
        if(InstanceFinder.IsServerStarted)
        {
            var args = new HealthChangedEventArgs()
            {
                Player = target.GetComponent<Player>(),
                Source = origin.GetComponent<Player>(),
                HealthChangeAmount = -DamageCalculator.CalculateDamage(origin, target, this),
                HealthChangeType = HealthChangeType.Damage,
                DamageType = AbilityInfo.DamageType,
                AbilityId = Id
            };
            target.GetComponent<PlayerHealth>().UpdateHealthServer(args);

            foreach (var aura in ApplyAuras.Where(a => a.AuraApplyTiming == AuraApplyTiming.OnHit))
            {
                if (aura.AuraTarget == AuraTargetType.Player)
                    aura.Apply(origin, origin);
                else if (aura.AuraTarget == AuraTargetType.Target)
                    aura.Apply(origin, target);
            }
        }
    }
}
