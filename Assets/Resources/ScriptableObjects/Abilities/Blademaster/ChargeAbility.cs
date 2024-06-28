using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(menuName = "Abilities/Blademaster/ChargeAbility", fileName = "ChargeAbility")]
public class ChargeAbility : CharacterAbility
{
    public float chargeSpeed;
    public float stunDuration;
    protected override void Use(object owner, object target)
    {
        //applyAura(AuraType.Stun, target)
        //get transform, move towards
        throw new System.NotImplementedException();
    }
}
