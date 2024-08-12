using Assets.ArenaPVP.Scripts.Models.Enums;
using Assets.Scripts.Enums;
using FishNet;
using MonoFN.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.GridLayoutGroup;
using Logger = Assets.Scripts.Helpers.Logger;

[Serializable]
[CreateAssetMenu(fileName = "NewAura", menuName = "Auras/New Aura")]
public class AuraBase : ScriptableObject
{
    public int Id;
    public string Name;
    public string Description;
    public float Duration;
    public AuraTargetType AuraTarget;
    public AuraType AuraType;
    public AuraApplyTiming AuraApplyTiming;
    public Sprite Icon;
    public List<StatModifier> StatModifiers;
    public bool isDebuff;
    private int _auraId;
    private Player _applyTo;

    public int MaxStacks;



    public void Apply(Player owner, Player target) 
    {
        if (InstanceFinder.IsServerStarted)
        {
            switch (AuraTarget)
            {
                case AuraTargetType.Player:
                    _applyTo = owner;
                    break;
                case AuraTargetType.Target:
                    _applyTo = target;
                    break;
                default: throw new ArgumentOutOfRangeException();
            }

            var stackAmountBeforeApplyAura = AuraManager.Instance.GetStackAmount(_applyTo.Id, Id);

            _auraId = AuraManager.Instance.AddAura(owner, _applyTo, this); //will stack if possible

            if (stackAmountBeforeApplyAura < MaxStacks)
            {
                foreach (var statMod in StatModifiers)
                {
                    statMod.SourceAuraId = _auraId;
                    _applyTo.Stats.Mediator.AddModifier(statMod);
                }
            }
        }
    }
    public void Fade() 
    {
        _applyTo.Stats.Mediator.RemoveAuraModifiers(_auraId);  
    }
}
public enum AuraTargetType
{
    Player, 
    Target
}