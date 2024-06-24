using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ability
{
    public Champion Champion;
    public Sprite Icon;
    public string Name;
    public string Description;
    public int Id;

    public AbilityTarget[] AbilityTargets;
}
