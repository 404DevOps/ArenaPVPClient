using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
    [SerializeField] GameObject _floatingTextPrefab;
    [SerializeField] GameObject _textContainerPrefab;
    [SerializeField] Transform _textContainerParent;

    private Dictionary<int, Transform> _playerTextContainers = new Dictionary<int, Transform>();
    private void OnPlayerHealthChanged(HealthChangedEventArgs args)
    {
        if (!args.Source.IsOwnedByMe) //if damage/healing wasnt done by me, dont show floating text.
            return;
        TryInitializePlayerTextContainers(args.Player);
        var absAmountChanged = Mathf.Abs(args.HealthChangeAmount);
        InstantiateText(args.Player, absAmountChanged, args.HealthChangeType);
    }

    private void InstantiateText(Player player, float absAmountChanged, HealthChangeType healthChangeType)
    {
        var gO = new GameObject();
        gO.SetActive(false);
        var floatTextGo = Instantiate(_floatingTextPrefab, gO.transform);
        var ftextComponent = floatTextGo.GetComponentInChildren<FloatingText>();
        ftextComponent.Color = AppearanceData.Instance().GetFloatingTextColor(healthChangeType);
        ftextComponent.Text = Mathf.CeilToInt(absAmountChanged).ToString();
        var playerTransform = _playerTextContainers[player.Id];
        floatTextGo.transform.SetParent(playerTransform, false); //, false);
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

    private void TryInitializePlayerTextContainers(Player player)
    {
        if (player.IsOwnedByMe) return;

        if (!_playerTextContainers.ContainsKey(player.Id))
        {
            var transform = InstantiateContainer(player);
            _playerTextContainers.Add(player.Id, transform);
        }
    }

    private Transform InstantiateContainer(Player player)
    {
        var gO = new GameObject();
        gO.SetActive(false);
        var containerGo = Instantiate(_textContainerPrefab, gO.transform);
        var containerComponent = containerGo.GetComponent<FloatingTextContainer>();
        containerComponent.Player = player;
        containerGo.transform.SetParent(_textContainerParent, false);
        Destroy(gO);

        return containerGo.transform;
    }
}
