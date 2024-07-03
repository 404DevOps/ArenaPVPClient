using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Logger = Assets.Scripts.Helpers.Logger;

public class AbilityUIDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public AbilityBase Ability;
    public Image Icon;

    public GameObject TooltipPrefab;
    private GameObject Tooltip;
    private Transform canvas;

    private float offsetX = 30;
    private float offsetY = 30;
    private float abilityMenuOffsetX = 45;

    private bool isInActionSlot = false;
    private bool showTooltip = false;

    private float timer = 0f;
    private float delay = 0.2f;
    private void Start()
    {
        canvas = FindFirstObjectByType<Canvas>().transform;
        if (transform.GetComponent<ActionSlot>() != null)
            isInActionSlot = true;
        if(Ability != null)
            Icon.sprite = Ability.AbilityInfo.Icon;
    }

    public void Update()
    {
        if (showTooltip)
        {
            timer += Time.deltaTime;

            if (timer >= delay)
            { 
                ShowTooltip();
                showTooltip = false;
            }
        }
    }

    public void ShowTooltip()
    {
        //make inactive parent first so we can delay onEnable until abilityInfo is set
        GameObject parent = new GameObject();
        parent.SetActive(false);
        Tooltip = Instantiate(TooltipPrefab, parent.transform);
        var tooltip = Tooltip.GetComponent<AbilityTooltipUIDisplay>();
        tooltip.OnTooltipInstantiated += SetTooltipPosition;
        tooltip.abilityInfo = Ability.AbilityInfo;
        parent.SetActive(true);
        Tooltip.transform.parent = canvas.transform;
        Destroy(parent);
    }

    void SetTooltipPosition()
    {
        var tooltip = Tooltip.GetComponent<AbilityTooltipUIDisplay>();
        var rect = Tooltip.GetComponent<RectTransform>();

        if (isInActionSlot)
        {
            var pos = new Vector3(Screen.width - rect.sizeDelta.x / 2 - offsetX, 0 + rect.sizeDelta.y / 2 + offsetY);
            Tooltip.transform.position = pos;
        }
        else
        {
            Tooltip.transform.position = new Vector3(transform.position.x + rect.sizeDelta.x / 2 + abilityMenuOffsetX, transform.position.y);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Ability == null)
            return;

        showTooltip = true;
        timer = 0;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        showTooltip = false;
        Destroy(Tooltip);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        showTooltip = false;
        Destroy(Tooltip);
    }
}
