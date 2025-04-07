using System;

public static class DamageCalculator
{
    public static float CalculateDamage(Entity source, Entity target, DamageEntityEffect damageEffect)
    {
        var dmgAfterPowerMod = GetDamageAfterPowerModifiers(source, damageEffect.DamageAmount, damageEffect.DamageType);
        var result = GetDamageAfterDefenseModifiers(target, dmgAfterPowerMod, damageEffect.DamageType);
        return result;
    }
    public static float GetDamageAfterPowerModifiers(Entity source, float abilityBaseDamage, DamageType damageType) 
    {
        float result = 0;
        var playerStats = source.GetComponent<EntityStats>();
        switch (damageType) 
        {
            case DamageType.Magic: result = playerStats.Spellpower + abilityBaseDamage; break;
            case DamageType.Physical: result = playerStats.Attackpower + abilityBaseDamage; break;
            default: throw new ArgumentOutOfRangeException();
        }

        return result;
    }

    public static float GetDamageAfterDefenseModifiers(Entity target, float damageAfterPower, DamageType damageType)
    {
        float result = 0;
        var playerStats = target.GetComponent<EntityStats>();

        switch (damageType)
        {
            case DamageType.Magic: result = damageAfterPower - playerStats.SpellResistance; break;
            case DamageType.Physical: result = damageAfterPower - playerStats.Armor; break;
            default: throw new ArgumentOutOfRangeException();
        }

        return result;
    }

}
