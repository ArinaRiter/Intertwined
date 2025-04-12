using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private DamageType damageType;
    [SerializeField] [Range(0, 1)] private float pierce;
    [SerializeField] [Range(0, 1)] private float breach;
    private readonly Dictionary<AttackType, float> _attacks = new();
    private readonly List<CharacterStats> _hitEnemies = new();
    private CharacterStats _characterStats;
    private float _damage;
    private float _damageMultiplier;

    private void Start()
    {
        _characterStats = GetComponentInParent<CharacterStats>();
        if (_characterStats.Stats.TryGetValue(StatType.Power, out Stat power))
        {
            _damage = power.Value;
        }

        var attacks = _characterStats.AttackSO.Attacks;
        foreach (var attack in attacks)
        {
            _attacks.Add(attack.Type, attack.Multiplier);
        }
    }

    public void SetAttackType(int type)
    {
        if (_attacks.TryGetValue((AttackType)type, out float multiplier))
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
        Debug.Log($"{other.gameObject.name} trigger enter");
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<CharacterStats>();
            if (_hitEnemies.Contains(enemy)) return;
            _hitEnemies.Add(enemy);
            enemy.TakeDamage(damageType, _damage * _damageMultiplier, pierce, breach);
            Debug.Log($"{other.gameObject.name} hit");
        }
    }

    public void ClearHitEnemiesList()
    {
        _hitEnemies.Clear();
    }
}