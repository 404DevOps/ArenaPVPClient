using System;

[Serializable]
public abstract class AbilityEffectBase
{
    public abstract void Apply(AbilityBase ability, Entity origin, Entity target);
}