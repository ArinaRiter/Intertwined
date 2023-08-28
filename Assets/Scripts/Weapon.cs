using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private readonly Dictionary<AttackType, float> _attacks = new();
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
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Strike");
            other.GetComponent<CharacterStats>().TakeDamage(DamageType.Phys, _damage * _damageMultiplier, 0, 0);
        }
    }
}