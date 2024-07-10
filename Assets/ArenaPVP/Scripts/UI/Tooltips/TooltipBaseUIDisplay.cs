using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class TooltipBaseUIDisplay : MonoBehaviour
{
    public Action OnTooltipInstantiated;

    public IEnumerator WaitForFrame()
    {
        yield return 0;
        OnTooltipInstantiated?.Invoke();
    }
}
