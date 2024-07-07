using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[Serializable]
public class GeneralSettings
{
    public Controls Controls;
    public bool LockActionBars;
    public bool ShowHealthAsPercentage;
    public bool ShowPlayerNameplate;

    public void SetControls(Controls controls)
    {
        Controls.targetNext = controls.targetNext;
        Controls.jump = controls.jump;
        Controls.targetSelf = controls.targetSelf;
        Controls.forwards = controls.forwards;
        Controls.backwards = controls.backwards;
        Controls.strafeLeft = controls.strafeLeft;
        Controls.strafeRight = controls.strafeRight;
        Controls.AbilityKeybinds = new KeyBind[controls.AbilityKeybinds.Length];

        for (int i = 0; i < controls.AbilityKeybinds.Length; i++)
        {
            Controls.AbilityKeybinds[i] = new KeyBind() { primary = new KeyCode[] { controls.AbilityKeybinds[i].primary.FirstOrDefault() }, secondary = new KeyCode[] { controls.AbilityKeybinds[i].secondary.FirstOrDefault() } };
        }
    }

    public void SaveSettingsToFile()
    {
        var str = JsonUtility.ToJson(this);
        File.WriteAllText(PlayerConfiguration.settingsPath, str);
    }
}
