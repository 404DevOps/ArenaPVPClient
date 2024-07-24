using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InformationPopupUIHandler : MonoBehaviour
{
    [SerializeField] Transform _parent;
    [SerializeField] GameObject _popupTextPrefab;
    [SerializeField] int _maxTextCount;


    private void OnEnable()
    {
        UIEvents.OnShowInformationPopup.AddListener(InstantiateText);
        ClearTexts();
    }
    private void OnDisable()
    {
        UIEvents.OnShowInformationPopup.RemoveListener(InstantiateText);
    }

    private void InstantiateText(string text) 
    {
        var gO = new GameObject();
        gO.SetActive(false);
        var textGo = Instantiate(_popupTextPrefab, gO.transform);
        var component = textGo.GetComponent<InformationPopupText>();
        component.Text = text;
        textGo.transform.SetParent(_parent, false);
        DestroyImmediate(gO);
    }

    private void ClearTexts()
    { 
        for (int i = 0; i < _parent.childCount; i++)
        {
            Destroy(_parent.GetChild(i).gameObject);
        }
    }

    public void Update()
    {
        var texts = GetComponentsInChildren<InformationPopupText>().ToList();
        if (texts.Count > _maxTextCount)
        {
            texts = texts.OrderByDescending(t => t.Timer).ToList();
            int itemsToDelete = texts.Count - _maxTextCount;

            for (int i = itemsToDelete; i > 0; i--)
            {
                DestroyImmediate(texts[0].gameObject);
            }
        }
    }
}
