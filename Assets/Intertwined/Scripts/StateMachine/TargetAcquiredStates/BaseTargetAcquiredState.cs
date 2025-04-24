public abstract class BaseTargetAcquiredState : BaseState
{
    public override void UpdateState()
    {
        base.UpdateState();
        if (_context.DangerState.CanBeInState()) _context.SwitchState(_context.DangerState);
        else if (!CanBeInState())
        {
            if (_context.AttackState.CanBeInState()) _context.SwitchState(_context.AttackState);
            else if (_context.TargetLostState.CanBeInState()) _context.SwitchState(_context.TargetLostState);
        }
    }
}
