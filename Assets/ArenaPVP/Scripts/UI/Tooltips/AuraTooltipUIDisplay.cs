using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AuraTooltipUIDisplay : TooltipBaseUIDisplay
{
    public AuraInfo AuraInfo;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI TimeRemaining;

    public Action OnAuraExpired;

    public void OnEnable()
    {
        var aura = AbilityStorage.GetAura(AuraInfo.AuraId);
        var secRemaining = Mathf.CeilToInt(AuraManager.Instance.GetRemainingAuraDuration(AuraInfo.AppliedTo.Id, AuraInfo.AuraId));
        Name.text = aura.Name;
        Description.text = aura.Description;
        TimeRemaining.text = secRemaining + "sec remaining";
        StartCoroutine(WaitForFrame());
    }

    public void FixedUpdate()
    {
        var secRemaining = Mathf.CeilToInt(AuraManager.Instance.GetRemainingAuraDuration(AuraInfo.AppliedTo.Id, AuraInfo.AuraId));
        TimeRemaining.text = secRemaining.ToString() + "sec remaining";

        if (secRemaining <= 0)
            OnAuraExpired.Invoke();
    }
}
