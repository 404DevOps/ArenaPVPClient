using Assets.ArenaPVP.Scripts.Enums;
using FishNet;
using FishNet.Object;
using System;
using System.Collections;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public string Name;
    public bool IsOwnedByMe;
    public CharacterClassType ClassType;
    public Transform ModelContainer;

    public int Id = 0;

    private bool _initialized = false;

    public PlayerHealth Health;
    public PlayerStats Stats;
    public PlayerResource Resource;
    //BaseStats baseStats;
    //public PlayerStats Stats { get; private set; }

    void Awake()
    {
        //baseStats = ClassStatMapping.Instance().GetBaseStats(ClassType);
        //Stats = new PlayerStats(new StatsMediator(), baseStats);
    }

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
        Stats = GetComponent<PlayerStats>();
        Stats.Initialize(this);

        Health = GetComponent<PlayerHealth>();
        Health.Initialize();

        Resource = GetComponent<PlayerResource>();
        Resource.Initialize();


        ClientEvents.OnPlayerInitialized.Invoke(this);
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
            ClientEvents.OnPlayerInitialized.Invoke(this);
        }
    }
}
