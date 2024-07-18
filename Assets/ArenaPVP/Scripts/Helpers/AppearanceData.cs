using Assets.ArenaPVP.Scripts.Models.Enums;
using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ClassAppearanceData", menuName = "Appearance/ClassAppearanceData")]
public class AppearanceData : ScriptableObject
{

    private static AppearanceData _instance;

    [SerializeField]
    private List<ClassIconPair> _classIcons = new List<ClassIconPair>();
    [SerializeField]
    private List<ClassColorPair> _classColors = new List<ClassColorPair>();
    [SerializeField]
    private List<CombatLogColorPair> _combatLogColors= new List<CombatLogColorPair>();
    [SerializeField]
    private List<FloatingTextColorPair> _floatingTextColors = new List<FloatingTextColorPair>();


    public static AppearanceData Instance()
    {
        if (_instance == null)
        {
            _instance = Resources.Load("ScriptableObjects/Settings/" + typeof(AppearanceData).Name) as AppearanceData;
        }
        return _instance;
    }

    public Color GetClassColor(CharacterClassType classType)
    {
        if (_classColors.Any(entry => entry.ClassType == classType))
        { 
            return _classColors.FirstOrDefault(entry => entry.ClassType == classType).Color;
        }

        throw new KeyNotFoundException(classType.ToString() + "does not exist in ClassColors.");
    }
    public Sprite GetClassIcon(CharacterClassType classType)
    {
        if (_classIcons.Any(entry => entry.ClassType == classType))
        {
            return _classIcons.FirstOrDefault(entry => entry.ClassType == classType).Icon;
        }
        throw new KeyNotFoundException(classType.ToString() + "does not exist in ClassIcons.");
    }
    public Color GetCombatLogColor(CombatLogType logType)
    {
        if (_combatLogColors.Any(entry => entry.LogType == logType))
        {
            return _combatLogColors.FirstOrDefault(entry => entry.LogType == logType).Color;
        }

        throw new KeyNotFoundException(logType.ToString() + "does not exist in CombatLogColors.");
    }
    public Color GetFloatingTextColor(HealthChangeType healthChangedType)
    {
        if (_floatingTextColors.Any(entry => entry.HealthChangedType == healthChangedType))
        {
            return _floatingTextColors.FirstOrDefault(entry => entry.HealthChangedType == healthChangedType).Color;
        }

        throw new KeyNotFoundException(healthChangedType.ToString() + "does not exist in CombatLogColors.");
    }

}


[Serializable]
public class ClassColorPair
{ 
    public CharacterClassType ClassType;
    public Color Color;
}

[Serializable]
public class ClassIconPair
{
    public CharacterClassType ClassType;
    public Sprite Icon;
}

[Serializable]
public class CombatLogColorPair
{
    public CombatLogType LogType;
    public Color Color;
}
[Serializable]
public class FloatingTextColorPair
{
    public HealthChangeType HealthChangedType;
    public Color Color;
}


