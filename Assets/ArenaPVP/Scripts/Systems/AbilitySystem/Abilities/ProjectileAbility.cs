using GameKit.Dependencies.Utilities.ObjectPooling.Examples;
using UnityEngine;
using Logger = Assets.Scripts.Helpers.Logger;

[CreateAssetMenu(menuName = "Abilities/ProjectileAbility", fileName = "ProjectileAbility")]
public class ProjectileAbility : AbilityBase
{
    public float DamageAmount;
    public GameObject ProjectilePrefab;
    public AuraBase[] ApplyAuras;

    private Player _target;
    private Player _owner;

    public DamageType DamageType;

    private GameObject _projectile;

    protected override void Use(Player owner, Player target)
    {
        _target = target;
        _owner = owner;
        InstantiateProjectile(owner);
    }
    private void InstantiateProjectile(Player owner)
    {
        var gO = new GameObject();
        gO.SetActive(false);
        _projectile = Instantiate(ProjectilePrefab, gO.transform);
        _projectile.transform.position = new Vector3(owner.transform.position.x, 0.5f, owner.transform.position.z);
        var projectileScript = _projectile.GetComponent<ProjectileMove>();
        projectileScript.Origin = owner;
        projectileScript.Target = _target;
        projectileScript.OnCollision += OnCollision;
        _projectile.transform.SetParent(null);
        _projectile.SetActive(true);
        Destroy(gO);
    }
    public void OnCollision()
    {
        var projectileScript = _projectile.GetComponent<ProjectileMove>();
        projectileScript.OnCollision -= OnCollision;

        var args = new HealthChangedEventArgs() 
        { 
            Player = _target.GetComponent<Player>(), 
            Source = _owner.GetComponent<Player>(), 
            HealthChangeAmount = -DamageAmount, 
            HealthChangeType = HealthChangeType.Damage, 
            DamageType = DamageType 
        };

        foreach (var aura in ApplyAuras)
        {
            aura.Apply(_owner, _target);
        }

        GameEvents.OnPlayerHealthChanged.Invoke(args);


    }
}
