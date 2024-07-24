using Assets.ArenaPVP.Scripts.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AuraDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AuraInfo AuraInfo;
    public Image AuraIcon;
    public Image AuraIconDark;
    public Image AuraBorder;
    public TextMeshProUGUI StackAmountText;
    public TextMeshProUGUI ExpiresInText;

    private bool _showTooltip = false;
    private float _timer = 0f;
    private float _delay = 0.2f;
    private float _expiresIn;
    private int _stackAmount = 0;

    public void OnEnable()
    {
        AuraIcon.sprite = AuraInfo.Aura.Icon;
        AuraIconDark.sprite = AuraInfo.Aura.Icon;
        AuraBorder.color = AuraInfo.Aura.isDebuff ? Color.red : Color.green;
        StackAmountText.gameObject.SetActive(false);
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

        _expiresIn = AuraManager.Instance.GetRemainingAuraDuration(AuraInfo.AppliedTo.Id, AuraInfo.AuraId);
        _stackAmount = AuraManager.Instance.GetStackAmount(AuraInfo.AppliedTo.Id, AuraInfo.AuraId);
    }

    public void LateUpdate()
    {
        if (_expiresIn >= 0)
        {
            var percentage = _expiresIn / AuraInfo.Aura.Duration;
            AuraIcon.fillAmount = percentage;
        }
        if (_expiresIn <= 3)
        {
            ExpiresInText.text = Mathf.CeilToInt(_expiresIn).ToString();
            ExpiresInText.color = Color.red;
        }
        else if (_expiresIn <= 5)
        {
            ExpiresInText.gameObject.SetActive(true);
            ExpiresInText.text = Mathf.CeilToInt(_expiresIn).ToString();
        }
        else 
        {
            ExpiresInText.gameObject.SetActive(false);
            ExpiresInText.color = Color.yellow;
        }

        if (_stackAmount > 1)
        {
            StackAmountText.gameObject.SetActive(true);
            StackAmountText.text = _stackAmount.ToString();
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
