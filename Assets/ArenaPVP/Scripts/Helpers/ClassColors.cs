using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ClassVisuals")]
public class ClassAppearanceData : ScriptableObject
{

    private static ClassAppearanceData _instance;

    [SerializeField]
    private List<ClassIconPair> _classIcons = new List<ClassIconPair>();
    [SerializeField]
    private List<ClassColorPair> _classColors = new List<ClassColorPair>();

    public ClassAppearanceData Instance()
    {
        if (_instance == null)
        {
            _instance = Resources.Load(typeof(ClassAppearanceData).Name) as ClassAppearanceData;
        }
        return _instance;
    }

    public Color GetColor(CharacterClassType classType)
    {
        if (_classColors.Any(entry => entry.ClassType == classType))
        { 
            return _classColors.FirstOrDefault(entry => entry.ClassType == classType).Color;
        }

        throw new KeyNotFoundException(classType.ToString() + "does not exist in ClassColors.");
    }
    public Image GetIcon(CharacterClassType classType)
    {
        if (_classIcons.Any(entry => entry.ClassType == classType))
        {
            return _classIcons.FirstOrDefault(entry => entry.ClassType == classType).Icon;
        }
        throw new KeyNotFoundException(classType.ToString() + "does not exist in ClassIcons.");
    }

}

public class ClassColorPair
{ 
    public CharacterClassType ClassType;
    public Color Color;
}

public class ClassIconPair
{
    public CharacterClassType ClassType;
    public Image Icon;
}

//public static HashSet<CharacterClassType, Color> ClassColorDict = new Dictionary<CharacterClassType, Color>() 
//{
//    new KeyValuePair<CharacterClassType, Color>(CharacterClassType.Blademaster, Color.red),
//    new KeyValuePair<CharacterClassType, Color>(CharacterClassType.Spellslinger, Color.blue),
//    new KeyValuePair<CharacterClassType, Color>(CharacterClassType.Hawkeye, Color.green),
//}

