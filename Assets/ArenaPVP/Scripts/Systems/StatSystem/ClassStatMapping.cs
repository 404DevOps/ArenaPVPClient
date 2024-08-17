using Assets.ArenaPVP.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Class Stat Mapping", fileName = "ClassStatMapping")]
public class ClassStatMapping : ScriptableObject
{
    [SerializeField] private List<StatClassPair> classStatMapping;

    private static ClassStatMapping _instance;

    public static ClassStatMapping Instance()
    {
        if (_instance == null)
        {
            _instance = Resources.Load("ScriptableObjects/BaseStats/" + typeof(ClassStatMapping).Name) as ClassStatMapping;
        }
        return _instance;
    }

    public BaseStats GetBaseStats(CharacterClassType classType)
    {
        var pair = classStatMapping.FirstOrDefault(s => s.ClassType == classType);
        if (pair != default)
        {
            return pair.Stats;
        }

        throw new ArgumentOutOfRangeException($"Didnt find BaseStats for {classType.ToString()}");
    }
}

[Serializable]
public class StatClassPair 
{
    public CharacterClassType ClassType;
    public BaseStats Stats;
}
