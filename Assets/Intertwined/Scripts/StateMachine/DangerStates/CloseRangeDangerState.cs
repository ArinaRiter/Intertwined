using UnityEngine;

[CreateAssetMenu(fileName = "CloseRangeDangerState", menuName = "AI State Machine/Danger States/CloseRangeDangerState")]
public class CloseRangeDangerState : BaseDangerState
{
    [SerializeField] private float dangerRange = 5f;
    [SerializeField] private float dangerRangeModifier = 2f;
    
    private Vector3 _vectorToTarget;
    private bool _isInDanger;

    public override void EnterState()
    {
        base.EnterState();
        _isInDanger = true;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_exitedState) return;
        if (_context.Target is not null)
        {
            _context.NavMeshAgent.destination = _context.transform.position - _vectorToTarget.normalized * dangerRange;
            var isFacingTarget = Vector3.Angle(_context.transform.forward,
                _context.Target.transform.position - _context.transform.position) < 90f;
            _context.EntityAnimator.SetIsRunning(!isFacingTarget);
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        _context.EntityAnimator.SetIsRunning(false);
        _isInDanger = false;
    }

    public override bool CanBeInState()
    {
        if (_context.Target is not null)
        {
            _vectorToTarget = _context.Target.transform.position - _context.transform.position;
            if (_isInDanger) return _vectorToTarget.magnitude <= dangerRange + dangerRangeModifier;
            return _vectorToTarget.magnitude <= dangerRange;
        }
        return false;
    }
}
