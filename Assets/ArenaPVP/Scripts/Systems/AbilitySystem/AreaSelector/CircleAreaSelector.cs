using FishNet;
using System.Collections.Generic;
using UnityEngine;
using ArenaLogger =Assets.ArenaPVP.Scripts.Helpers.ArenaLogger;


[CreateAssetMenu(fileName = "CircleAreaSelector", menuName = "AreaSelector/CircleAreaSelector")]
public class CircleAreaSelector : AreaSelectorBase
{
    [SerializeField] private float _radius;
    private Entity _source;
    public override List<Entity> GetTargetsInArea(Entity source)
    {
        if (InstanceFinder.IsServerStarted)
        {
            _source = source;

            Collider[] colliders;
            var playersInArea = new List<Entity>();

            var layerMask = LayerMask.GetMask(new string[] { "Player" });
            colliders = Physics.OverlapSphere(source.transform.position, _radius, layerMask);
            foreach (Collider collider in colliders)
            {
                var player = collider.GetComponent<Entity>();
                if (player != null)
                {
                    playersInArea.Add(player);
                }
            }
            ArenaLogger.Log($"Found {playersInArea.Count} Targets in Area.");
            return playersInArea;
        }
        else 
        {
            throw new System.Exception("Cannot be executed on Client");
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawSphere(_source.transform.position, _radius);
    }
}
