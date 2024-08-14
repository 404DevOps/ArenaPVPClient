using UnityEngine;

public struct AbilityCooldownInfo
{
    public AbilityCooldownInfo(int ownerId, int abilityId, float cooldown = 0)
    {
        this.OwnerId = ownerId;
        this.AbilityId = abilityId;
        this.CooldownRemaining = cooldown;
    }
    public string Identifier => OwnerId + "_" + AbilityId;
    public int OwnerId;
    public int AbilityId;
    public float CooldownRemaining;
}

public struct AbilityCastInfo
{
    public AbilityCastInfo(int abilityId, float castDuration, bool interrupted = false)
    {
        this.AbilityId = abilityId;
        this.CastTimeRemaining = castDuration;
        this.WasInterrupted = interrupted;
    }

    public int AbilityId;
    public float CastTimeRemaining;
    public bool WasInterrupted;
}