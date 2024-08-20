using Assets.ArenaPVP.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(menuName = "Abilities/Blademaster/Charge", fileName = "Charge")]
public class ChargeAbility : AbilityBase
{
    public float chargeSpeed;
    public float stunDuration;
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
