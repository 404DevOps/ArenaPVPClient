using Assets.ArenaPVP.Scripts.Enums;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AuraDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AuraInfo AuraInfo;
    public Image AuraIcon;
    public Image AuraBorder;

    private bool _showTooltip = false;
    private float _timer = 0f;
    private float _delay = 0.2f;

    public void OnEnable()
    {
        AuraIcon.sprite = AuraInfo.Aura.Icon;
        AuraBorder.color = AuraInfo.Aura.isDebuff ? Color.red : Color.green;
    }

    public void Update()
    {
        if (_showTooltip)
        {
            _timer += Time.deltaTime;

            if (_timer >= _delay)
            {
                UIEvents.OnShowTooltip.Invoke(TooltipType.Aura, AuraInfo);
                _showTooltip = false;
            }
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (AuraInfo == null)
            return;

        _showTooltip = true;
        _timer = 0;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _showTooltip = false;
        UIEvents.OnHideTooltip.Invoke();
    }
}
