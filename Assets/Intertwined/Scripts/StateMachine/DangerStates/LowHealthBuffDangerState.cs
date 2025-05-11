using UnityEngine;

[CreateAssetMenu(fileName = "LowHealthBuffDangerState", menuName = "AI State Machine/Danger States/LowHealthBuffDangerState")]
public class LowHealthBuffDangerState : BaseDangerState
{
    [SerializeField] private float dangerHealthLevel = 0.5f;
    [SerializeField] private float buffAttempts = 1;
    [SerializeField] private float buffCooldown = 10;
    [SerializeField] private StatusEffect stateEffect;
    [SerializeField] private StatusEffect buffEffect;
    [SerializeField] private string animationTrigger;
    
    private Stat _maxHealth;
    private StatusEffect _currentStateEffect;
    private StatusEffect _currentBuffEffect;
    private float _lastHealTime;

    public override void EnterState()
    {
        base.EnterState();
        _context.Animator.SetTrigger(animationTrigger);
        _currentStateEffect = new StatusEffect(stateEffect.Name, stateEffect.IsBuff, stateEffect.Duration, stateEffect.StatMods);
        _context.EntityStats.ApplyStatusEffect(_currentStateEffect);
        _lastHealTime = Time.timeSinceLevelLoad;
        buffAttempts--;
    }

    public override void ExitState()
    {
        base.ExitState();
        _context.EntityStats.RemoveStatusEffect(_currentStateEffect);
    }

    public override bool CanBeInState()
    {
        if (_maxHealth is null && !_context.EntityStats.Stats.TryGetValue(StatType.MaxHealth, out _maxHealth)) _maxHealth = new Stat(0);
        var isReady = Time.timeSinceLevelLoad - _lastHealTime > buffCooldown;
        return buffAttempts > 0 && isReady && _context.EntityStats.Health <= _maxHealth.Value * dangerHealthLevel;
    }
}