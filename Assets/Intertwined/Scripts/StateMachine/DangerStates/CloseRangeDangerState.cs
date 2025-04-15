using UnityEngine;

[CreateAssetMenu(fileName = "CloseRangeDangerState", menuName = "AI State Machine/Danger States/CloseRangeDangerState")]
public class CloseRangeDangerState : BaseDangerState
{
    [SerializeField] private float dangerRange = 5f;

    private Vector3 _vectorToTarget;

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
            _context.NavMeshAgent.destination = _context.transform.position - _vectorToTarget.normalized * dangerRange;
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        _context.EntityAnimator.SetIsRunning(false);
    }

    public override bool CanBeInState()
    {
        if (_context.Target is not null)
        {
            _vectorToTarget = _context.Target.transform.position - _context.transform.position;
            return _vectorToTarget.magnitude <= dangerRange;
        }
        return false;
    }
}
