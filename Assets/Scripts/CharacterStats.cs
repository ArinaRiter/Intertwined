using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private StatSO statSo;
    public ReadOnlyDictionary<StatType, Stat> Stats { get; private set; }
    
    public float Health { get; private set; }
    public float Stamina { get; private set; }
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

        if (Stats.TryGetValue(StatType.MaxStamina, out var stamina))
        {
            Stamina = stamina.Value;
            stamina.ChangedValue += UpdateStamina;
        }
        
        if (Stats.TryGetValue(StatType.Armor, out var armor))
        {
            DamageReduction = armor.Value / (200 + armor.Value);
            armor.ChangedValue += UpdateArmor;
        }
    }

    public void TakeDamage(DamageType damageType, float damage, float pierce, float breach)
    {
        var totalDamage = CalculateDamage(damageType, damage, pierce, breach);
        Health -= totalDamage;
        if (Health <= 0)
        {
            Health = 0;
            IsDead = true;
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
    
    private void UpdateStamina(float value)
    {
        if (Stamina > value)
        {
            Stamina = value;
            IsExhausted = Stamina <= 0;
        }
    }
    
    private void UpdateArmor(float value)
    {
        DamageReduction = value / (200 + value);
    }

    public IEnumerator ModifierDecay(StatType statType, StatMod statMod, float duration)
    {
        yield return new WaitForSeconds(duration);
        if (Stats.TryGetValue(statType, out var stat))
        {
            stat.RemoveModifier(statMod);
        }
    }

    /*[SerializeField] private StatSO statSo;
    private readonly Dictionary<StatType, Stat> _statDictionary = new();
    private bool _isDead;

    private void Awake()
    {
        var statList = statSo.Stats;
        foreach (var statInfo in statList)
        {
            _statDictionary.Add(statInfo.StatType, new Stat(statInfo.BaseValue));
        }
    }

    private Stat GetStat(StatType statType)
    {
        if (_statDictionary.TryGetValue(statType, out var stat))
        {
            return stat;
        }
        Debug.Log($"No {statType} in {gameObject.name}");
        return null;
    }

    public void TakeDamage(float value, ModType modType)
    {
        var health = GetStat(StatType.Health);
        Debug.Log(health.CurrentValue);
        health.AddModifier(value, modType, false);
        if (health.CurrentValue == 0)
        {
            _isDead = true;
            Debug.Log("You are dead");
        }
    }

    public void ApplyStatModifier(StatType statType, float value, ModType modType, float modifierTime,
        bool maxValueChange)
    {
        var stat = GetStat(statType);
        stat.AddModifier(value, modType, maxValueChange);
        if (modifierTime > 0)
        {
            StartCoroutine(RemoveModifier(stat, value, modType, modifierTime, maxValueChange));
        }
    }

    IEnumerator RemoveModifier(Stat stat, float value, ModType modType, float modifierTime, bool maxValueChange)
    {
        yield return new WaitForSeconds(modifierTime);
        stat.AddModifier(value, modType, maxValueChange);
    }*/
}