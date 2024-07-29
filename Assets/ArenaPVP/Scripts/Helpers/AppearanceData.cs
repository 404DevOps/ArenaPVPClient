using Assets.ArenaPVP.Scripts.Models.Enums;
using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ClassAppearanceData", menuName = "Appearance/ClassAppearanceData")]
public class AppearanceData : ScriptableObject
{
    private static AppearanceData _instance;

    #region Collections

    [SerializeField]
    private List<ClassIconPair> _classIcons = new List<ClassIconPair>();
    [SerializeField]
    private List<ClassColorPair> _classColors = new List<ClassColorPair>();
    [SerializeField]
    private List<CombatLogColorPair> _combatLogColors = new List<CombatLogColorPair>();
    [SerializeField]
    private List<FloatingTextColorPair> _floatingTextColors = new List<FloatingTextColorPair>();
    [SerializeField]
    private List<ClassRessourceDescPair> _classRessourceDescriptor = new List<ClassRessourceDescPair>();

    #endregion

    public static AppearanceData Instance()
    {
        if (_instance == null)
        {
            _instance = Resources.Load("ScriptableObjects/Settings/" + typeof(AppearanceData).Name) as AppearanceData;
        }
        return _instance;
    }
    public ClassColorPair GetClassColors(CharacterClassType classType)
    {
        if (_classColors.Any(entry => entry.ClassType == classType))
        {
            return _classColors.FirstOrDefault(entry => entry.ClassType == classType);
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
    public string GetRessourceDescriptor(CharacterClassType classType)
    {
        if (_classRessourceDescriptor.Any(entry => entry.ClassType == classType))
        {
            return _classRessourceDescriptor.FirstOrDefault(entry => entry.ClassType == classType).Descriptor;
        }

        throw new KeyNotFoundException(classType.ToString() + "does not exist in ClassRessourceDescriptors.");
    }
    public ResourceType GetRessourceType(CharacterClassType classType)
    {
        if (_classRessourceDescriptor.Any(entry => entry.ClassType == classType))
        {
            return _classRessourceDescriptor.FirstOrDefault(entry => entry.ClassType == classType).ResourceType;
        }

        throw new KeyNotFoundException(classType.ToString() + "does not exist in ClassRessourceDescriptors.");
    }
}

    #region Collection Classes

[Serializable]
public class ClassColorPair
{
    public CharacterClassType ClassType;
    public Color MainColor;
    public Color ResourceColor;
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
[Serializable]
public class ClassRessourceDescPair
{
    public CharacterClassType ClassType;
    public ResourceType ResourceType;
    public string Descriptor;
}

#endregion