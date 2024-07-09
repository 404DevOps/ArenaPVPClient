using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manabar : MonoBehaviour
{
    private float _currentMana = 1;
    private float _maxMana = 1;

    [SerializeField] private Image _manabarImage;
    [SerializeField] private float _updateManaSpeed;

    public void InitializeBar(CharacterClassType classType, float currentHealth, float maxHealth)
    {
        SetBarClassColor(classType);
        SetNewMana(currentHealth, maxHealth, true);
    }
    public void SetNewMana(float currentHealth, float maxHealth, bool updateInstant = false)
    {
        _currentMana = currentHealth;
        _maxMana = maxHealth;
        UpdateBar(updateInstant);
    }

    private void SetBarClassColor(CharacterClassType classType)
    {
        _manabarImage.color = ClassAppearanceData.Instance().GetColor(classType);
    }
    private void UpdateBar(bool updateInstant = false)
    {
        var percentage = _currentMana / _maxMana;

        if (!updateInstant)
            StartCoroutine(SmoothChangeMana(percentage));
        else
            SetManaInstant(percentage);
    }
    private void SetManaInstant(float percentage)
    {
        _manabarImage.fillAmount = percentage;
    }
    private IEnumerator SmoothChangeMana(float manaPercentage)
    {
        float preChangePercentage = _manabarImage.fillAmount;
        float elapsed = 0f;
        while (elapsed < _updateManaSpeed)
        {
            elapsed += Time.deltaTime;
            _manabarImage.fillAmount = Mathf.Lerp(preChangePercentage, manaPercentage, elapsed / _updateManaSpeed);
            yield return null;
        }

        _manabarImage.fillAmount = manaPercentage;
    }
}
