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
    public Toggle ShowPlayerNameplate;

    // Start is called before the first frame update
    void Start()
    {
        menuHandler = FindObjectOfType<MenuHandlerUIScript>();
        closeButton.onClick.AddListener(CloseMenu);
        saveButton.onClick.AddListener(SaveSettings);
        settings = FindObjectOfType<PlayerConfiguration>().Settings;
        InitializeSettingsInputs();
    }

    private void InitializeSettingsInputs()
    {
        LockActionBars.isOn = settings.LockActionBars;
        ShowHealthAsPercentage.isOn = settings.ShowHealthAsPercentage;
        ShowPlayerNameplate.isOn = settings.ShowPlayerNameplate;
    }

    public void CloseMenu()
    {
        UIEvents.OnCloseSubMenu.Invoke();
    }
    public void SaveSettings()
    {
        settings.LockActionBars = LockActionBars.isOn;
        settings.ShowHealthAsPercentage = ShowHealthAsPercentage.isOn;
        settings.ShowPlayerNameplate = ShowPlayerNameplate.isOn;
        settings.SaveSettingsToFile();
        UIEvents.OnSettingsSaved.Invoke();
    }
}
