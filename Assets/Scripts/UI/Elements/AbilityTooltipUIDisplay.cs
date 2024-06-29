using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilityTooltipUIDisplay : MonoBehaviour
{
    public AbilityInfo abilityInfo;
    public TextMeshProUGUI abilityTitle;
    public TextMeshProUGUI abilityDescription;

    void Start()
    {
        abilityTitle.text = abilityInfo.Name;
        abilityTitle.text = abilityInfo.Description;
    }

}
