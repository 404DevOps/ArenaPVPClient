using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatLogUIHandler : MonoBehaviour
{
    [SerializeField] Scrollbar _scrollbar;
    [SerializeField] Transform _content;
    [SerializeField] GameObject _logEntryPrefab;
    private int _maxChildAmount = 30;
    // Start is called before the first frame update
    void OnEnable()
    {
        GameEvents.OnPlayerHealthChanged.AddListener(OnPlayerHealthChanged);
        GameEvents.OnAuraApplied.AddListener(OnAuraApplied);
        GameEvents.OnAuraExpired.AddListener(OnAuraExpired);
    }
    void OnDisable()
    {
        GameEvents.OnPlayerHealthChanged.RemoveListener(OnPlayerHealthChanged);
        GameEvents.OnAuraApplied.RemoveListener(OnAuraApplied);
        GameEvents.OnAuraExpired.RemoveListener(OnAuraExpired);
    }

    private void OnAuraExpired(int arg1, int arg2)
    {
        //throw new NotImplementedException();
    }

    private void OnAuraApplied(int arg1, int arg2)
    {
        //throw new NotImplementedException();
    }

    private void OnPlayerHealthChanged(HealthChangedEventArgs args)
    {
        var hitOrHeal = args.HealthChangeType == HealthChangeType.Heal ? "heals" : "hits";
        var str = $"{args.Source.Name}'s {args.Ability.name} {hitOrHeal} {args.Player.Name} for {Mathf.Abs(args.HealthChangeAmount)} {args.DamageType} Damage.";
        var color = GetCombatLogTextColor(args);
        InstantiateText(str, color);
    }

    private Color GetCombatLogTextColor(HealthChangedEventArgs args)
    {
        if (args.Player.IsOwnedByMe && args.HealthChangeType == HealthChangeType.Damage)
            return Color.red;
        if (args.Player.IsOwnedByMe && args.HealthChangeType == HealthChangeType.Heal)
            return Color.green;
        if (args.Source.IsOwnedByMe && args.HealthChangeType == HealthChangeType.Heal)
            return Color.cyan;
        if (args.Source.IsOwnedByMe && args.HealthChangeType == HealthChangeType.Damage)
            return Color.white;

        return Color.white;
    }

    private void InstantiateText(string str, Color color)
    {
        var gO = new GameObject();
        gO.SetActive(false);
        var logEntryGo = Instantiate(_logEntryPrefab, gO.transform);
        var logEntryComponent = logEntryGo.GetComponent<LogEntry>();
        logEntryComponent.Text = str;
        logEntryComponent.TextColor = color;
        logEntryGo.transform.SetParent(_content, false);
        Destroy(gO);

        //set scrollbar to bottom when new text is added.
        _scrollbar.value = 0;
    }

    private void Update()
    {
        if (_content.childCount > _maxChildAmount)
        {
            var childrenToDelete = _content.childCount - _maxChildAmount;
            for (int i = 0; i < childrenToDelete; i++)
            {
                DestroyImmediate(_content.GetChild(0).gameObject);
            }
        }
    }
}
