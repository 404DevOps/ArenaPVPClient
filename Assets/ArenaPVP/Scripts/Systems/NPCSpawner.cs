using FishNet.Object;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject TargetDummyPrefab;
    [SerializeField] private List<Vector3> SpawnPositions;
    public override void OnStartServer()
    {
        base.OnStartServer();
        SpawnDummies();
    }

    void SpawnDummies()
    {
        foreach (var pos in SpawnPositions)
        {
            var dummy = Instantiate(TargetDummyPrefab, pos, Quaternion.identity);
            base.Spawn(dummy);
        }
    }
}
