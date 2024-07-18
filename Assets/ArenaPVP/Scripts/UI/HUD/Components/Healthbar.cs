using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    private float _currentHealth = 1;
    private float _maxHealth = 1;

    [SerializeField] private Image _healthbarImage;
    [SerializeField] private float _updateHealthSpeed;

    public void InitializeBar(CharacterClassType classType, float currentHealth, float maxHealth)
    {
        SetBarClassColor(classType);
        SetNewHealth(currentHealth, maxHealth, true);
    }
    public void SetNewHealth(float currentHealth, float maxHealth, bool updateInstant = false)
    {
        _currentHealth = currentHealth;
        _maxHealth = maxHealth;
        UpdateBar(updateInstant);
    }

    private void SetBarClassColor(CharacterClassType classType)
    {
        _healthbarImage.color = AppearanceData.Instance().GetClassColor(classType);
    }
    private void UpdateBar(bool updateInstant = false)
    {
        var percentage = _currentHealth / _maxHealth;

        if (!updateInstant)
            StartCoroutine(SmoothChangeHealth(percentage));
        else
            SetHealthInstant(percentage);
    }
    private void SetHealthInstant(float percentage)
    {
        _healthbarImage.fillAmount = percentage;
    }
    private IEnumerator SmoothChangeHealth(float healthPercentage)
    {
        float preChangePercentage = _healthbarImage.fillAmount;
        float elapsed = 0f;
        while (elapsed < _updateHealthSpeed)
        {
            elapsed += Time.deltaTime;
            _healthbarImage.fillAmount = Mathf.Lerp(preChangePercentage, healthPercentage, elapsed / _updateHealthSpeed);
            yield return null;
        }

        _healthbarImage.fillAmount = healthPercentage;
    }
}
