using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public List<CharacterAbility> AllAbilities;
    // Start is called before the first frame update
    void Start()
    {
        AllAbilities = new List<CharacterAbility>();
        var abilities = Resources.LoadAll("ScriptableObjects/Abilities/", typeof(CharacterAbility));
        foreach (var ability in abilities) 
        {
            AllAbilities.Add((CharacterAbility)ability);
        }
    }
}
