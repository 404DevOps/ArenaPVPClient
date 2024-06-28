using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SettingsScriptableObject", menuName = "ScriptableObjects/Settings")]
public class SettingsScriptableObject : ScriptableObject
{
    public GeneralSettings GeneralSettings;    
}
