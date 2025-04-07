using Assets.ArenaPVP.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{

    public GameObject auraTooltipPrefab;
    public GameObject abilityTooltipPrefab;

    #region private Variables
    private Transform _canvas;
    private GameObject _tooltip;
    private TooltipType _type;

    private float offsetX = 30;
    private float offsetY = 30;
    private float abilityMenuOffsetX = 45;
    #endregion
    #region Singleton

    private static TooltipManager _instance;
    public static TooltipManager Instance => _instance;
    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);
    }

    #endregion
    #region RegisterEvents
    private void OnEnable()
    {
        _canvas = FindAnyObjectByType<Canvas>().transform;
        UIEvents.OnShowTooltip.AddListener(OnShowTooltip);
        UIEvents.OnHideTooltip.AddListener(DestroyTooltip);
    }
    private void OnDisable()
    {
        UIEvents.OnShowTooltip.RemoveListener(OnShowTooltip);
        UIEvents.OnHideTooltip.RemoveListener(DestroyTooltip);
    }
    #endregion

    public void OnShowTooltip(TooltipType tooltipType, object tooltipInfo)
    {
        DestroyTooltip();
        _type = tooltipType;

        switch (tooltipType)
        {
            case TooltipType.Aura:
                InstantiateTooltip((AuraInfo)tooltipInfo);
                break;
            case TooltipType.AbilityMenu:
            case TooltipType.AbilitySlot:
                InstantiateTooltip((AbilityBase)tooltipInfo);
                break;
            default: break;
        }
    }
    void SetTooltipPosition()
    {
        var rect = _tooltip.GetComponent<RectTransform>();

        if (_type == TooltipType.AbilityMenu)
        {
            _tooltip.transform.position = new Vector3(transform.position.x + rect.sizeDelta.x / 2 + abilityMenuOffsetX, transform.position.y);
        }
        else
        {
            _tooltip.transform.position = new Vector3(Screen.width - rect.sizeDelta.x / 2 - offsetX, 0 + rect.sizeDelta.y / 2 + offsetY);
        }
    }

    void DestroyTooltip()
    {
        if (_tooltip != null)
            Destroy(_tooltip);
    }

    private void InstantiateTooltip(AbilityBase ability)
    {
        var go = new GameObject();
        go.SetActive(false);
        _tooltip = Instantiate(abilityTooltipPrefab, go.transform);
        var tooltipComponent = _tooltip.GetComponent<AbilityTooltipUIDisplay>();
        tooltipComponent.Ability = ability;
        _tooltip.transform.SetParent(_canvas);
        tooltipComponent.OnTooltipInstantiated += SetTooltipPosition;
        Destroy(go);
    }
    private void InstantiateTooltip(AuraInfo tooltipInfo)
    {
        var go = new GameObject();
        go.SetActive(false);
        _tooltip = Instantiate(auraTooltipPrefab, go.transform);
        var tooltipComponent = _tooltip.GetComponent<AuraTooltipUIDisplay>();
        tooltipComponent.AuraInfo = tooltipInfo;
        _tooltip.transform.SetParent(_canvas);
        tooltipComponent.OnTooltipInstantiated += SetTooltipPosition;
        tooltipComponent.OnAuraExpired += DestroyTooltip;
        Destroy(go);
    }
}
