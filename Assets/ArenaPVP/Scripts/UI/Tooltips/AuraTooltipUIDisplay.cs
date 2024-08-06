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
        Name.text = aura.Name;
        Description.text = aura.Description;
        TimeRemaining.text = AuraInfo.ExpiresInSec.ToString() + "sec remaining";
        StartCoroutine(WaitForFrame());
    }

    public void FixedUpdate()
    {
        AuraInfo.ExpiresInSec = Mathf.CeilToInt(AuraManager.Instance.GetRemainingAuraDuration(AuraInfo.AppliedTo.Id, AuraInfo.AuraId));
        TimeRemaining.text = AuraInfo.ExpiresInSec.ToString() + "sec remaining";

        if (AuraInfo.ExpiresInSec <= 0)
            OnAuraExpired.Invoke();
    }
}
