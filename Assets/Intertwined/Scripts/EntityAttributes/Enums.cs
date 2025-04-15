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
    Health,
    MaxStamina,
    Stamina,
    MaxEnergy,
    Energy,
    StaminaReplenishCooldown,
    StaminaReplenishRate,
    CooldownReduction,
    MaxVitality,
    Armor,
    Stability,
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
    Physical,
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

public enum EntityStatus
{
    Clear,
    Staggered,
    Dead
}