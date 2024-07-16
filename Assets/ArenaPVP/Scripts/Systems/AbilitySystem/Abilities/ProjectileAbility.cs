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
        var bolt = Instantiate(ProjectilePrefab, gO.transform);
        bolt.transform.position = new Vector3(owner.transform.position.x, 0.5f, owner.transform.position.z);
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
        var args = new HealthChangedEventArgs() 
        { 
            Player = _target.GetComponent<Player>(), 
            Source = _owner.GetComponent<Player>(), 
            HealthChangeAmount = -DamageAmount, 
            HealthChangeType = HealthChangeType.Damage, 
            DamageType = DamageType 
        };

        GameEvents.OnPlayerHealthChanged.Invoke(args);

        foreach (var aura in ApplyAuras)
        {
            aura.Apply(_owner, _target);
        }

        Logger.Log(_target.gameObject.name + " hit, applyAura & damage");
    }
}
