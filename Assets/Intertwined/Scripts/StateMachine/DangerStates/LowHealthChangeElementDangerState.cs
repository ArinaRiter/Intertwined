using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "LowHealthChangeElementDangerState", menuName = "AI State Machine/Danger States/LowHealthChangeElementDangerState")]
public class LowHealthChangeElementDangerState : BaseDangerState
{
    [SerializeField] private float dangerHealthLevel = 0.5f;
    [SerializeField] private float stateAttempts = 1;
    [SerializeField] private float stateCooldown;
    [SerializeField] private List<DamageType> damageTypes;
    [SerializeField] private string animationTrigger;
    [SerializeField] private string animationBool;
    [SerializeField] private Color dangerColor;
    [SerializeField] private ParticleSystem dangerParticles;
    
    private Stat _maxHealth;
    private float _lastStateTime;

    public override void EnterState()
    {
        base.EnterState();
        var type = damageTypes[Random.Range(0, damageTypes.Count)];
        _context.Animator.SetTrigger(animationTrigger);
        _context.Animator.SetBool(string.Concat(animationBool, type), true);
        _context.EntityAnimator.Weapons.ToList().ForEach(weapon => weapon.SetDamageType(type));
        if (dangerParticles is not null && type == DamageType.True) Instantiate(dangerParticles, _context.transform);
        if (dangerColor != Color.clear && type == DamageType.Fire) _context.GetComponentsInChildren<Renderer>().ToList().ForEach(renderer => renderer.material.color = dangerColor);
        _lastStateTime = Time.timeSinceLevelLoad;
        stateAttempts--;
    }

    public override bool CanBeInState()
    {
        if (_maxHealth is null && !_context.EntityStats.Stats.TryGetValue(StatType.MaxHealth, out _maxHealth)) _maxHealth = new Stat(0);
        var isReady = Time.timeSinceLevelLoad - _lastStateTime > stateCooldown;
        return stateAttempts > 0 && isReady && _context.EntityStats.Health <= _maxHealth.Value * dangerHealthLevel;
    }
}