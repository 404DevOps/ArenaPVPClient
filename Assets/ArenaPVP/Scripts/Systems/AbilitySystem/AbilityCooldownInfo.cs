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
    public AbilityCastInfo(int abilityId, float castDuration)
    {
        this.AbilityId = abilityId;
        this.CastTimeRemaining = castDuration;
    }

    public int AbilityId;
    public float CastTimeRemaining;
}