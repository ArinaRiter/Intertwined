using UnityEngine;

[CreateAssetMenu(fileName = "MeleeAttackState", menuName = "AI State Machine/Attack States/MeleeAttackState")]
public class MeleeAttackState : BaseAttackState
{
    public override void UpdateState()
    {
        base.UpdateState();
        _context.EntityAnimator.SetAttack();
    }

    public override void ExitState()
    {
        base.ExitState();
        _context.EntityAnimator.ResetAttack();
    }

    public override bool CanBeInState()
    {
        return _context.IsTargetAttackable;
    }
}
