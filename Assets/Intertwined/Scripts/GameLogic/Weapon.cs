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
    private Vector3 _previousPosition;
    private Quaternion _previousRotation;
    private bool _isFirstAttackFrame;
    private readonly RaycastHit[] _hits = new RaycastHit[5];
    
    public CapsuleCollider WeaponCollider { get; private set; }

    private void Awake()
    {
        WeaponCollider = GetComponent<CapsuleCollider>();
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

        if (_characterStats.AttackSO is not null)
        {
            var attacks = _characterStats.AttackSO.Attacks;
            foreach (var attack in attacks)
            {
                _attacks.Add(attack.Type, attack.Multiplier);
            }
        }
    }

    private void Update()
    {
        if (WeaponCollider.enabled)
        {
            if (_isFirstAttackFrame)
            {
                _previousRotation = transform.rotation;
                _previousPosition = _previousRotation * WeaponCollider.center + transform.position;
                _isFirstAttackFrame = false;
                return;
            }
            var rotation = Quaternion.Lerp(_previousRotation, transform.rotation, 0.5f);
            var centerPoint = rotation * WeaponCollider.center + _previousPosition;
            var centerOffset = rotation * Vector3.up * (WeaponCollider.height / 2 - WeaponCollider.radius);
            var point1 = centerPoint + centerOffset;
            var point2 = centerPoint - centerOffset;
            var currentPosition = transform.rotation * WeaponCollider.center + transform.position;
            var direction = currentPosition - _previousPosition;
            
            var hitCount = Physics.CapsuleCastNonAlloc(point1, point2, WeaponCollider.radius, direction.normalized, _hits, direction.magnitude, targetLayers, QueryTriggerInteraction.Collide);
            for (var i = 0; i < hitCount; i++)
            {
                Strike(_hits[i].collider);
            }
            
            _previousRotation = transform.rotation;
            _previousPosition = currentPosition;
        }
        else _isFirstAttackFrame = true;
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
        Strike(other);
    }

    private void Strike(Collider target)
    {
        if (Utilities.IsLayerInMask(target.gameObject.layer, targetLayers))
        {
            var targetStats = target.GetComponent<CharacterStats>();
            if (_hitTargets.Contains(targetStats)) return;
            var pierce = _pierceStat?.Value ?? 0;
            var breach = _breachStat?.Value ?? 0;
            var power = _powerStat?.Value ?? 0;
            var damageBonus = _damageBonusStat?.Value ?? 0;
            targetStats.TakeDamage(damageType, power * (1 + damageBonus) * _damageMultiplier, pierce, breach);
            _hitTargets.Add(targetStats);
        }
    }

    public void ClearHitTargetsList()
    {
        _hitTargets.Clear();
    }
}