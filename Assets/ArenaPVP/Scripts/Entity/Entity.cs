using Assets.ArenaPVP.Scripts.Enums;
using FishNet;
using FishNet.CodeGenerating;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EntityHealth), typeof(EntityStats), typeof(EntityResource))]
[RequireComponent(typeof(EntityStamina))]
public class Entity : NetworkBehaviour
{
    public string Name;
    public bool IsOwnedByMe;
    public CharacterClassType ClassType;
    public Transform ModelContainer;

    public int Id = 0;

    private bool _initialized = false;

    [HideInInspector] public EntityHealth Health;
    [HideInInspector] public EntityStats Stats;
    [HideInInspector] public EntityResource Resource;
    [HideInInspector] public EntityStamina Stamina;

    [AllowMutableSyncType]
    public SyncVar<bool> IsDodging = new SyncVar<bool>();

    public override void OnStartClient()
    {
        base.OnStartClient();
        StartCoroutine(DelayedClientInit());
    }

    private void LoadModel()
    {
        ModelContainer.DeleteChildren();
        Instantiate(AppearanceData.Instance().GetModelForClasssType(ClassType), this.ModelContainer);
    }

    public override void OnStartNetwork()
    {
        Id = GetComponent<NetworkObject>().OwnerId;
    }

    public override void OnStartServer()
    {
        Stats = GetComponent<EntityStats>();
        Stats.Initialize(this);

        Health = GetComponent<EntityHealth>();
        Health.Initialize();

        Resource = GetComponent<EntityResource>();
        Resource.Initialize();

        Stamina = GetComponent<EntityStamina>();
        Stamina.Initialize();

        ServerEvents.OnEntityInitialized.Invoke(this);

        IsDodging.Value = false;
    }

    private IEnumerator DelayedClientInit()
    {
        // Wait until all dependencies are likely ready.
        yield return new WaitForEndOfFrame(); // or WaitUntil(() => something != null)

        if (_initialized) yield break;

        IsOwnedByMe = IsOwner;
        GetComponent<Targetable>().IsSelf = IsOwner;
        LoadModel();

        _initialized = true;

        if (!IsServerOnlyStarted)
        {
            // Only invoke on clients that are not hosts, these reeive onstartServer()
            ClientEvents.OnEntityInitialized.Invoke(this);
        }
    }

    [Server]
    public void SetDodging(bool isDodging)
    {
        IsDodging.Value = isDodging;
    }
}
