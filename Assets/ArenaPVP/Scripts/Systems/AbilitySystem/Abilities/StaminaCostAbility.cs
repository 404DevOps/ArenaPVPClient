using Assets.ArenaPVP.Scripts.Models.Enums;
using Assets.ArenaPVP.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Assets.ArenaPVP.Scripts.Helpers;
using FishNet;

[CreateAssetMenu(menuName = "Abilities/InstantAbility", fileName = "InstantAbility")]
public class StaminaCostAbility : AbilityBase
{
    public float StaminaCost;
    public override bool CanBeUsed(Entity owner, Entity target = null)
    {
        bool canbeUse = true;

        if (!base.IsCooldownReady(owner.Id))
        {
            if (InstanceFinder.IsClientStarted)
                UIEvents.OnShowInformationPopup.Invoke("This ability is not ready yet");

            return false;
        }
        if (!HasEnoughStamina(owner))
        {
            if (InstanceFinder.IsClientStarted)
            {
                var ressourceName = AppearanceData.Instance().GetRessourceDescriptor(owner.ClassType);
                UIEvents.OnShowInformationPopup.Invoke($"Not enough Stamina");
            }

            return false;
        }
        if (!AreConditionsMet(owner, target))
        {
            if (InstanceFinder.IsClientStarted)
                UIEvents.OnShowInformationPopup.Invoke("Conditions not met.");

            return false;
        }

        return canbeUse;
    }
    protected bool HasEnoughStamina(Entity owner)
    {
        if (owner.Stamina.CurrentStamina.Value >= this.StaminaCost)
            return true;

        return false;
    }
    internal override void UseServer(Entity owner, Entity target)
    {
        base.UseServer(owner, target);
        ApplyEffectsServer(owner, target);
    }
    internal override void UseClient(Entity owner, Entity target)
    {
        base.UseClient(owner, target);
        return;
    }
    internal override void ApplyEffectsServer(Entity owner, Entity target)
    {
        base.ApplyEffectsServer(owner, target);
        return;
    }
}
