using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    public SettingsScriptableObject Settings;

    public void ResetKeybinds()
    {
        var defaultSettings = Resources.Load<SettingsScriptableObject>("DefaultSettingsScriptableObject");
        Settings.Controls = defaultSettings.Controls;
        Resources.UnloadAsset(defaultSettings);
        Debug.Log("Reset Controls to default.");
    }
}
