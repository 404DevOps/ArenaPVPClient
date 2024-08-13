using FishNet;
using FishNet.CodeGenerating;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;
using Logger = Assets.Scripts.Helpers.Logger;

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
        if (ability.CanBeUsed(args.Origin, args.Target))
        {
            if (ability.AbilityInfo.CastTime > 0)
            {
                //GameEvents.OnCastInterrupted.AddListener(WasInterrupted);
                CastManager.Instance.AddOrUpdate(args.Origin.Id, new AbilityCastInfo(ability.Id, ability.AbilityInfo.CastTime));
                CastManager.Instance.StartCastCoroutine(CastTimer(args, sender));
            }
            else
            {
                UseAbilityWithCooldownAndResource(args, sender);
                
                foreach (var conn in _nob.Observers)
                {
                    OnUseAbilityTargetRpc(conn, args);
                }
            }
        }
    }

    [Server]
    void UseAbilityWithCooldownAndResource(UseAbilityArgs args, NetworkConnection sender = null) 
    {
        var ability = AbilityStorage.GetAbility(args.AbilityId);

        if(ability.AbilityInfo.Cooldown > 0)
            CooldownManager.Instance.AddOrUpdate(new AbilityCooldownInfo(args.Origin.Id, ability.Id, ability.AbilityInfo.Cooldown));
        if (ability.AbilityInfo.ResourceCost > 0)
            args.Origin.GetComponent<PlayerResource>().UpdateResourceServer(new ResourceChangedEventArgs() { Player = args.Origin, ResourceChangeAmount = -ability.AbilityInfo.ResourceCost });

        ability.UseServer(args.Origin, args.Target);
    }

    [Server]
    IEnumerator CastTimer(UseAbilityArgs args, NetworkConnection sender)
    {
        var wasInterrupted = false;
        var ability = AbilityStorage.GetAbility(args.AbilityId);

        if (wasInterrupted)
        {
            foreach (var conn in _nob.Observers)
            {
                OnCastInterruptedTargetRpc(conn, args);
            }
            Logger.Log($"Ability {ability.AbilityInfo.Name} was Interrupted while casting.");
            yield break;
        }

        //wait till casttimer hit 0
        yield return new WaitUntil(() => CastManager.Instance.GetRemainingCastTime(args.Origin.Id, args.AbilityId) <= 0);
        CastManager.Instance.Remove(args.Origin.Id);

        //check line of sight again for non-area effects, automatically interrupt if LoS is not given anymore
        if (ability.AbilityInfo.AbilityType != AbilityType.AreaOfEffect && (!ability.IsInFront(args.Origin.transform, args.Target.transform) || !ability.IsLineOfSight(args.Origin.transform, args.Target.transform)))
        {
            wasInterrupted = true;
        }
        if (!wasInterrupted)
        {
            UseAbilityWithCooldownAndResource(args, sender);

            foreach (var conn in _nob.Observers)
            {
                OnCastCompletedTargetRpc(conn, args);
                OnUseAbilityTargetRpc(conn, args);
            }
        }
    }

    [TargetRpc]
    public void OnCastCompletedTargetRpc(NetworkConnection conn, UseAbilityArgs args)
    {
        GameEvents.OnCastCompleted.Invoke(args.Origin.Id);
    }

    [TargetRpc]
    public void OnCastInterruptedTargetRpc(NetworkConnection conn, UseAbilityArgs args)
    {
        GameEvents.OnCastInterrupted.Invoke(args.Origin.Id);
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
    public UseAbilityArgs(int abilityId, Player origin, Player target) 
    {
        AbilityId = abilityId;
        Origin = origin;
        Target = target;
    }
    public int AbilityId;
    public Player Origin;
    public Player Target;
}
