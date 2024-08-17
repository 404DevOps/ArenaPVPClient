using Assets.ArenaPVP.Scripts.Enums;
using FishNet.Object;

public class Player : NetworkBehaviour
{
    public string Name;
    public bool IsOwnedByMe;
    public CharacterClassType ClassType;

    public int Id = 0;

    //BaseStats baseStats;
    //public PlayerStats Stats { get; private set; }

    void Awake()
    {
        //baseStats = ClassStatMapping.Instance().GetBaseStats(ClassType);
        //Stats = new PlayerStats(new StatsMediator(), baseStats);
    }

    public override void OnStartClient()
    {
        IsOwnedByMe = this.IsOwner;
        GameEvents.OnPlayerInitialized.Invoke(this);
        GetComponent<Targetable>().IsSelf = this.IsOwner;
    }
    public override void OnStartNetwork()
    {
        Id = GetComponent<NetworkObject>().OwnerId;
    }

    public override void OnStartServer()
    {
        GameEvents.OnPlayerInitialized.Invoke(this);
    }
}
