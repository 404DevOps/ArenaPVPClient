using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.ArenaPVP.Scripts.Systems.AbilitySystem
{
    [CreateAssetMenu(fileName = "AuraCondition", menuName = "Conditions/AuraCondition")]
    public class AuraCondition : ConditionBase
    {
        public AuraTargetType targetType;
        public List<AuraBase> Auras;
        public bool TrueOnAuraMissing;
        public bool TrueOnAllMatching;

        public override bool IsTrue(Player origin, Player target)
        {
            Player playerToInspect;
            switch (targetType)
            {
                case AuraTargetType.Player: playerToInspect = origin;
                    break;
                case AuraTargetType.Target: playerToInspect = target;
                    break;
                default:
                    throw new Exception("AuraTarget Type not found.");
            }

            var playerAuras = AuraManager.Instance.GetAuraInfosForPlayer(playerToInspect.Id);
            if (playerAuras.Count == 0 && !TrueOnAuraMissing)
            {
                return false;
            }
            foreach (var aura in Auras)
            {
                //if only one needs to be on target direct return once match has been found.
                var auraFound = playerAuras.Any(a => a.AuraId == aura.Id);

                if (auraFound && !TrueOnAllMatching && !TrueOnAuraMissing) //one found, only one needed
                    return true;
                if (!auraFound && !TrueOnAllMatching && TrueOnAuraMissing) //one missing, only one needs be missing (will probybly never occur)
                    return true;
                if (auraFound && TrueOnAllMatching && TrueOnAuraMissing) //one found but all need be missing
                    return false;
                if (!auraFound && TrueOnAllMatching && !TrueOnAuraMissing) //one missing but all need be present
                    return false;
            }

            //safe to assume conditions are met since we would have returned if not.
            return true;
        }
    }
}
