using Assets.ArenaPVP.Scripts.Models.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Projectiles/ProjectileCollection", fileName = "ProjectileCollection")]
public class ProjectileCollection : ScriptableObject
{
    public List<ProjectileEntry> Collection = new();
}
[Serializable]
public class ProjectileEntry
{
    public ProjectileIdentifier Identifier;
    public GameObject ProjectilePrefab;
}
