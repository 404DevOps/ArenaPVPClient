using Assets.Scripts.Enums;
using MonoFN.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.GridLayoutGroup;
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
    public List<StatModifier> StatModifiers;
    public bool isDebuff;
    private int _auraId;
    private Player _target;

    public void Apply(Player owner, Player target) 
    {
        _auraId = AuraManager.Instance.AddAura(owner, target, this);

        foreach (var statMod in StatModifiers)
        {
            statMod.SourceAuraId = _auraId;
            owner.Stats.Mediator.AddModifier(statMod);
        }
    }
    public void Fade() 
    {
        _target.Stats.Mediator.RemoveAuraModifiers(_auraId);  
    }
}
public enum ModifierType
{ 
    Multiply, Add, Subtract, Divide
}