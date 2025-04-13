using System;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private DamageType damageType;
    private readonly Dictionary<AttackType, float> _attacks = new();
    private readonly List<CharacterStats> _hitTargets = new();
    private CharacterStats _characterStats;
    private Stat _powerStat;
    private Stat _pierceStat;
    private Stat _breachStat;
    private Stat _damageBonusStat;
    private float _damageMultiplier = 1;
    
    public Collider Collider { get; private set; }

    private void Awake()
    {
        Collider = GetComponent<Collider>();
    }

    private void Start()
    {
        _characterStats = GetComponentInParent<CharacterStats>();
        _characterStats.Stats.TryGetValue(StatType.Power, out _powerStat);
        _characterStats.Stats.TryGetValue(StatType.Pierce, out _pierceStat);
        _characterStats.Stats.TryGetValue(StatType.Breach, out _breachStat);
        switch (damageType)
        {
            case DamageType.Physical:
                _characterStats.Stats.TryGetValue(StatType.PhysDamageBonus, out _damageBonusStat);
                break;
            case DamageType.Fire:
                _characterStats.Stats.TryGetValue(StatType.FireDamageBonus, out _damageBonusStat);
                break;
            case DamageType.Poison:
                _characterStats.Stats.TryGetValue(StatType.PoisonDamageBonus, out _damageBonusStat);
                break;
            case DamageType.True:
                _characterStats.Stats.TryGetValue(StatType.TrueDamageBonus, out _damageBonusStat);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        var attacks = _characterStats.AttackSO.Attacks;
        foreach (var attack in attacks)
        {
            _attacks.Add(attack.Type, attack.Multiplier);
        }
    }

    private void OnDisable()
    {
        ClearHitTargetsList();
    }

    public void SetAttackType(int type)
    {
        if (_attacks.TryGetValue((AttackType)type, out var multiplier))
        {
            _damageMultiplier = multiplier;
        }
        else
        {
            Debug.LogError("Incorrect attack type");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{gameObject.name}: {other.name}");
        if (Utilities.IsLayerInMask(other.gameObject.layer, targetLayers))
        {
            var target = other.GetComponent<CharacterStats>();
            if (_hitTargets.Contains(target)) return;
            var pierce = _pierceStat?.Value ?? 0;
            var breach = _breachStat?.Value ?? 0;
            var power = _powerStat?.Value ?? 0;
            target.TakeDamage(damageType, power * _damageMultiplier, pierce, breach);
            _hitTargets.Add(target);
        }
    }

    public void ClearHitTargetsList()
    {
        _hitTargets.Clear();
    }
}