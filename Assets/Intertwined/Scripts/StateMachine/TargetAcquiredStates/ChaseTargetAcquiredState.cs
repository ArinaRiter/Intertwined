using UnityEngine;

[CreateAssetMenu(fileName = "ChaseTargetAcquiredState", menuName = "AI State Machine/Target Acquired States/ChaseTargetAcquiredState")]
public class ChaseTargetAcquiredState : BaseTargetAcquiredState
{
    public override void EnterState()
    {
        base.EnterState();
        _context.EntityAnimator.SetIsRunning(true);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_context.Target is not null) _context.NavMeshAgent.destination = _context.Target.transform.position;
    }

    public override void ExitState()
    {
        base.ExitState();
        _context.EntityAnimator.SetIsRunning(false);
    }

    public override bool CanBeInState()
    {
        return _context.Target is not null && !_context.IsTargetAttackable;
    }
}
