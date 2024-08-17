using UnityEngine;

namespace Assets.ArenaPVP.Scripts.Helpers
{
    public static class PositionHelper
    {
            public static bool IsInFront(Transform origin, Transform target, float searchRadius = 0f)
            {
                var directionToTarget = origin.position - target.transform.position;
                float angle = Vector3.Angle(origin.forward, directionToTarget);
                float distance = directionToTarget.magnitude;
                return searchRadius > 0 ? Mathf.Abs(angle) > 90 && distance < searchRadius : Mathf.Abs(angle) > 90;
            }
    }
}
