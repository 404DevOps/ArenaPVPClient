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

    public Image IconImage;

    public Image HealthbarImage;
    public Image ManabarImage;

    public Player Player;

    [SerializeField] private bool _isPlayerFrame;
    private int _ownerId;
    private PlayerHealth _playerHealth;
    private float _updateHealthSpeed = 0.2f;

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
            SetUnitFrameAppearance();
            _ownerId = Player.transform.GetInstanceID();
            _playerHealth = Player.GetComponent<PlayerHealth>();
        }
        else 
        {
            UIEvents.OnTargetChanged.AddListener(OnTargetChanged);
            FrameParent.gameObject.SetActive(false);
        }
        GameEvents.OnPlayerHealthChanged.AddListener(OnHealthChanged);
    }

    private void OnHealthChanged(int ownerId, float healthChanged)
    {
        if (ownerId != _ownerId)
            return;

        StartCoroutine(SmoothChangeHealth(_playerHealth.CurrentHealth / _playerHealth.MaxHealth));
    }

    private IEnumerator SmoothChangeHealth(float healthPercentage)
    {
        float preChangePercentage = HealthbarImage.fillAmount;
        float elapsed = 0f;
        while (elapsed < _updateHealthSpeed)
        {
            elapsed += Time.deltaTime;
            HealthbarImage.fillAmount = Mathf.Lerp(preChangePercentage, healthPercentage, elapsed / _updateHealthSpeed);
            yield return null;
        }

        HealthbarImage.fillAmount = healthPercentage;
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
            _ownerId = Player.transform.GetInstanceID();
            _playerHealth = Player.GetComponent<PlayerHealth>();
            SetUnitFrameAppearance();
            SetHealthInstant();
            FrameParent.gameObject.SetActive(true);
        }
        else 
        {
            FrameParent.gameObject.SetActive(false);
        }
    }

    public void SetHealthInstant()
    {
        var percentage = _playerHealth.CurrentHealth / _playerHealth.MaxHealth;
        HealthbarImage.fillAmount = percentage;
    }

    private void SetUnitFrameAppearance()
    {
        HealthbarImage.color = ClassAppearanceData.Instance().GetColor(Player.ClassType);
        IconImage.sprite = ClassAppearanceData.Instance().GetIcon(Player.ClassType);
    }
}
