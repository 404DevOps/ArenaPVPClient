using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public static List<AbilityBase> AllAbilities;

    void Awake()
    {
        AllAbilities = new List<AbilityBase>();
        var abilities = Resources.LoadAll("ScriptableObjects/Abilities/", typeof(AbilityBase));
        foreach (var ability in abilities) 
        {
            AllAbilities.Add((AbilityBase)ability);
        }
    }
}
