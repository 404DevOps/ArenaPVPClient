using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SettingsScriptableObject", menuName = "ScriptableObjects/Settings")]
public class SettingsScriptableObject : ScriptableObject
{
    public Controls Controls;
    public bool LockActionBars;
    public bool ShowHealthAsPercentage;
}
