using UnityEngine;

[CreateAssetMenu(fileName = "NeutralTargetAcquireState", menuName = "AI State Machine/Target Acquired States/NeutralTargetAcquireState")]
public class NeutralTargetAcquireState : BaseTargetAcquiredState
{
    private Stat _maxHealth;
    
    public override void UpdateState()
    {
        base.UpdateState();
        if (_exitedState) return;
        if (_context.Target is not null)
        {
            _context.NavMeshAgent.destination = _context.Target.transform.position;
            var isFacingTarget = Vector3.Angle(_context.transform.forward,
                _context.Target.transform.position - _context.transform.position) < 90f;
            _context.EntityAnimator.SetIsRunning(isFacingTarget);
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        _context.EntityAnimator.SetIsRunning(false);
    }

    public override bool CanBeInState()
    {
        if (_maxHealth is null && !_context.EntityStats.Stats.TryGetValue(StatType.MaxHealth, out _maxHealth)) _maxHealth = new Stat(0);
        return PeaceModeManager.IsPeaceMode && _context.EntityStats.Health < _maxHealth.Value && _context.Target is not null && !_context.IsTargetAttackable;
    }
}
