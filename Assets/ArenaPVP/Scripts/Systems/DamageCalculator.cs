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
        var playerStats = source.GetComponent<PlayerStats>();
        switch (damageType) 
        {
            case DamageType.Magic: result = playerStats.Spellpower + abilityBaseDamage; break;
            case DamageType.Physical: result = playerStats.Attackpower + abilityBaseDamage; break;
            default: throw new ArgumentOutOfRangeException();
        }

        return result;
    }

    public static float GetDamageAfterDefenseModifiers(Player target, float damageAfterPower, DamageType damageType)
    {
        float result = 0;
        var playerStats = target.GetComponent<PlayerStats>();

        switch (damageType)
        {
            case DamageType.Magic: result = damageAfterPower - playerStats.SpellResistance; break;
            case DamageType.Physical: result = damageAfterPower - playerStats.Armor; break;
            default: throw new ArgumentOutOfRangeException();
        }

        return result;
    }

}
