using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NameplateUIHandler : MonoBehaviour
{
    public Entity Player;
    public Transform CastbarParent;
    public Transform HealthbarParent;

    public float updateHealthSpeed = 0.2f;

    private EntityHealth _playerHealth;
    private Healthbar _healthbar;
    // Start is called before the first frame update
    void OnEnable()
    {
        _playerHealth = Player.GetComponent<EntityHealth>();
        ClientEvents.OnEntityHealthChanged.AddListener(OnHealthChanged);

        _healthbar = GetComponentInChildren<Healthbar>();
        _healthbar.InitializeBar(Player.ClassType, _playerHealth.CurrentHealth.Value, _playerHealth.MaxHealth.Value);

        if (Player.IsOwnedByMe)
            Destroy(CastbarParent.gameObject);
    }
    void OnDisable()
    {
        ClientEvents.OnEntityHealthChanged.RemoveListener(OnHealthChanged);
    }

    private void OnHealthChanged(HealthChangedEventArgs args)
    {
        if (args.Player.Id != Player.Id)
            return;

        _healthbar.SetNewHealth(_playerHealth.CurrentHealth.Value, _playerHealth.MaxHealth.Value);
    }
}
