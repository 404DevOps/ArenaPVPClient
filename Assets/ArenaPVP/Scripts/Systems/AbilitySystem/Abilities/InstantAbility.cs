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
    internal override void UseServer(Player owner, Player target)
    {
        base.UseServer(owner, target);
        ApplyEffectsServer(owner, target);
    }
    internal override void UseClient(Player owner, Player target)
    {
        base.UseClient(owner, target);
        return;
    }
    internal override void ApplyEffectsServer(Player owner, Player target)
    {
        base.ApplyEffectsServer(owner, target);
        return;
    }
}
