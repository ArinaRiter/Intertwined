using System;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    [SerializeField] private Transform launcherPoint;
    [SerializeField] private Projectile projectile;
    [SerializeField] private DamageType damageType;
    
    private EntityStats _entityStats;

    private void Awake()
    {
        _entityStats = GetComponent<EntityStats>();
    }

    public void LaunchProjectile()
    {
        var power = _entityStats.Stats.TryGetValue(StatType.Power, out var powerStat) ? powerStat.Value : 1;
        var pierce = _entityStats.Stats.TryGetValue(StatType.Pierce, out var pierceStat) ? pierceStat.Value : 0;
        var breach = _entityStats.Stats.TryGetValue(StatType.Breach, out var breachStat) ? breachStat.Value : 0;
        Stat damageBonusStat;
        switch (damageType)
        {
            case DamageType.Physical:
                _entityStats.Stats.TryGetValue(StatType.PhysDamageBonus, out damageBonusStat);
                break;
            case DamageType.Fire:
                _entityStats.Stats.TryGetValue(StatType.FireDamageBonus, out damageBonusStat);
                break;
            case DamageType.Poison:
                _entityStats.Stats.TryGetValue(StatType.PoisonDamageBonus, out damageBonusStat);
                break;
            case DamageType.True:
                _entityStats.Stats.TryGetValue(StatType.TrueDamageBonus, out damageBonusStat);
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
