public struct CastEventArgs
{
    public CastEventArgs(int onwerId, int abilityId, uint castId)
    { 
        OwnerId = onwerId;
        AbilityId = abilityId;
        CastId = castId;
    }
    public int OwnerId;
    public int AbilityId;
    public uint CastId;
}