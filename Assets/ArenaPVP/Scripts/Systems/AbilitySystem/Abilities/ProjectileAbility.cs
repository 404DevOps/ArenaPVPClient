using UnityEngine;
using Logger = Assets.Scripts.Helpers.Logger;

[CreateAssetMenu(menuName = "Abilities/ProjectileAbility", fileName = "ProjectileAbility")]
public class ProjectileAbility : AbilityBase
{
    public float damageAmount;
    public GameObject projectilePrefab;
    public AuraBase[] ApplyAuras;

    private Transform _target;
    private Transform _owner;

    protected override void Use(Transform owner, Transform target)
    {
        _target = target;
        _owner = owner;
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
        var player = _target.GetComponent<Player>();
        GameEvents.OnPlayerHealthChanged.Invoke(player, -damageAmount);

        foreach (var aura in ApplyAuras)
        {
            aura.Apply(_owner, _target);
        }

        Logger.Log(_target.gameObject.name + " hit, applyAura & damage");
    } 
}
