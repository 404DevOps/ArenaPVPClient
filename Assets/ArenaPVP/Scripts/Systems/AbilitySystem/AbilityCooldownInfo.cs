using Assets.ArenaPVP.Scripts.Models.Enums;
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
    public AbilityCastInfo(int ownerId, int abilityId, float castDuration, uint castId, bool interrupted = false, InterruptType reason = InterruptType.None)
    {
        this.OwnerId = ownerId;
        this.AbilityId = abilityId;
        this.CastTimeRemaining = castDuration;
        this.WasInterrupted = interrupted;
        this.CastId = castId;
        this.Reason = reason;
    }

    public int OwnerId;
    public int AbilityId;
    public float CastTimeRemaining;
    public uint CastId;
    public bool WasInterrupted;
    public InterruptType Reason;

    public static AbilityCastInfo Null = new AbilityCastInfo();
}