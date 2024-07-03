using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(menuName = "Abilities/Blademaster/MortalStrike", fileName = "Mortal Strike")]
public class MortalStrikeAbility : AbilityBase
{
    [Range(1,100)]
    public float weaponDamagePercentage;
    public AuraBase[] AurasToApply;

    protected override void Use(Transform owner, Transform target)
    {
        //applyAura(AuraType.Stun, target)
        //get transform, move towards
        throw new System.NotImplementedException();
    }
}
