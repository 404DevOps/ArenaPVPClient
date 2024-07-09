using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Logger = Assets.Scripts.Helpers.Logger;

public class AuraDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AuraInfo AuraInfo;
    public Image AuraIcon;
    public Image AuraBorder;

    public void OnEnable()
    {
        AuraIcon.sprite = AuraInfo.Aura.Icon;
        AuraBorder.color = AuraInfo.Aura.isDebuff ? Color.red : Color.green;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Logger.Log("Show Aura Tooltip");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Logger.Log("Hide Aura Tooltip");
    }
}
