using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NameplateUIHandler : MonoBehaviour
{
    public Player Player;
    public Transform CastbarParent;
    public Transform HealthbarParent;

    public float updateHealthSpeed = 0.2f;

    private PlayerHealth _playerHealth;
    private Healthbar _healthbar;
    private int _ownerId;
    // Start is called before the first frame update
    void OnEnable()
    {
        _playerHealth = Player.GetComponent<PlayerHealth>();
        _ownerId = Player.transform.GetInstanceID();
        GameEvents.OnPlayerHealthChanged.AddListener(OnHealthChanged);

        _healthbar = GetComponentInChildren<Healthbar>();
        _healthbar.InitializeBar(Player.ClassType, _playerHealth.CurrentHealth, _playerHealth.MaxHealth);

        if (Player.IsOwnedByMe)
            Destroy(CastbarParent.gameObject);
    }
    void OnDisable()
    {
        GameEvents.OnPlayerHealthChanged.RemoveListener(OnHealthChanged);
    }

    private void OnHealthChanged(int ownerId, float healthChanged)
    {
        if (ownerId != _ownerId)
            return;

        _healthbar.SetNewHealth(_playerHealth.CurrentHealth, _playerHealth.MaxHealth);
    }
}
