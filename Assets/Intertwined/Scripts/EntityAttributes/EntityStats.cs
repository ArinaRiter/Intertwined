using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class EntityStats : MonoBehaviour, ISaveable
{
    [SerializeField] private StatSO statSO;
    [SerializeField] private AttackSO attackSO;
    
    public AttackSO AttackSO => attackSO;
    
    public ReadOnlyDictionary<StatType, Stat> Stats { get; private set; }
    
    private readonly List<StatusEffect> _statusEffects = new();
    private float _health;
    private float _stamina;
    private float _staminaReplenishTimer;
    private bool _isStaminaReplenishing;
    private bool _isDead;
    private Stat _stability;

    public event Action OnDeath;
    public event Action OnDamageTaken;
    public event Action OnStaminaChanged;
    public event Action OnStagger;

    public float Health
    {
        get => _health;
        private set
        {
            _health = Mathf.Clamp(value, 0, Stats[StatType.MaxHealth].Value);
            if (_health == 0) IsDead = true;
        }
    }

    public float Stamina
    {
        get => _stamina;
        set
        {
            var maxStamina = Stats[StatType.MaxStamina].Value;
            if (_stamina > value)
            {
                _isStaminaReplenishing = false;
                _staminaReplenishTimer = Stats[StatType.StaminaReplenishCooldown].Value;
            }
            else if (value >= maxStamina)
            {
                _isStaminaReplenishing = false;
            }

            _stamina = Mathf.Clamp(value, 0, maxStamina);
        }
    }

    public float DamageReduction { get; private set; }

    public bool IsDead
    {
        get => _isDead;
        private set
        {
            if (_isDead != value)
            {
                _isDead = value;
                if (_isDead) OnDeath?.Invoke();
            }
        }
    }

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

        if (Stats.TryGetValue(StatType.MaxStamina, out var maxStamina))
        {
            Stamina = maxStamina.Value;
            maxStamina.ChangedValue += UpdateMaxStamina;
        }

        if (Stats.TryGetValue(StatType.Armor, out var armor))
        {
            DamageReduction = armor.Value / (200 + armor.Value);
            armor.ChangedValue += UpdateArmor;
        }
        
        _stability = Stats.TryGetValue(StatType.Stability, out var stability) ? stability : new Stat(0);
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

        if (_staminaReplenishTimer > 0)
        {
            _staminaReplenishTimer -= Time.deltaTime;
            if (_staminaReplenishTimer < 0)
            {
                _staminaReplenishTimer = 0;
                _isStaminaReplenishing = true;
                StartCoroutine(ReplenishStamina());
            }
        }
    }

    public void TakeDamage(DamageType damageType, float damage, float pierce, float breach)
    {
        var totalDamage = CalculateDamageTaken(damageType, damage, pierce, breach);
        Health -= totalDamage;
        OnDamageTaken?.Invoke();
        Debug.Log($"Hit, {Health} health remaining");
        if (!IsDead && totalDamage >= _stability.Value) OnStagger?.Invoke();
    }

    public void Heal(float amount)
    {
        if (Stats.TryGetValue(StatType.HealingBonus, out var healingBonus)) amount *= 1 + healingBonus.Value;
        Health += amount;
    }

    private float CalculateDamageTaken(DamageType damageType, float damage, float pierce, float breach)
    {
        float damageResistance = 0;
        switch (damageType)
        {
            case DamageType.Physical:
                if (Stats.TryGetValue(StatType.PhysDamageResistance, out var physResistance))
                {
                    damageResistance = physResistance.Value;
                }
                break;
            case DamageType.Fire:
                if (Stats.TryGetValue(StatType.FireDamageResistance, out var fireResistance))
                {
                    damageResistance = fireResistance.Value;
                }
                break;
            case DamageType.Poison:
                if (Stats.TryGetValue(StatType.PoisonDamageResistance, out var poisonResistance))
                {
                    damageResistance = poisonResistance.Value;
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

    public void RemoveStatusEffect(StatusEffect statusEffect)
    {
        _statusEffects.Remove(statusEffect);
        foreach (var statMod in statusEffect.StatMods)
        {
            if (Stats.ContainsKey(statMod.Stat)) Stats[statMod.Stat].RemoveModifier(statMod);
        }
    }

    private void UpdateMaxHealth(float value)
    {
        if (Health > value)
        {
            Health = value;
        }
    }

    private void UpdateMaxStamina(float value)
    {
        if (Stamina > value)
        {
            Stamina = value;
            IsExhausted = _stamina == 0;
        }
    }

    private void UpdateArmor(float value)
    {
        DamageReduction = value / (200 + value);
    }

    private IEnumerator ReplenishStamina()
    {
        while (_isStaminaReplenishing)
        {
            Stamina += Stats[StatType.StaminaReplenishRate].Value;
            OnStaminaChanged?.Invoke();
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

        if (Stats.TryGetValue(StatType.MaxStamina, out var maxStamina))
        {
            Stamina = maxStamina.Value;
            maxStamina.ChangedValue += UpdateMaxStamina;
        }

        if (Stats.TryGetValue(StatType.Armor, out var armor))
        {
            DamageReduction = armor.Value / (200 + armor.Value);
            armor.ChangedValue += UpdateArmor;
        }
    }

    public SaveData SaveData()
    {
        var guid = GetComponent<GloballyUniqueIdentifier>().GUID;
        var saveData = new EntityData(guid, transform.position, transform.rotation, Health, Stamina);
        return saveData;
    }

    public void LoadData(SaveData data)
    {
        var saveData = (EntityData)data;
        transform.position = data.position;
        transform.rotation = data.rotation;
        Health = saveData.health;
        Stamina = saveData.stamina;
    }
}