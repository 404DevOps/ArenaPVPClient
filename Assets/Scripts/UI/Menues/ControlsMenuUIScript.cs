using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ControlsMenuUIScript : MonoBehaviour
{
    public Button closeButton;
    public Button resetDefaultsButton;

    MenuHandlerUIScript menuHandler;
    PlayerSettings settings;

    // Start is called before the first frame update
    void Start()
    {
        menuHandler = FindObjectOfType<MenuHandlerUIScript>();
        closeButton.onClick.AddListener(CloseMenu);
        resetDefaultsButton.onClick.AddListener(ResetDefaultControls);
        settings = FindObjectOfType<PlayerSettings>();

    }

    public void CloseMenu()
    {
        menuHandler.CloseMenu();
    }
    public void ResetDefaultControls()
    {
        settings.ResetKeybinds();
        var inputHandlers = GetComponentsInChildren<InputBoxHandler>();
        foreach (var inputHandler in inputHandlers)
        {
            inputHandler.SetKeyBindText();
        }    
    }

    public void OnNewBoxSelected()
    {
        var inputHandlers = GetComponentsInChildren<InputBoxHandler>();
        foreach (var inputHandler in inputHandlers.Where(ih => ih.IsInputListenerActive))
        {
            inputHandler.ResetBox();
        }
    }
}
