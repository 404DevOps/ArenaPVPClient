using JetBrains.Annotations;

public struct CastEventArgs
{
    public CastEventArgs(int onwerId, int abilityId, uint castId, uint serverTick)
    {
        OwnerId = onwerId;
        AbilityId = abilityId;
        CastId = castId;
        ServerTick = serverTick;
    }
    public int OwnerId;
    public int AbilityId;
    public uint CastId;
    public uint ServerTick;
}
