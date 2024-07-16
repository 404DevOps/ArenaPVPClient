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
    public Image ManabarImage;
    private Healthbar _healthbar;

    public Player Player;

    [SerializeField] private bool _isPlayerFrame;
    private PlayerHealth _playerHealth;



    // Start is called before the first frame update
    void OnEnable()
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
            Player = FindObjectsOfType<Player>().First(p => p.IsOwnedByMe);
            SetUnitFrameIcon();
            _playerHealth = Player.GetComponent<PlayerHealth>();
            ActivateAuraGrid();
        }
        else 
        {
            UIEvents.OnTargetChanged.AddListener(OnTargetChanged);
            FrameParent.gameObject.SetActive(false);
            AuraContainer.gameObject.SetActive(false);
        }
        GameEvents.OnPlayerHealthChanged.AddListener(OnHealthChanged);
        _healthbar = GetComponentInChildren<Healthbar>(true);
    }

    private void OnHealthChanged(HealthChangedEventArgs args)
    {
        if (Player.Id != args.Player.Id)
            return;

        _healthbar.SetNewHealth(_playerHealth.CurrentHealth, _playerHealth.MaxHealth);
    }

    private void OnDisable()
    {
        if (!_isPlayerFrame)
        {
            UIEvents.OnTargetChanged.RemoveListener(OnTargetChanged);
        }
        GameEvents.OnPlayerHealthChanged.RemoveListener(OnHealthChanged);
    }
    private void OnTargetChanged(Player player) 
    {
        if (player != null)
        {
            Player = player;
            _playerHealth = Player.GetComponent<PlayerHealth>();
            SetUnitFrameIcon();
            _healthbar.InitializeBar(Player.ClassType, _playerHealth.CurrentHealth, _playerHealth.MaxHealth);
            FrameParent.gameObject.SetActive(true);
            ActivateAuraGrid();
            
        }
        else 
        {
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
        IconImage.sprite = ClassAppearanceData.Instance().GetIcon(Player.ClassType);
    }
}
