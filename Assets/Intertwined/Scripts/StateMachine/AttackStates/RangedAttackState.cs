using UnityEngine;

[CreateAssetMenu(fileName = "RangedAttackState", menuName = "AI State Machine/Attack States/RangedAttackState")]
public class RangedAttackState : BaseAttackState
{
    [SerializeField] private float attackAngle = 90;

    public override void UpdateState()
    {
        base.UpdateState();
        if (_exitedState) return;
        if (_context.Target is not null)
        {
            _context.NavMeshAgent.destination = _context.Target.transform.position;
            var canAttack = Vector3.Angle(_context.transform.forward, _context.Target.transform.position - _context.transform.position) < attackAngle;
            _context.EntityAnimator.SetIsAttacking(canAttack);
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        _context.EntityAnimator.SetIsAttacking(false);
    }

    public override bool CanBeInState()
    {
        return _context.Target is not null && _context.IsTargetAttackable;
    }
}