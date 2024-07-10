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

    public void OnEnable()
    {
        Name.text = AuraInfo.Aura.Name;
        Description.text = AuraInfo.Aura.Description;
        TimeRemaining.text = AuraInfo.ExpiresInSec.ToString() + "sec remaining";
        StartCoroutine(WaitForFrame());
    }

    public void FixedUpdate()
    {
        AuraInfo.ExpiresInSec = Mathf.CeilToInt(AuraManager.Instance.GetRemainingAuraDuration(AuraInfo.AppliedToId, AuraInfo.AuraId));
        TimeRemaining.text = AuraInfo.ExpiresInSec.ToString() + "sec remaining";
    }
}
