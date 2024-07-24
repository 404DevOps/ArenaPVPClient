using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public static class DamageCalculator
{
    public static float CalculateDamage(Player source, Player target, AbilityBase ability)
    {
        var dmgAfterPowerMod = GetDamageAfterPowerModifiers(source, ability.AbilityInfo.Damage, ability.AbilityInfo.DamageType);
        var result = GetDamageAfterDefenseModifiers(target, dmgAfterPowerMod, ability.AbilityInfo.DamageType);
        return result;
    }
    public static float GetDamageAfterPowerModifiers(Player source, float abilityBaseDamage, DamageType damageType) 
    {
        float result = 0;

        switch (damageType) 
        {
            case DamageType.Magic: result = source.Stats.Spellpower + abilityBaseDamage; break;
            case DamageType.Physical: result = source.Stats.Attackpower + abilityBaseDamage; break;
            default: throw new ArgumentOutOfRangeException();
        }

        return result;
    }

    public static float GetDamageAfterDefenseModifiers(Player target, float damageAfterPower, DamageType damageType)
    {
        float result = 0;

        switch (damageType)
        {
            case DamageType.Magic: result = damageAfterPower - target.Stats.SpellResistance; break;
            case DamageType.Physical: result = damageAfterPower - target.Stats.Armor; break;
            default: throw new ArgumentOutOfRangeException();
        }

        return result;
    }

}
