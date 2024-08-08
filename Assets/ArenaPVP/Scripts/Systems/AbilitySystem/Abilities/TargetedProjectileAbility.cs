using FishNet;
using FishNet.Object;
using GameKit.Dependencies.Utilities.ObjectPooling.Examples;
using UnityEngine;
using Logger = Assets.Scripts.Helpers.Logger;

[CreateAssetMenu(menuName = "Abilities/ProjectileAbility", fileName = "ProjectileAbility")]
public class TargetedProjectileAbility : AbilityBase
{
    public GameObject ProjectilePrefab;
    public AuraBase[] ApplyAuras;

    protected override void Use(Player owner, Player target)
    {
        ProjectileSpawner.Instance.ClientFireTargeted(new FireTargetedProjectileArgs(Id, owner, target, ProjectilePrefab));
        owner.GetComponent<PlayerResource>().UpdateResourceServer(new ResourceChangedEventArgs() { Player = owner, ResourceChangeAmount = -AbilityInfo.ResourceCost });
    }

    public override void ApplyEffects(Player origin,Player target)
    {
        if(InstanceFinder.IsServerStarted)
        {
            var args = new HealthChangedEventArgs()
            {
                Player = origin.GetComponent<Player>(),
                Source = target.GetComponent<Player>(),
                HealthChangeAmount = -DamageCalculator.CalculateDamage(origin, target, this),
                HealthChangeType = HealthChangeType.Damage,
                DamageType = AbilityInfo.DamageType,
                AbilityId = Id
            };

            foreach (var aura in ApplyAuras)
            {
                aura.Apply(origin, target);
            }

            target.GetComponent<PlayerHealth>().UpdateHealthServer(args); 
        }
    }
}
