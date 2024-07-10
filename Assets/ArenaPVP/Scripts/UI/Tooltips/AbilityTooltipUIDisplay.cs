using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilityTooltipUIDisplay : TooltipBaseUIDisplay
{
    public AbilityInfo abilityInfo;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI Cooldown;
    public TextMeshProUGUI CastTime;
    public TextMeshProUGUI Range;
    public TextMeshProUGUI Cost;

    public void OnEnable()
    {
        Name.text = abilityInfo.Name;
        Description.text = abilityInfo.Description;
        Cooldown.text = abilityInfo.Cooldown.ToString() + "sec Cooldown";
        CastTime.text = abilityInfo.CastTime == 0 ? "Instant" : abilityInfo.CastTime.ToString() + "sec Cast";
        Range.text = abilityInfo.Range >= 5 ? "Melee Range" : abilityInfo.Range.ToString() + "yd Range";
        Cost.text = abilityInfo.ResourceCost == 0 ? "" : abilityInfo.ResourceCost.ToString() + " " + GetResourceName();
        StartCoroutine(WaitForFrame());
    }

    private string GetResourceName()
    {
        switch (abilityInfo.ClassType)
        {
            case Assets.Scripts.Enums.CharacterClassType.Blademaster:
                return "Rage";
            case Assets.Scripts.Enums.CharacterClassType.Spellslinger:
            case Assets.Scripts.Enums.CharacterClassType.Soulmender:
                return "Mana";
            case Assets.Scripts.Enums.CharacterClassType.Hawkeye:
                return "Focus";
            default: return "";
        }
    }
}
