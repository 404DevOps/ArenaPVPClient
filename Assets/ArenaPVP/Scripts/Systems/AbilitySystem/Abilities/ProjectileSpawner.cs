using FishNet.Object;
using UnityEngine;
using Logger = Assets.Scripts.Helpers.Logger;

public class ProjectileSpawner : NetworkBehaviour
{
    private static ProjectileSpawner _instance;
    public static ProjectileSpawner Instance => _instance;

    private const float MAX_PASSED_TIME = 0.3f;

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);
    }

    public void ClientFireTargeted(FireTargetedProjectileArgs args)
    {
        SpawnProjectile(args, 0f);
        ServerFireTargeted(args, base.TimeManager.Tick);
    }

    [ServerRpc]
    private void ServerFireTargeted(FireTargetedProjectileArgs args, uint tick)
    {
        float passedTime = (float)base.TimeManager.TimePassed(tick, false);
        passedTime = Mathf.Min(MAX_PASSED_TIME / 2f, passedTime);
        SpawnProjectile(args, passedTime);

        ObserverFireTargeted(args, tick);
    }


    [ObserversRpc(ExcludeOwner = true)]
    private void ObserverFireTargeted(FireTargetedProjectileArgs args, uint tick)
    {
        Logger.Log("Instantiate Projectile Client");
        float passedTime = (float)base.TimeManager.TimePassed(tick, false);
        passedTime = Mathf.Min(MAX_PASSED_TIME, passedTime);

        SpawnProjectile(args, passedTime);
    }

    private void SpawnProjectile(FireTargetedProjectileArgs args, float passedTime)
    {
        var projectile = Instantiate(args.Prefab);
        projectile.transform.position = new Vector3(args.Origin.transform.position.x, args.Origin.transform.position.y, args.Origin.transform.position.z);
        var projectileScript = projectile.GetComponent<ProjectileTargeted>();
        projectileScript.Initialize(args.AbilityId, args.Origin, args.Target, passedTime);
        projectile.SetActive(true);
    }
}

public struct FireTargetedProjectileArgs
{
    public FireTargetedProjectileArgs(int abilityId, Player origin, Player target, GameObject prefab)
    {
        AbilityId = abilityId;
        Origin = origin;
        Target = target;
        Prefab = prefab;
    }

    public int AbilityId;
    public Player Origin;
    public Player Target;
    public GameObject Prefab;

}
