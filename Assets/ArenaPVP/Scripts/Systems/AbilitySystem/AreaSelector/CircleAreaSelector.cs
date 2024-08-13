using FishNet;
using System.Collections.Generic;
using UnityEngine;
using Logger = Assets.Scripts.Helpers.Logger;


[CreateAssetMenu(fileName = "CircleAreaSelector", menuName = "AreaSelector/CircleAreaSelector")]
public class CircleAreaSelector : AreaSelectorBase
{
    [SerializeField] private float _radius;
    private Player _source;
    public override List<Player> GetTargetsInArea(Player source)
    {
        if (InstanceFinder.IsServerStarted)
        {
            _source = source;

            Collider[] colliders;
            var playersInArea = new List<Player>();

            var layerMask = LayerMask.GetMask(new string[] { "Player" });
            colliders = Physics.OverlapSphere(source.transform.position, _radius, layerMask);
            foreach (Collider collider in colliders)
            {
                var player = collider.GetComponent<Player>();
                if (player != null)
                {
                    playersInArea.Add(player);
                }
            }
            Logger.Log($"Found {playersInArea.Count} Targets in Area.");
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
