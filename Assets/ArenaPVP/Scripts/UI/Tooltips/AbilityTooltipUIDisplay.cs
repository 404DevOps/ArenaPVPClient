using Assets.ArenaPVP.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AbilityTooltipUIDisplay : TooltipBaseUIDisplay
{
    public AbilityInfo AbilityInfo;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI Cooldown;
    public TextMeshProUGUI CastTime;
    public TextMeshProUGUI Range;
    public TextMeshProUGUI Cost;

    public void OnEnable()
    {
        var player = FindObjectsOfType<Player>().FirstOrDefault(p => p.IsOwnedByMe);
        Name.text = AbilityInfo.Name;
        Description.text = AbilityInfo.Description.Replace("$DAMAGE$", DamageCalculator.GetDamageAfterPowerModifiers(player, AbilityInfo.Damage, AbilityInfo.DamageType).ToString());
        Cooldown.text = AbilityInfo.Cooldown > 0 ? AbilityInfo.Cooldown.ToString() + "sec Cooldown" : "";
        CastTime.text = AbilityInfo.CastTime == 0 ? "Instant" : AbilityInfo.CastTime.ToString() + "sec Cast";
        Range.text = AbilityInfo.Range <= 5 ? "Melee Range" : AbilityInfo.Range.ToString() + "yd Range";
        Cost.text = AbilityInfo.ResourceCost == 0 ? "" : AbilityInfo.ResourceCost.ToString() + " " + GetResourceName();

        StartCoroutine(WaitForFrame());
    }

    private string GetResourceName()
    {
        switch (AbilityInfo.ClassType)
        {
            case CharacterClassType.Trooper:
                return "Rage";
            case CharacterClassType.Medic:
            case CharacterClassType.Cryomancer:
            case CharacterClassType.Voidweaver:
                return "Mana";
            case CharacterClassType.Tracker:
                return "Focus";
            case CharacterClassType.Spectre:
                return "Energy";
            default: return "";
        }
    }
}
