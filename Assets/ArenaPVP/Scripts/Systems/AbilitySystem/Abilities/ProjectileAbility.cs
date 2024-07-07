using Logger = Assets.Scripts.Helpers.Logger;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.NetworkInformation;
using static UnityEngine.UI.GridLayoutGroup;

[CreateAssetMenu(menuName = "Abilities/ProjectileAbility", fileName = "ProjectileAbility")]
public class ProjectileAbility : AbilityBase
{
    public float damageAmount;
    public GameObject projectilePrefab;
    public AuraBase[] ApplyAuras;
    private Transform _target; 

    protected override void Use(Transform owner, Transform target)
    {
        _target = target;
        InstantiateProjectile(owner);
    }
    private void InstantiateProjectile(Transform owner)
    {
        var gO = new GameObject();
        gO.SetActive(false);
        var bolt = Instantiate(projectilePrefab, gO.transform);
        bolt.transform.position = new Vector3(owner.position.x, 0.5f, owner.position.z);
        var projectileScript = bolt.GetComponent<ProjectileMove>();
        projectileScript.Origin = owner;
        projectileScript.Target = _target;
        projectileScript.OnCollision += OnCollision;
        bolt.transform.SetParent(null);
        bolt.SetActive(true);
        Destroy(gO);
    }
    public void OnCollision()
    {
        GameEvents.OnPlayerHealthChanged.Invoke(_target.GetInstanceID(), -damageAmount);
        Logger.Log(_target.gameObject.name + " hit, applyAura & damage");

    } 
}
