using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class PlayerConfiguration : MonoBehaviour
{
    public GeneralSettings Settings;
    public static string settingsPath;


    private static PlayerConfiguration _instance;
    public static PlayerConfiguration Instance => _instance;

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);

        UIEvents.OnSettingsSaved.AddListener(LoadSettings);
        settingsPath = Application.persistentDataPath + "/GeneralSettings.json";
        LoadSettings();
    }
    public void OnDisable()
    {
        UIEvents.OnSettingsSaved.RemoveListener(LoadSettings);
    }

    public void ResetKeybinds()
    {
        File.Delete(settingsPath);
        LoadSettings();
    }

    public void LoadSettings()
    {
        if (!File.Exists(settingsPath))
        {
            var defaultSettings = Resources.Load<SettingsScriptableObject>("ScriptableObjects/Settings/DefaultSettingsScriptableObject");
            Settings.SetControls(defaultSettings.GeneralSettings.Controls);
            Settings.SaveSettingsToFile();
            //Resources.UnloadAsset(defaultSettings);
        }
        else
        {
            Settings = JsonUtility.FromJson<GeneralSettings>(File.ReadAllText(settingsPath));
        }
        UIEvents.OnSettingsLoaded.Invoke();
    }
}
