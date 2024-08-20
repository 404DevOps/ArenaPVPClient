using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.ArenaPVP.Scripts.Systems.AbilitySystem
{
    [CreateAssetMenu(fileName = "HealthCondition", menuName = "Conditions/HealthCondition")]
    public class HealthCondition : ConditionBase
    {
        AuraTargetType targetType;
        public int HealthPercentage;
        public bool TrueIfBelow;

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

            var healthComponent = playerToInspect.GetComponent<PlayerHealth>();
            var healthPercentage = (healthComponent.CurrentHealth.Value / healthComponent.MaxHealth.Value) * 100;

            if (HealthPercentage <= healthPercentage)
            {
                return TrueIfBelow;
            }
            else 
            {
                return !TrueIfBelow;
            }
        }
    }
}
