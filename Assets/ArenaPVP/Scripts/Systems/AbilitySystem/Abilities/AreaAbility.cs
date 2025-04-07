using FishNet;
using GameKit.Dependencies.Utilities.ObjectPooling.Examples;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Logger =Assets.ArenaPVP.Scripts.Helpers.ArenaLogger;

[CreateAssetMenu(menuName = "Abilities/AreaAbility", fileName = "AreaAbility")]
public class AreaAbility : AbilityBase
{
    [SerializeField] private AreaSelectorBase _areaSelector;
    internal override void UseServer(Entity origin, Entity target = null)
    {
        if (InstanceFinder.IsServerStarted)
        {
            var targets = _areaSelector.GetTargetsInArea(origin);

            foreach (var tar in targets)
            {
                if (tar.Id == origin.Id)
                    continue;
                //TODO: check if target is enemy or friendly and check TargetingType to match that.
                if (IsLineOfSight(origin.transform, tar.transform))
                {
                    ApplyEffectsServer(origin, tar);
                }
            }
        }
    }
}
