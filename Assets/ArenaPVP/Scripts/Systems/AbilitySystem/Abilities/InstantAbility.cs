using Assets.ArenaPVP.Scripts.Models.Enums;
using Assets.ArenaPVP.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Assets.ArenaPVP.Scripts.Helpers;

[CreateAssetMenu(menuName = "Abilities/InstantAbility", fileName = "InstantAbility")]
public class InstantAbility : AbilityBase
{
    internal override void UseServer(Entity owner, Entity target)
    {
        base.UseServer(owner, target);
        ApplyEffectsServer(owner, target);
    }
    internal override void UseClient(Entity owner, Entity target)
    {
        base.UseClient(owner, target);
        return;
    }
    internal override void ApplyEffectsServer(Entity owner, Entity target)
    {
        base.ApplyEffectsServer(owner, target);
        return;
    }
}
