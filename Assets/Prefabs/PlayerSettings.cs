using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    public GeneralSettings Settings;
    public static string settingsPath;

    public void Start()
    {
        settingsPath = Application.persistentDataPath + "/GeneralSettings.json";
        LoadSettings();
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
            Settings.SaveSettings();
            //Resources.UnloadAsset(defaultSettings);
        }
        else
        {
            Settings = JsonUtility.FromJson<GeneralSettings>(File.ReadAllText(settingsPath));
        }

        FindObjectOfType<CharacterController>().ReloadControlSettings();
        FindObjectOfType<ActionBarsUIController>().RebuildActionBars();
    }
}
