using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlsMenuUIScript : MonoBehaviour
{
    public Button closeButton;
    public Button resetDefaultsButton;

    PlayerConfiguration _settings;

    public GameObject AdjustRebindInputPrefab;
    public Transform ActionSlotFrame;

    private void OnEnable()
    {
        UIEvents.OnNewKeyBindInputSelected.AddListener(OnNewBoxSelected);
    }
    private void OnDisable()
    {
        UIEvents.OnNewKeyBindInputSelected.RemoveListener(OnNewBoxSelected);
    }
    // Start is called before the first frame update
    void Start()
    {
        closeButton.onClick.AddListener(CloseMenu);
        resetDefaultsButton.onClick.AddListener(ResetDefaultControls);
        _settings = FindObjectOfType<PlayerConfiguration>();
        InitializeActionSlotFrame();
    }

    private void InitializeActionSlotFrame()
    {
        int i = 0;
        foreach (var action in _settings.Settings.Controls.AbilityKeybinds)
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
        UIEvents.OnCloseSubMenu.Invoke();
    }
    public void ResetDefaultControls()
    {
        _settings.ResetKeybinds();
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
