using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NameplateUIHandler : MonoBehaviour
{
    public Player Player;
    public Transform CastbarParent;
    public Transform HealthbarParent;
    public Image HealthbarImage;

    public float updateHealthSpeed = 0.2f;

    private PlayerHealth _playerHealth;
    private int _ownerId;
    // Start is called before the first frame update
    void OnEnable()
    {
        _playerHealth = Player.GetComponent<PlayerHealth>();
        _ownerId = Player.transform.GetInstanceID();
        GameEvents.OnPlayerHealthChanged.AddListener(OnHealthChanged);

        HealthbarImage.color = ClassAppearanceData.Instance().GetColor(Player.ClassType);
        if(Player.IsOwnedByMe)
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

        StartCoroutine(SmoothChangeHealth(_playerHealth.CurrentHealth / _playerHealth.MaxHealth));
    }

    private IEnumerator SmoothChangeHealth(float healthPercentage)
    {
        float preChangePercentage = HealthbarImage.fillAmount;
        float elapsed = 0f;
        while (elapsed < updateHealthSpeed)
        {
            elapsed += Time.deltaTime;
            HealthbarImage.fillAmount = Mathf.Lerp(preChangePercentage, healthPercentage, elapsed / updateHealthSpeed);
            yield return null;
        }

        HealthbarImage.fillAmount = healthPercentage;
    }
}
