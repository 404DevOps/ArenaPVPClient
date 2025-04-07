public class DamageEntityEffect : AbilityEffectBase
{
    public TargetType Target;
    public DamageType DamageType;
    public float DamageAmount;

    public override void Apply(AbilityBase ability, Entity origin, Entity target)
    {
        var args = new HealthChangedEventArgs()
        {
            Player = target,
            Source = origin,
            HealthChangeAmount = -DamageCalculator.CalculateDamage(origin, target, this),
            HealthChangeType = HealthChangeType.Damage,
            DamageType = DamageType,
            AbilityId = ability.Id
        };

        target.GetComponent<EntityHealth>().UpdateHealthServer(args);
    }
}

