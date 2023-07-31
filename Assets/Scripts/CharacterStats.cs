using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private StatSO statSo;
    public ReadOnlyDictionary<StatType, Stat> Stats { get; private set; }
    private List<StatusEffect> _statusEffects = new();

    public float Health { get; private set; }
    public float Energy { get; private set; }
    public float DamageReduction { get; private set; }
    public bool IsDead { get; private set; }
    public bool IsExhausted { get; private set; }
    
    private void Awake()
    {
        var statList = statSo.Stats;
        var stats = new Dictionary<StatType, Stat>();
        foreach (var statInfo in statList)
        {
            stats.Add(statInfo.StatType, new Stat(statInfo.BaseValue));
        }
        Stats = new ReadOnlyDictionary<StatType, Stat>(stats);
        if (Stats.TryGetValue(StatType.MaxHealth, out var health))
        {
            Health = health.Value;
            health.ChangedValue += UpdateHealth;
        }

        if (Stats.TryGetValue(StatType.MaxEnergy, out var stamina))
        {
            Energy = stamina.Value;
            stamina.ChangedValue += UpdateEnergy;
        }
        
        if (Stats.TryGetValue(StatType.Armor, out var armor))
        {
            DamageReduction = armor.Value / (200 + armor.Value);
            armor.ChangedValue += UpdateArmor;
        }
    }

    private void Update()
    {
        foreach (var statusEffect in _statusEffects)
        {
            if (statusEffect.Duration <= 0)
            {
                if (Stats.TryGetValue(statusEffect.Type, out var stat))
                {
                    stat.RemoveModifier(statusEffect.Mod);
                }
                _statusEffects.Remove(statusEffect);
            }
        }
    }

    public void TakeDamage(DamageType damageType, float damage, float pierce, float breach)
    {
        var totalDamage = CalculateDamage(damageType, damage, pierce, breach);
        Health -= totalDamage;
        Debug.Log($"Hit, {Health} health remaining");
        if (Health <= 0)
        {
            Health = 0;
            IsDead = true;
            Debug.Log("Dead");
        }
    }

    private float CalculateDamage(DamageType damageType, float damage, float pierce, float breach)
    {
        float damageResistance = 0;
        switch (damageType)
        {
            case DamageType.Phys:
                if (Stats.TryGetValue(StatType.PhysRes, out var physRes))
                {
                    damageResistance = physRes.Value;
                }
                break;
            case DamageType.Fire:
                if (Stats.TryGetValue(StatType.PhysRes, out var fireRes))
                {
                    damageResistance = fireRes.Value;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(damageType), damageType, null);
        }
        return damage * (1 - damageResistance * (1 - breach)) * (1 - DamageReduction * (1 - pierce));
    }

    private void UpdateHealth(float value)
    {
        if (Health > value)
        {
            Health = value;
            IsDead = Health <= 0;
        }
    }
    
    private void UpdateEnergy(float value)
    {
        if (Energy > value)
        {
            Energy = value;
            IsExhausted = Energy <= 0;
        }
    }
    
    private void UpdateArmor(float value)
    {
        DamageReduction = value / (200 + value);
    }

    
    
    /*public IEnumerator ModifierDecay(StatType statType, StatMod statMod, float duration)
    {
        yield return new WaitForSeconds(duration);
        if (Stats.TryGetValue(statType, out var stat))
        {
            stat.RemoveModifier(statMod);
        }
    }*/
}