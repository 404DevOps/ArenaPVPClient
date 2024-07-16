using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
    [SerializeField] GameObject _floatingTextPrefab;
    private void OnPlayerHealthChanged(HealthChangedEventArgs args)
    {
        if (!args.Source.IsOwnedByMe) //if damage/healing wasnt done by me, dont show floating text.
            return;

        var isHeal = args.HealthChangeType == HealthChangeType.Heal;
        var absAmountChanged = Mathf.Abs(args.HealthChangeAmount);
        ShowText(args.Player, absAmountChanged, isHeal);
    }

    private void ShowText(Player player, float absAmountChanged, bool isHeal)
    {
        var gO = new GameObject();
        gO.SetActive(false);
        var floatTextGo = Instantiate(_floatingTextPrefab, gO.transform);
        var ftextComponent = floatTextGo.GetComponentInChildren<FloatingText>();
        ftextComponent.Color = isHeal ?  Color.green : Color.red;
        ftextComponent.Text = Mathf.CeilToInt(absAmountChanged).ToString();
        //ftextComponent.StickToObject = player.transform;
        floatTextGo.transform.parent = player.GetComponentInChildren<FloatingTextContainer>().transform; //, false);
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
