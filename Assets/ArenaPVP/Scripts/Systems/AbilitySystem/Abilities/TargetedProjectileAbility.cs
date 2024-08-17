using Assets.ArenaPVP.Scripts.Models.Enums;
using FishNet;
using FishNet.Object;
using GameKit.Dependencies.Utilities.ObjectPooling.Examples;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.Image;
using Logger =Assets.ArenaPVP.Scripts.Helpers.ArenaLogger;

[CreateAssetMenu(menuName = "Abilities/ProjectileAbility", fileName = "ProjectileAbility")]
public class TargetedProjectileAbility : AbilityBase
{
    public ProjectileIdentifier ProjectileIdentifier;

    internal override void UseServer(Player origin, Player target)
    {
        base.UseServer(origin, target); 
    }
    internal override void UseClient(Player origin, Player target)
    {
        base.UseClient(origin, target);
        ProjectileSpawner.Instance.ClientFireTargeted(new FireTargetedProjectileArgs(Id, origin, target, ProjectileIdentifier));
    }

    internal override void ApplyEffectsServer(Player origin,Player target)
    {
        base.ApplyEffectsServer(origin, target);

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
    }
}
