using FishNet.Connection;
using FishNet.Object;
using System.Collections;
using UnityEngine;

public class AbilityExecutor : NetworkBehaviour
{
    private NetworkObject _nob;

    private static AbilityExecutor _instance;
    public static AbilityExecutor Instance => _instance;

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        _nob = GetComponent<NetworkObject>();
    }

    /// <summary>
    /// Tries executing the Abiltiy with given Arguments while checking Conditions before sending request to Server.
    /// </summary>
    /// <param name="args">Ability, Origin, Target</param>
    public void TryUseAbilityClient(UseAbilityArgs args)
    {
        var ability = AbilityStorage.GetAbility(args.AbilityId);
        if (ability.CanBeUsed(args.Origin, args.Target))
        {
            TryUseAbilityServer(args);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void TryUseAbilityServer(UseAbilityArgs args, NetworkConnection sender = null)
    {
        var ability = AbilityStorage.GetAbility(args.AbilityId);
        args.CastId = TimeManager.LocalTick;
        if (ability.CanBeUsed(args.Origin, args.Target))
        {
            GCDManager.Instance.AddOrUpdate(args.Origin.Id);

            if (ability.AbilityInfo.CastTime > 0)
            {
                //GameEvents.OnCastInterrupted.AddListener(WasInterrupted);
                CastManager.Instance.AddOrUpdate(args.Origin.Id, new AbilityCastInfo(args.Origin.Id, ability.Id, ability.AbilityInfo.CastTime, args.CastId));
                CastManager.Instance.StartCastCoroutine(CastTimer(args, sender));
            }
            else
            {
                UseAbilityWithCooldownAndResource(args, sender);
                
                foreach (var conn in _nob.Observers)
                {
                    if (conn != sender)
                        OnUseAbilityTargetRpc(conn, args);
                }
            }
        }
    }

    [Server]
    void UseAbilityWithCooldownAndResource(UseAbilityArgs args, NetworkConnection sender = null) 
    {
        var ability = AbilityStorage.GetAbility(args.AbilityId);

        if (ability.AbilityInfo.Cooldown > 0)
        {
            CooldownManager.Instance.AddOrUpdate(new AbilityCooldownInfo(args.Origin.Id, ability.Id, ability.AbilityInfo.Cooldown));
            foreach (var conn in _nob.Observers)
            {
                OnCooldownStartedTargetRpc(conn, args);
            }
        }
        if (ability.AbilityInfo.ResourceCost > 0)
            args.Origin.GetComponent<PlayerResource>().UpdateResourceServer(new ResourceChangedEventArgs() { Player = args.Origin, ResourceChangeAmount = -ability.AbilityInfo.ResourceCost });

        ability.UseServer(args.Origin, args.Target);
    }

    [Server]
    IEnumerator CastTimer(UseAbilityArgs args, NetworkConnection sender)
    {
        var ability = AbilityStorage.GetAbility(args.AbilityId);
        //wait till casttimer hit 0
        yield return new WaitUntil(() => CastManager.Instance.GetRemainingCastTime(args.Origin.Id) <= 0 || CastManager.Instance.GetInterrupted(args.Origin.Id) == true);

        var castInfo = CastManager.Instance.GetCastInfo(args.Origin.Id);
        //enemy interrupt
        if (CastManager.Instance.GetInterrupted(args.Origin.Id) == true)
        {
            SendClientInterrupt(castInfo, sender);
        }
        //moved to invalid position during cast results in interrupt aswell
        else if(ability.AbilityInfo.AbilityType != AbilityType.AreaOfEffect && (!ability.IsInFront(args.Origin.transform, args.Target.transform) || !ability.IsLineOfSight(args.Origin.transform, args.Target.transform)))
        {
            SendClientInterrupt(castInfo, sender);
        }
        else
        {
            UseAbilityWithCooldownAndResource(args, sender);

            foreach (var conn in _nob.Observers)
            {
                OnCastCompletedTargetRpc(conn, args);
                if (conn != sender)
                    OnUseAbilityTargetRpc(conn, args);
            }
        }

        CastManager.Instance.Remove(args.Origin.Id);
    }

    [Server]
    private void SendClientInterrupt(AbilityCastInfo args, NetworkConnection sender)
    {
        foreach (var conn in _nob.Observers)
        {
            OnCastInterruptedTargetRpc(conn, args, conn == sender);
        }
    }

    [TargetRpc]
    public void OnCastCompletedTargetRpc(NetworkConnection conn, UseAbilityArgs args)
    {
        ClientEvents.OnCastCompleted.Invoke(new CastEventArgs(args.Origin.Id, args.AbilityId, args.CastId));
    }
    [TargetRpc]
    public void OnCooldownStartedTargetRpc(NetworkConnection conn, UseAbilityArgs args)
    {
        ClientEvents.OnCooldownStarted.Invoke(args.Origin.Id, args.AbilityId);
    }

    [TargetRpc]
    public void OnCastInterruptedTargetRpc(NetworkConnection conn, AbilityCastInfo args, bool isSender = false)
    {
        ClientEvents.OnCastInterrupted.Invoke(args);
        if (isSender)
            UIEvents.OnShowInformationPopup.Invoke("Interrupted");
    }

    [TargetRpc]
    public void OnUseAbilityTargetRpc(NetworkConnection conn, UseAbilityArgs args)
    {
        var ability = AbilityStorage.GetAbility(args.AbilityId);
        ability.UseClient(args.Origin, args.Target);
    }
}

public struct UseAbilityArgs 
{
    public UseAbilityArgs(int abilityId, Player origin, Player target, uint castId) 
    {
        AbilityId = abilityId;
        Origin = origin;
        Target = target;
        CastId = castId;
    }
    public int AbilityId;
    public Player Origin;
    public Player Target;
    public uint CastId;
}
