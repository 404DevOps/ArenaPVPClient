using Assets.ArenaPVP.Scripts.Enums;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Resourcebar : MonoBehaviour
{
    private float _currentResource = 1;
    private float _maxResource = 1;

    [SerializeField] private Image _resourcebarImage;
    [SerializeField] private float _updateManaSpeed;

    public void InitializeBar(CharacterClassType classType, float currentResource, float maxResource)
    {
        SetBarClassColor(classType);
        SetNewValue(currentResource, maxResource, true);
    }
    public void SetNewValue(float currentResource, float maxResource, bool updateInstant = false)
    {
        _currentResource = currentResource;
        _maxResource = maxResource;
        UpdateBar(updateInstant);
    }

    private void SetBarClassColor(CharacterClassType classType)
    {
        _resourcebarImage.color = AppearanceData.Instance().GetClassColors(classType).ResourceColor;
    }
    private void UpdateBar(bool updateInstant = false)
    {
        var percentage = _currentResource / _maxResource;

        if (!updateInstant)
            StartCoroutine(SmoothChangeMana(percentage));
        else
            SetManaInstant(percentage);
    }
    private void SetManaInstant(float percentage)
    {
        _resourcebarImage.fillAmount = percentage;
    }
    private IEnumerator SmoothChangeMana(float manaPercentage)
    {
        float preChangePercentage = _resourcebarImage.fillAmount;
        float elapsed = 0f;
        while (elapsed < _updateManaSpeed)
        {
            elapsed += Time.deltaTime;
            _resourcebarImage.fillAmount = Mathf.Lerp(preChangePercentage, manaPercentage, elapsed / _updateManaSpeed);
            yield return null;
        }

        _resourcebarImage.fillAmount = manaPercentage;
    }
}
