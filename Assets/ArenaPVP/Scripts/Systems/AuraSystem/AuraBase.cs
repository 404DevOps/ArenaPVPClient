using Assets.Scripts.Enums;
using MonoFN.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logger = Assets.Scripts.Helpers.Logger;

[Serializable]
[CreateAssetMenu(fileName = "NewAura", menuName = "Auras/New Aura")]
public class AuraBase : ScriptableObject
{
    public string Name;
    public string Description;
    public float Duration;
    public Sprite Icon;
    public AuraType AuraType;

    public ModifierType ModifierType;
    public float ModifierValue;
    public bool isDebuff;

    public void Apply(Player owner, Player target) 
    {
        AuraManager.Instance.AddAura(owner.Id, target.Id, this);
        Logger.Log($"Aura {Name} has been applied to Target {target.Name}");
    }
    public void Fade() 
    {
        Logger.Log($"Aura {Name} has faded.");
    }
}
public enum ModifierType
{ 
    Multiply, Add, Subtract, Divide
}