using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
    [SerializeField] GameObject _floatingTextPrefab;

    private void OnPlayerHealthChanged(Player player, float healthChanged)
    {
        var isHeal = healthChanged > 0;
        var absAmountChanged = Mathf.Abs(healthChanged);
        ShowText(player, absAmountChanged, isHeal);
    }

    private void ShowText(Player player, float absAmountChanged, bool isHeal)
    {
        var gO = new GameObject();
        gO.SetActive(false);
        var floatTextGo = Instantiate(_floatingTextPrefab, gO.transform);
        var ftextComponent = floatTextGo.GetComponent<FloatingText>();
        ftextComponent.Color = isHeal ?  Color.green : Color.red;
        ftextComponent.Text = Mathf.CeilToInt(absAmountChanged).ToString();
        floatTextGo.transform.SetParent(player.transform);
        Destroy(gO);
    }

    void OnDisable()
    {
        GameEvents.OnPlayerHealthChanged.RemoveListener(OnPlayerHealthChanged);
    }
    void OnEnable()
    {
        GameEvents.OnPlayerHealthChanged.AddListener(OnPlayerHealthChanged);
    }
}
