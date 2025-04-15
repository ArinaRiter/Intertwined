public abstract class BaseDangerState : BaseState
{
    public override void UpdateState()
    {
        base.UpdateState();
        if (!CanBeInState())
        {
            if (_context.AttackState.CanBeInState()) _context.SwitchState(_context.AttackState);
            else if (_context.TargetAcquiredState.CanBeInState()) _context.SwitchState(_context.TargetAcquiredState);
            else if (_context.TargetLostState.CanBeInState()) _context.SwitchState(_context.TargetLostState);
            else _context.SwitchState(_context.IdleState);
        }
    }
}