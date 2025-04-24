using System;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    [SerializeField] private Transform launcherPoint;
    [SerializeField] private Projectile projectile;
    [SerializeField] private DamageType damageType;
    
    private CharacterStats _characterStats;

    private void Awake()
    {
        _characterStats = GetComponent<CharacterStats>();
    }

    public void LaunchProjectile()
    {
        var power = _characterStats.Stats.TryGetValue(StatType.Power, out var powerStat) ? powerStat.Value : 1;
        var pierce = _characterStats.Stats.TryGetValue(StatType.Pierce, out var pierceStat) ? pierceStat.Value : 0;
        var breach = _characterStats.Stats.TryGetValue(StatType.Breach, out var breachStat) ? breachStat.Value : 0;
        Stat damageBonusStat;
        switch (damageType)
        {
            case DamageType.Physical:
                _characterStats.Stats.TryGetValue(StatType.PhysDamageBonus, out damageBonusStat);
                break;
            case DamageType.Fire:
                _characterStats.Stats.TryGetValue(StatType.FireDamageBonus, out damageBonusStat);
                break;
            case DamageType.Poison:
                _characterStats.Stats.TryGetValue(StatType.PoisonDamageBonus, out damageBonusStat);
                break;
            case DamageType.True:
                _characterStats.Stats.TryGetValue(StatType.TrueDamageBonus, out damageBonusStat);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        var damageBonus = damageBonusStat?.Value ?? 0;
        var damage = power * (1 + damageBonus);
        
        var currentProjectile = Instantiate(projectile, launcherPoint.position, launcherPoint.rotation);
        currentProjectile.SetupProjectile(damageType, damage, pierce, breach);
    }
}
