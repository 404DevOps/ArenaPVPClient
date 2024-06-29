using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuUIScript : MonoBehaviour
{
    public Button closeButton;
    public Button saveButton;

    MenuHandlerUIScript menuHandler;
    GeneralSettings settings;

    public Toggle LockActionBars;
    public Toggle ShowHealthAsPercentage;

    // Start is called before the first frame update
    void Start()
    {
        menuHandler = FindObjectOfType<MenuHandlerUIScript>();
        closeButton.onClick.AddListener(CloseMenu);
        saveButton.onClick.AddListener(SaveSettings);
        settings = FindObjectOfType<PlayerSettings>().Settings;
        InitializeSettingsInputs();
    }



    private void InitializeSettingsInputs()
    {
        LockActionBars.isOn = settings.LockActionBars;
        ShowHealthAsPercentage.isOn = settings.ShowHealthAsPercentage;
    }

    public void CloseMenu()
    {
        menuHandler.CloseMenu();
    }
    public void SaveSettings()
    {
        settings.LockActionBars = LockActionBars.isOn;
        settings.ShowHealthAsPercentage = ShowHealthAsPercentage.isOn;
        settings.SaveSettingsToFile();
        ReloadUI();
        menuHandler.CloseMenu();
    }

    private void ReloadUI()
    {
        var actionBarsController = FindObjectOfType<ActionBarUIHandler>();
        actionBarsController.RebuildActionBars();
    }
}
