using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Enums;
using TMPro;

public class ControlsMenuUIScript : MonoBehaviour
{
    public Button closeButton;
    public Button resetDefaultsButton;

    PlayerConfiguration settings;

    public GameObject AdjustRebindInputPrefab;
    public Transform ActionSlotFrame;

    private void OnEnable()
    {
        UIEvents.onNewKeyBindInputSelected.AddListener(OnNewBoxSelected);
    }
    private void OnDisable()
    {
        UIEvents.onNewKeyBindInputSelected.RemoveListener(OnNewBoxSelected);
    }
    // Start is called before the first frame update
    void Start()
    {
        closeButton.onClick.AddListener(CloseMenu);
        resetDefaultsButton.onClick.AddListener(ResetDefaultControls);
        settings = FindObjectOfType<PlayerConfiguration>();
        InitializeActionSlotFrame();
    }

    private void InitializeActionSlotFrame()
    {
        int i = 0;
        foreach (var action in settings.Settings.Controls.AbilityKeybinds)
        { 
            var gO = Instantiate(AdjustRebindInputPrefab, ActionSlotFrame);
            var inputs = gO.GetComponentsInChildren<KeybindInputHandler>();
            var label = gO.GetComponentInChildren<TextMeshProUGUI>();
            label.text = "Action Slot " + (i + 1).ToString();

            foreach (var input in inputs)
            {
                input.actionToRebind = i.ToString();
                input.isAbilityKeybind = true;
            }
            i++;
        }
    }

    public void CloseMenu()
    {
        UIEvents.onCloseSubMenu.Invoke();
    }
    public void ResetDefaultControls()
    {
        settings.ResetKeybinds();
        var inputHandlers = GetComponentsInChildren<KeybindInputHandler>();
        foreach (var inputHandler in inputHandlers)
        {
            inputHandler.SetKeyBindText();
        }
    }

    public void OnNewBoxSelected()
    {
        var inputHandlers = GetComponentsInChildren<KeybindInputHandler>();
        foreach (var inputHandler in inputHandlers.Where(ih => ih.IsInputListenerActive))
        {
            inputHandler.ResetBox();
        }
    }
}
