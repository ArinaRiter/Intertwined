using UnityEngine;

[CreateAssetMenu(fileName = "LowHealthHealDangerState", menuName = "AI State Machine/Danger States/LowHealthHealDangerState")]
public class LowHealthHealDangerState : BaseDangerState
{
    [SerializeField] private float dangerHealthLevel = 0.75f;
    [SerializeField] private float healAttempts = 3;
    [SerializeField] private float healCooldown = 10;
    [SerializeField] private StatusEffect healEffect;
    
    private static readonly int Heal = Animator.StringToHash("Heal");
    
    private Stat _maxHealth;
    private float _lastHealTime;

    public override void EnterState()
    {
        base.EnterState();
        _context.Animator.SetTrigger(Heal);
        _context.EntityStats.ApplyStatusEffect(healEffect);
        _lastHealTime = Time.timeSinceLevelLoad;
        healAttempts--;
    }

    public override void ExitState()
    {
        base.ExitState();
        _context.EntityStats.RemoveStatusEffect(healEffect);
    }

    public override bool CanBeInState()
    {
        if (_maxHealth is null && !_context.EntityStats.Stats.TryGetValue(StatType.MaxHealth, out _maxHealth)) _maxHealth = new Stat(0);
        var timePassed = Time.timeSinceLevelLoad - _lastHealTime > healCooldown;
        return healAttempts > 0 && timePassed && _context.EntityStats.Health <= _maxHealth.Value * dangerHealthLevel;
    }
}