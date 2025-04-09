using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

internal class StaminaBarUIHandler : MonoBehaviour
{
    public float DisableDelay;
    private Coroutine _disableCoroutine;
    Entity _localPlayer;
    EntityStamina _stamina;

    GameObject StaminaBarContainer;

    public Image Fill;
    public Image Bar;

    public void OnEnable()
    {
        StaminaBarContainer = Bar.gameObject;
        SetActive(true);
        Fill.fillAmount = 1;
        ClientEvents.OnEntityStaminaChanged.AddListener(OnStaminaChanged);
    }

    private void SetActive(bool active)
    {
        if (StaminaBarContainer.activeSelf != active) ;
            StaminaBarContainer.SetActive(active);
        if (active)
        {
            SetAlpha(1);
        }
    }

    private void SetAlpha(float newAlpha)
    {
        var fillcolor = Fill.color;
        fillcolor.a = newAlpha;
        Fill.color = fillcolor;

        var borderColor = Bar.color;
        borderColor.a = newAlpha;
        Bar.color = borderColor;
    }

    public void OnDisable()
    {
        ClientEvents.OnEntityStaminaChanged.RemoveListener(OnStaminaChanged);
    }

    private void OnStaminaChanged(StaminaChangedEventArgs args)
    {
       if(_localPlayer == null) Initialize();
        if (_stamina.CurrentStamina.Value >= _stamina.MaxStamina.Value)
            _disableCoroutine = StartCoroutine(StartDisableCoroutine(DisableDelay));
        else
        {
            if(_disableCoroutine != null)
                StopCoroutine(_disableCoroutine);
            SetActive(true);
        }

        var perc = _stamina.CurrentStamina.Value / _stamina.MaxStamina.Value;
        Fill.fillAmount = perc;

        if (perc < 0.3)
            SetBarColor(Color.red);
        else
            SetBarColor(Color.green);
    }

    private IEnumerator StartDisableCoroutine(float delay)
    {
        var timeRemaining = delay;

        while (timeRemaining > 0)
        {
            var perc = timeRemaining / delay;
            SetAlpha(perc);
            timeRemaining -= Time.deltaTime;
            yield return null;
        }

        SetActive(false);
    }

    private void SetBarColor(Color color)
    {
        if(Fill.color != color)
            Fill.color = color;
    }

    private void Initialize()
    {
        _localPlayer = Helpers.LocalPlayer;
        _stamina = _localPlayer.GetComponent<EntityStamina>();
    }
}

