using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CastBarUIHandler : MonoBehaviour
{

    public GameObject CastBarParent;
    public Image Fill;
    public Image Icon;
    public TextMeshProUGUI CurrentCastTime;
    public TextMeshProUGUI MaxCastTime;
    public TextMeshProUGUI AbilityNameText;
    public Color CastbarColor;

    public AbilityInfo _ability;

    private bool _isCasting;

    // Start is called before the first frame update
    private void OnEnable()
    {
        GameEvents.OnCastStarted.AddListener(OnCastStarted);
        GameEvents.OnCastStopped.AddListener(OnCastStopped);

        Fill = GetComponent<Image>();
        _isCasting = true;
    }
    private void OnDisable()
    {
        _isCasting = false;
        GameEvents.OnCastStarted.RemoveListener(OnCastStarted);
        GameEvents.OnCastStopped.RemoveListener(OnCastStopped);
    }

    public void OnCastStarted(AbilityInfo Ability)
    {
        this._ability = Ability;
        Fill.fillAmount = 0;
        Fill.color = CastbarColor;
        Icon.sprite = Ability.Icon;
        AbilityNameText.text = Ability.Name;
        CurrentCastTime.text = "0";
        MaxCastTime.text = Ability.CastTime.ToString();
        CastBarParent.SetActive(true);
    }
    public void OnCastStopped()
    {
        Fill.fillAmount = 1;
        Fill.color = Color.red;
        CurrentCastTime.text = "Interrupted";
        StartCoroutine(SetInvisibleAfterTime(0.3f));
    }

    IEnumerator SetInvisibleAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        CastBarParent.SetActive(false);
        _ability = null;
    }

    void Update()
    {
        if (_ability != null)
        {
            if (_isCasting)
            {
                float timeSinceCastStart = 0;// CastManager.getStart
                var percentage = timeSinceCastStart / _ability.CastTime;
                if (percentage > 1)
                {
                    Fill.color = Color.green;
                    StartCoroutine(SetInvisibleAfterTime(0.3f));
                }
                else
                {
                    Fill.fillAmount = percentage;
                }
            }

        }
    }
}
