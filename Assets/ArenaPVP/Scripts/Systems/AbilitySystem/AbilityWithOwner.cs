using UnityEngine;

public struct AbilityWithOwner
{
    public AbilityWithOwner(int ownerId, string name)
    {
        this.ownerId = ownerId;
        this.abilityName = name;
    }
    public string Identifier => ownerId + "_" + abilityName;
    public int ownerId;
    public string abilityName;
}

public struct AbilityCastInfo
{
    public AbilityCastInfo(string abilityName)
    {
        this.AbilityName = abilityName;
        this.CastStarted = Time.time;
    }

    public string AbilityName;
    public float CastStarted;
}