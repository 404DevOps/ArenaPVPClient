using Assets.ArenaPVP.Scripts.Enums;
using Assets.ArenaPVP.Scripts.Helpers;
using Assets.ArenaPVP.Scripts.Models.Enums;
using FishNet;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NewAura", menuName = "Auras/New Aura")]
public class AuraBase : ScriptableObject
{
    public int Id;
    public string Name;
    public string Description;
    public float Duration;
    public TargetType AuraTarget;
    public AuraType AuraType;
    public ApplyTiming AuraApplyTiming;
    public Sprite Icon;
    public List<StatModifier> StatModifiers;
    public bool isDebuff;
    private int _auraId;

    public int MaxStacks;


    public void Apply(Entity owner, Entity target) 
    {
        if (InstanceFinder.IsServerStarted)
        {
            Entity applyTo;
            switch (AuraTarget)
            {
                case TargetType.Player:
                    applyTo = owner;
                    break;
                case TargetType.Target:
                    applyTo = target;
                    break;
                default: throw new ArgumentOutOfRangeException();
            }

            var stackAmountBeforeApplyAura = AuraManager.Instance.GetStackAmount(applyTo.Id, Id);

            _auraId = AuraManager.Instance.AddAura(owner, applyTo, this); //will stack if possible

            if (stackAmountBeforeApplyAura < MaxStacks)
            {
                foreach (var statMod in StatModifiers)
                {
                    statMod.SourceAuraId = _auraId;
                    applyTo.GetComponent<StatsMediator>().AddModifierServer(statMod);
                }
            }
        }
    }
    public void Fade(Entity player, int auraId) 
    {
        if (InstanceFinder.IsServerStarted)
        {
            player.GetComponent<StatsMediator>().RemoveModifiersServer(auraId);
            ArenaLogger.Log($"Applied {Name} Aura to Player with ID {player.Id}");
        }
    }
}
