using Assets.ArenaPVP.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AbilityTooltipUIDisplay : TooltipBaseUIDisplay
{
    public AbilityBase Ability;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI Cooldown;
    public TextMeshProUGUI CastTime;
    public TextMeshProUGUI Range;
    public TextMeshProUGUI Cost;

    public void OnEnable()
    {
        var player = Helpers.LocalPlayer;
        Name.text = Ability.AbilityInfo.Name;
        Description.text = GetDescriptionText(player);
        Cooldown.text = Ability.AbilityInfo.Cooldown > 0 ? Ability.AbilityInfo.Cooldown.ToString() + "sec Cooldown" : "";
        CastTime.text = Ability.AbilityInfo.CastTime == 0 ? "Instant" : Ability.AbilityInfo.CastTime.ToString() + "sec Cast";
        Range.text = Ability.AbilityInfo.Range <= 5 ? "Melee Range" : Ability.AbilityInfo.Range.ToString() + "yd Range";
        Cost.text = Ability.AbilityInfo.ResourceCost == 0 ? "" : Ability.AbilityInfo.ResourceCost.ToString() + " " + GetResourceName();

        StartCoroutine(WaitForFrame());
    }

    /// <summary>
    /// Replace all pssible Variables in Description Text.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    private string GetDescriptionText(Entity player)
    {   
        string desc = string.Empty;
        if (Ability.Effects != null)
        {
            var dmgEffect = Ability.Effects.FirstOrDefault(fx => fx is DamageEntityEffect) as DamageEntityEffect;
            if(dmgEffect != null)
                desc = Ability.AbilityInfo.Description.Replace("$DAMAGE$", DamageCalculator.GetDamageAfterPowerModifiers(player, dmgEffect.DamageAmount, dmgEffect.DamageType).ToString());
        }
        if (!string.IsNullOrEmpty(desc))
            return desc;
        else
            return Ability.AbilityInfo.Description;
    }

    private string GetResourceName()
    {
        switch (Ability.AbilityInfo.ClassType)
        {
            case CharacterClassType.Trooper:
                return "Rage";
            case CharacterClassType.Medic:
            case CharacterClassType.Cryomancer:
            case CharacterClassType.Pyrohacker:
                return "Mana";
            case CharacterClassType.Tracker:
                return "Focus";
            case CharacterClassType.Spectre:
                return "Energy";
            default: return "";
        }
    }
}
