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

    public AbilityBase _ability;
    public Player player;

    private bool _isCasting;

    // Start is called before the first frame update
    private void OnEnable()
    {
        player = FindObjectOfType<Player>();
        GameEvents.OnCastStarted.AddListener(OnCastStarted);
        GameEvents.OnCastInterrupted.AddListener(OnCastInterrupted);
        GameEvents.OnCastCompleted.AddListener(OnCastCompleted);
    }
    private void OnDisable()
    {
        _isCasting = false;
        GameEvents.OnCastStarted.RemoveListener(OnCastStarted);
        GameEvents.OnCastInterrupted.RemoveListener(OnCastInterrupted);
        GameEvents.OnCastCompleted.RemoveListener(OnCastCompleted);
    }

    public void OnCastStarted(AbilityBase ability)
    {
        this._ability = ability;
        Fill.fillAmount = 0;
        Fill.color = CastbarColor;
        Icon.sprite = _ability.AbilityInfo.Icon;
        AbilityNameText.text = _ability.AbilityInfo.Name;
        CurrentCastTime.text = "0";
        MaxCastTime.text = "/ " + _ability.AbilityInfo.CastTime.ToString();
        _isCasting = true;
        CastBarParent.SetActive(true);
    }
    public void OnCastInterrupted()
    {
        Fill.fillAmount = 1;
        Fill.color = Color.red;
        AbilityNameText.text = "Interrupted";
        CurrentCastTime.text = "0";
        StartCoroutine(SetInvisibleAfterTime(0.3f));
    }
    public void OnCastCompleted()
    {
        Fill.fillAmount = 1;
        Fill.color = Color.green;
        AbilityNameText.text = "Complete";
        CurrentCastTime.text = _ability.AbilityInfo.CastTime.ToString("0.0");
        StartCoroutine(SetInvisibleAfterTime(0.3f));
    }

    IEnumerator SetInvisibleAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _isCasting = false;
        _ability = null;
        CastBarParent.SetActive(false);
    }

    void Update()
    {
        if (_ability != null)
        {
            if (_isCasting)
            {
                float timeSinceCastStart = CastManager.Instance.TimeSinceCastStarted(player.transform.GetInstanceID(), _ability.AbilityInfo.Name);
                if (timeSinceCastStart > 0)
                {
                    var percentage = timeSinceCastStart / _ability.AbilityInfo.CastTime;
                    Fill.fillAmount = percentage;
                    CurrentCastTime.text = timeSinceCastStart.ToString("0.0");
                }
            }
        }
    }
}
