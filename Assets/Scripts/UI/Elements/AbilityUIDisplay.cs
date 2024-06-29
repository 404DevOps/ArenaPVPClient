using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityUIDisplay : MonoBehaviour
{
    public CharacterAbility Ability;
    public Image Icon;

    public GameObject TooltipPrefab;
    private GameObject CurrentTooltip;

    public bool isBeingDragged = false;
    private void Start()
    {
        if(Ability != null)
            Icon.sprite = Ability.AbilityInfo.Icon;
    }

    private void OnMouseEnter()
    {
        if (isBeingDragged)
            return;

        CurrentTooltip = Instantiate(TooltipPrefab);
        var tooltip = CurrentTooltip.GetComponent<AbilityTooltipUIDisplay>();
        tooltip.abilityInfo = Ability.AbilityInfo;
        var rect = CurrentTooltip.GetComponent<RectTransform>();
        CurrentTooltip.transform.position = new Vector3(transform.position.y - rect.sizeDelta.y / 2, transform.position.x + rect.sizeDelta.x / 2);
    }

    private void OnMouseExit()
    {
        if (isBeingDragged)
            return;

        Destroy(CurrentTooltip);
    }
}
