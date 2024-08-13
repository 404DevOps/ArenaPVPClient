using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(menuName = "Abilities/Blademaster/MeleeAbility", fileName = "Melee Ability")]
public class MeleeAbility : AbilityBase
{
    [Range(1,100)]
    public float weaponDamagePercentage;
    public AuraBase[] AurasToApply;

    internal override void UseServer(Player owner, Player target)
    {
        //applyAura(AuraType.Stun, target)
        //get transform, move towards
        throw new System.NotImplementedException();
        ApplyEffectsServer(owner, target);
    }
    internal override void ApplyEffectsServer(Player owner, Player target)
    {
        throw new System.NotImplementedException();
    }
}
