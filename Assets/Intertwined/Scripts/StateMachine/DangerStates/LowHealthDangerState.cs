using UnityEngine;

[CreateAssetMenu(fileName = "LowHealthDangerState", menuName = "AI State Machine/Danger States/LowHealthDangerState")]
public class LowHealthDangerState : BaseDangerState
{
    [SerializeField] private float dangerHealthLevel = 0.2f;
    
    private Stat _maxHealth;

    public override void EnterState()
    {
        base.EnterState();
        _context.EntityAnimator.SetIsRunning(true);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_exitedState) return;
        if (_context.Target is not null)
        {
            _context.NavMeshAgent.destination = _context.transform.position + (_context.transform.position - _context.Target.transform.position);
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        _context.EntityAnimator.SetIsRunning(false);
    }

    public override bool CanBeInState()
    {
        if (_maxHealth is null && !_context.CharacterStats.Stats.TryGetValue(StatType.MaxHealth, out _maxHealth)) _maxHealth = new Stat(0);
        return _context.Target is not null && _context.CharacterStats.Health <= _maxHealth.Value * dangerHealthLevel;
    }
}