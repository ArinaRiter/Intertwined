using UnityEngine;

[CreateAssetMenu(fileName = "ChaseTargetAcquiredState", menuName = "AI State Machine/Target Acquired States/ChaseTargetAcquiredState")]
public class ChaseTargetAcquiredState : BaseTargetAcquiredState
{
    public override void UpdateState()
    {
        base.UpdateState();
        if (_context.Target is not null) _context.NavMeshAgent.destination = _context.Target.transform.position;
    }

    public override bool CanBeInState()
    {
        return _context.IsTargetAcquired;
    }
}
