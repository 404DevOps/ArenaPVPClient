using Assets.ArenaPVP.Scripts.Models.Enums;
using FishNet;
using FishNet.Managing.Timing;
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

    internal override void UseServer(Entity origin, Entity target)
    {
        Debug.Log("UserServer");
        base.UseServer(origin, target); 
        ProjectileSpawner.Instance.ServerFireTargeted(new FireTargetedProjectileArgs(Id, origin, target, ProjectileIdentifier), InstanceFinder.TimeManager.Tick);
    }
    internal override void UseClient(Entity origin, Entity target)
    {
        Debug.Log("UserClient");
        base.UseClient(origin, target);
        if(!origin.IsHostStarted)
            ProjectileSpawner.Instance.ClientFireTargeted(new FireTargetedProjectileArgs(Id, origin, target, ProjectileIdentifier));
    }
}
