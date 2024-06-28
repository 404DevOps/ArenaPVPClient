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

    public void SetControls(Controls controls)
    {
        Controls.targetNext = controls.targetNext;
        Controls.jump = controls.jump;
        Controls.targetSelf = controls.targetSelf;
        Controls.forwards = controls.forwards;
        Controls.backwards = controls.backwards;
        Controls.strafeLeft = controls.strafeLeft;
        Controls.strafeRight = controls.strafeRight;

        for (int i = 0; i < Controls.Abilities.Length; i++)
        {
            Controls.Abilities[i] = new KeyBind() { primary = new KeyCode[] { controls.Abilities[i].primary.FirstOrDefault() }, secondary = new KeyCode[] { controls.Abilities[i].secondary.FirstOrDefault() } };
        }
    }

    public void SaveSettings()
    {
        var str = JsonUtility.ToJson(this);
        File.WriteAllText(PlayerSettings.settingsPath, str);
    }
}
