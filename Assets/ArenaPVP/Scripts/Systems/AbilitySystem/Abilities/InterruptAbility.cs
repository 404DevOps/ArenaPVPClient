using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Logger = Assets.Scripts.Helpers.Logger;

[CreateAssetMenu(menuName = "Abilities/InterruptAbility", fileName = "InterrupAbility")]
public class InnterruptAbility : AbilityBase
{
    internal override void UseServer(Player owner, Player target)
    {
        base.UseServer(owner, target);

        CastManager.Instance.InterruptPlayer(target.Id);
        Logger.Log($"InterruptAbility:UseServer called, target = {target.Id}");
        ApplyEffectsServer(owner, target);
    }
    internal override void UseClient(Player owner, Player target)
    {
        base.UseClient(owner, target);
        return;
    }
    internal override void ApplyEffectsServer(Player owner, Player target)
    {
        base.ApplyEffectsServer(owner, target);
        return;
    }
}
