using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(menuName = "Abilities/Blademaster/Charge", fileName = "Charge")]
public class ChargeAbility : AbilityBase
{
    public float chargeSpeed;
    public float stunDuration;

    protected override void Use(Player owner, Player target)
    {
        //applyAura(AuraType.Stun, target)
        //get transform, move towards
        throw new System.NotImplementedException();
    }
}
