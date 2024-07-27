using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UnitFrameUIHandler : MonoBehaviour
{
    public bool IconRightSide;

    public Transform FrameParent;
    public Transform BarHolder;
    public Transform IconHolder;
    public AuraContainerUIHandler AuraContainer;

    public Image IconImage;
    private Healthbar _healthbar;
    private Resourcebar _resourcebar;

    public Player Player;

    [SerializeField] private bool _isPlayerFrame;
    private PlayerHealth _playerHealth;
    private PlayerResource _playerResource;

    // Start is called before the first frame update
    void OnEnable()
    {
        _healthbar = GetComponentInChildren<Healthbar>(true);
        _resourcebar = GetComponentInChildren<Resourcebar>(true);

        GameEvents.OnPlayerInitialized.AddListener(OnPlayerInitialized);
        GameEvents.OnPlayerHealthChanged.AddListener(OnHealthChanged);
        GameEvents.OnPlayerResourceChanged.AddListener(OnResourceChanged);
    }
    private void OnDisable()
    {
        if (!_isPlayerFrame)
        {
            UIEvents.OnTargetChanged.RemoveListener(OnTargetChanged);
        }
        GameEvents.OnPlayerHealthChanged.RemoveListener(OnHealthChanged);
        GameEvents.OnPlayerInitialized.RemoveListener(OnPlayerInitialized);
    }

    private void OnPlayerInitialized(Player player)
    {
        
        if (IconRightSide)
        {
            IconHolder.SetAsLastSibling();
            var verticalLayoutGrp = BarHolder.GetComponent<VerticalLayoutGroup>();
            verticalLayoutGrp.padding.left = 5;
            verticalLayoutGrp.padding.right = 0;
        }
        if (_isPlayerFrame)
        {
            if (player.IsOwnedByMe) {
                Player = player;
                SetUnitFrameIcon();
                _playerHealth = Player.GetComponent<PlayerHealth>();
                _playerResource = Player.GetComponent<PlayerResource>();
                _healthbar.InitializeBar(Player.ClassType, _playerHealth.CurrentHealth, _playerHealth.MaxHealth);
                _resourcebar.InitializeBar(Player.ClassType, _playerResource.CurrentResource, _playerResource.MaxResource);
                ActivateAuraGrid(); 
            }
        }
        else
        {
            UIEvents.OnTargetChanged.AddListener(OnTargetChanged);
            FrameParent.gameObject.SetActive(false);
            AuraContainer.gameObject.SetActive(false);
        }
    }
    private void OnHealthChanged(HealthChangedEventArgs args)
    {
        if (Player == null || Player.Id != args.Player.Id)
            return;

        _healthbar.SetNewHealth(_playerHealth.CurrentHealth + args.HealthChangeAmount, _playerHealth.MaxHealth);
    }
    private void OnResourceChanged(ResourceChangedEventArgs args)
    {
        if (Player == null || Player.Id != args.Player.Id)
            return;

        _resourcebar.SetNewValue(_playerResource.CurrentResource + args.ResourceChangeAmount, _playerResource.MaxResource);
    }
    private void OnTargetChanged(Player player) 
    {
        if (player != null)
        {
            Player = player;
            _playerHealth = Player.GetComponent<PlayerHealth>();
            _playerResource = Player.GetComponent<PlayerResource>();
            SetUnitFrameIcon();
            _healthbar.InitializeBar(Player.ClassType, _playerHealth.CurrentHealth, _playerHealth.MaxHealth);
            _resourcebar.InitializeBar(Player.ClassType, _playerResource.CurrentResource, _playerResource.MaxResource);
            FrameParent.gameObject.SetActive(true);
            ActivateAuraGrid();
            
        }
        else 
        {
            Player = null;
            AuraContainer.gameObject.SetActive(false);
            FrameParent.gameObject.SetActive(false);
        }
    }
    private void ActivateAuraGrid()
    {
        AuraContainer.InitializeGrid(Player.Id, AuraManager.Instance.GetAuraInfosForPlayer(Player.Id));
        AuraContainer.gameObject.SetActive(true);
    }
    private void SetUnitFrameIcon()
    {
        IconImage.sprite = AppearanceData.Instance().GetClassIcon(Player.ClassType);
    }
}
