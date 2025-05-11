using UnityEngine;

[CreateAssetMenu(fileName = "ChaseTargetAcquiredState", menuName = "AI State Machine/Target Acquired States/ChaseTargetAcquiredState")]
public class ChaseTargetAcquiredState : BaseTargetAcquiredState
{
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
        return !PeaceModeManager.IsPeaceMode && _context.Target is not null && !_context.IsTargetAttackable;
    }
}
