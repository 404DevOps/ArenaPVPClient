using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public static List<CharacterAbility> AllAbilities;

    void Awake()
    {
        AllAbilities = new List<CharacterAbility>();
        var abilities = Resources.LoadAll("ScriptableObjects/Abilities/", typeof(CharacterAbility));
        foreach (var ability in abilities) 
        {
            AllAbilities.Add((CharacterAbility)ability);
        }
    }
}
