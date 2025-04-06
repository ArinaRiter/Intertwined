using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private StatSO statSO;
    [SerializeField] private AttackSO attackSO;
    public AttackSO AttackSO => attackSO;
    public ReadOnlyDictionary<StatType, Stat> Stats { get; private set; }
    private readonly List<StatusEffect> _statusEffects = new();
    private float _health;
    private float _energy;
    private float _energyReplenishTimer;
    private bool _isEnergyReplenishing;
    
    public event Action OnDeath;

    public float Health
    {
        get => _health;
        private set => _health = Mathf.Clamp(value, 0, Stats[StatType.MaxHealth].Value);
    }

    public float Energy
    {
        get => _energy;
        set
        {
            var maxEnergy = Stats[StatType.MaxStamina].Value;
            if (_energy > value)
            {
                _isEnergyReplenishing = false;
                _energyReplenishTimer = Stats[StatType.StaminaReplenishCooldown].Value;
            }
            else if (value >= maxEnergy)
            {
                _isEnergyReplenishing = false;
            }

            _energy = Mathf.Clamp(value, 0, maxEnergy);
        }
    }

    public float DamageReduction { get; private set; }
    public bool IsDead { get; private set; }
    public bool IsExhausted { get; private set; }

    private void Awake()
    {
        var statList = statSO.Stats;
        var stats = statList.ToDictionary(statInfo => statInfo.StatType, statInfo => new Stat(statInfo.BaseValue));

        Stats = new ReadOnlyDictionary<StatType, Stat>(stats);
        if (Stats.TryGetValue(StatType.MaxHealth, out var maxHealth))
        {
            Health = maxHealth.Value;
            maxHealth.ChangedValue += UpdateMaxHealth;
        }

        if (Stats.TryGetValue(StatType.MaxStamina, out var maxEnergy))
        {
            Energy = maxEnergy.Value;
            maxEnergy.ChangedValue += UpdateMaxEnergy;
        }

        if (Stats.TryGetValue(StatType.Armor, out var armor))
        {
            DamageReduction = armor.Value / (200 + armor.Value);
            armor.ChangedValue += UpdateArmor;
        }
    }

    private void Update()
    {
        foreach (var statusEffect in _statusEffects.Where(statusEffect => !statusEffect.IsPermanent))
        {
            if (statusEffect.Duration <= 0)
            {
                foreach (var statMod in statusEffect.StatMods)
                {
                    if (Stats.TryGetValue(statMod.Stat, out var stat))
                    {
                        stat.RemoveModifier(statMod);
                    }
                }
                
                _statusEffects.Remove(statusEffect);
                break;
            }

            statusEffect.Duration -= Time.deltaTime;
        }

        if (_energyReplenishTimer > 0)
        {
            _energyReplenishTimer -= Time.deltaTime;
            if (_energyReplenishTimer < 0)
            {
                _energyReplenishTimer = 0;
                _isEnergyReplenishing = true;
                StartCoroutine(ReplenishEnergy());
            }
        }
    }

    public void TakeDamage(DamageType damageType, float damage, float pierce, float breach)
    {
        var totalDamage = CalculateDamageTaken(damageType, damage, pierce, breach);
        Health -= totalDamage;
        Debug.Log($"Hit, {Health} health remaining");
        if (Health <= 0)
        {
            Health = 0;
            IsDead = true;
            OnDeath?.Invoke();
            Debug.Log("Dead");
        }
    }

    private float CalculateDamageTaken(DamageType damageType, float damage, float pierce, float breach)
    {
        float damageResistance = 0;
        switch (damageType)
        {
            case DamageType.Phys:
                if (Stats.TryGetValue(StatType.PhysDamageResistance, out var physRes))
                {
                    damageResistance = physRes.Value;
                }
                break;
            case DamageType.Fire:
                if (Stats.TryGetValue(StatType.FireDamageResistance, out var fireRes))
                {
                    damageResistance = fireRes.Value;
                }
                break;
            case DamageType.Poison:
                if (Stats.TryGetValue(StatType.FireDamageResistance, out var trueRes))
                {
                    damageResistance = trueRes.Value;
                }
                break;
            case DamageType.True:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(damageType), damageType, null);
        }

        return damage * (1 - damageResistance * (1 - breach)) * (1 - DamageReduction * (1 - pierce));
    }

    public void ApplyStatusEffect(StatusEffect statusEffect)
    {
        _statusEffects.Add(statusEffect);
        foreach (var statMod in statusEffect.StatMods)
        {
            if (Stats.ContainsKey(statMod.Stat)) Stats[statMod.Stat].AddModifier(statMod);
        }
    }

    private void UpdateMaxHealth(float value)
    {
        if (Health > value)
        {
            Health = value;
            IsDead = _health == 0;
        }
    }

    private void UpdateMaxEnergy(float value)
    {
        if (Energy > value)
        {
            Energy = value;
            IsExhausted = _energy == 0;
        }
    }

    private void UpdateArmor(float value)
    {
        DamageReduction = value / (200 + value);
    }

    private IEnumerator ReplenishEnergy()
    {
        while (_isEnergyReplenishing)
        {
            Energy += Stats[StatType.StaminaReplenishRate].Value;
            yield return new WaitForSeconds(1f);
        }
    }

    public void UpdateStats(Dictionary<StatType, Stat> stats)
    {
        Stats = new ReadOnlyDictionary<StatType, Stat>(stats);
        if (Stats.TryGetValue(StatType.MaxHealth, out var maxHealth))
        {
            Health = maxHealth.Value;
            maxHealth.ChangedValue += UpdateMaxHealth;
        }

        if (Stats.TryGetValue(StatType.MaxStamina, out var maxEnergy))
        {
            Energy = maxEnergy.Value;
            maxEnergy.ChangedValue += UpdateMaxEnergy;
        }

        if (Stats.TryGetValue(StatType.Armor, out var armor))
        {
            DamageReduction = armor.Value / (200 + armor.Value);
            armor.ChangedValue += UpdateArmor;
        }
    }
}