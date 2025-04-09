using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Assets.ArenaPVP.Scripts.Helpers;
using System.Collections;
using static UnityEngine.UI.Image;

[CreateAssetMenu(menuName = "Abilities/DodgeAbility", fileName = "DodgeAbility")]
public class DodgeAbility : StaminaCostAbility
{
    internal override void UseServer(Entity origin, Entity target = null)
    {
        base.UseServer(origin, target);
        ApplyEffectsServer(origin, target);

        origin.Stamina.UpdateStaminaServer(new StaminaChangedEventArgs(origin, -StaminaCost));

        origin.SetDodging(true);
        origin.StartCoroutine(EndDodge(origin));
    }

    private IEnumerator EndDodge(Entity origin)
    {
        yield return WaitManager.Wait(0.7f);
        origin.SetDodging(false);
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