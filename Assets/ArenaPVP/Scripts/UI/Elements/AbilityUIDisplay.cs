using Assets.ArenaPVP.Scripts.Enums;
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

    private bool _isInActionSlot = false;
    private bool _showTooltip = false;
    private float _timer = 0f;
    private float _delay = 0.2f;

    private void Start()
    {
        if (transform.GetComponent<ActionSlot>() != null)
            _isInActionSlot = true;
        if(Ability != null)
            Icon.sprite = Ability.AbilityInfo.Icon;
    }
    public void Update()
    {
        if (_showTooltip)
        {
            _timer += Time.deltaTime;

            if (_timer >= _delay)
            {
                if (_isInActionSlot)
                    UIEvents.OnShowTooltip.Invoke(TooltipType.AbilitySlot, Ability.AbilityInfo);
                else
                    UIEvents.OnShowTooltip.Invoke(TooltipType.AbilityMenu, Ability.AbilityInfo);

                _showTooltip = false;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Ability == null)
            return;

        _showTooltip = true;
        _timer = 0;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _showTooltip = false;
        UIEvents.OnHideTooltip.Invoke();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _showTooltip = false;
        UIEvents.OnHideTooltip.Invoke();
    }
}
