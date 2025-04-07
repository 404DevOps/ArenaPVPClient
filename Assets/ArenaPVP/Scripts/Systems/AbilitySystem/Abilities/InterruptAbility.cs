using Assets.ArenaPVP.Scripts.Models.Enums;
using Assets.ArenaPVP.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ArenaLogger =Assets.ArenaPVP.Scripts.Helpers.ArenaLogger;

[CreateAssetMenu(menuName = "Abilities/InterruptAbility", fileName = "InterrupAbility")]
public class InterruptAbility : AbilityBase
{
    internal override void UseServer(Entity owner, Entity target)
    {
        base.UseServer(owner, target);

        CastManager.Instance.InterruptPlayer(target.Id, InterruptType.Interrupt);
        ArenaLogger.Log($"InterruptAbility:UseServer called, target = {target.Id}");
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
