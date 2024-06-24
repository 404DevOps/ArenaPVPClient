using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ControlsMenuUIScript : MonoBehaviour
{
    public Button closeButton;
    public Button saveButton;

    MenuHandlerUIScript menuHandler;
    SettingsScriptableObject settings;

    // Start is called before the first frame update
    void Start()
    {
        menuHandler = FindObjectOfType<MenuHandlerUIScript>();
        closeButton.onClick.AddListener(CloseMenu);
        saveButton.onClick.AddListener(SaveControls);
        settings = FindObjectOfType<PlayerSettings>().Settings;

    }

    public void CloseMenu()
    {
        menuHandler.CloseMenu();
    }
    public void SaveControls()
    {
        menuHandler.CloseMenu();
    }
}
