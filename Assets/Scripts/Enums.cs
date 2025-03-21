public enum ModType
{
    Base,
    Flat,
    PercentAdd,
    PercentMult
}

public enum StatType
{
    MaxHealth,
    MaxStamina,
    StaminaReplenishCooldown,
    StaminaReplenishRate,
    CooldownReduction,
    MaxVitality,
    Armor,
    Power,
    AttackSpeed,
    MovementSpeed,
    Pierce,
    Breach,
    Leech,
    CriticalRate,
    CriticalDamage,
    PhysDamageResistance,
    FireDamageResistance,
    PoisonDamageResistance,
    PhysDamageBonus,
    FireDamageBonus,
    PoisonDamageBonus,
    TrueDamageBonus,
    NormalAttackDamageBonus,
    ChargedAttackDamageBonus,
    ThrustAttackDamageBonus,
    PlungeAttackDamageBonus,
    SkillDamageBonus,
    UltimateDamageBonus,
    HealingBonus
}

public enum DamageType
{
    Phys,
    Fire,
    Poison,
    True
}

public enum AbilityState
{
    Ready,
    Active,
    Cooldown
}

public enum AttackType
{
    First,
    Second,
    Third,
    Fourth,
    Fifth,
    Charged,
    Thrust,
    Plunge
}