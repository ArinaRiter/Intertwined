public abstract class BaseTargetLostState : BaseState
{
    public override void UpdateState()
    {
        base.UpdateState();
        if (!CanBeInState())
        {
            if (_context.DangerState.CanBeInState()) _context.SwitchState(_context.DangerState);
            else if (_context.TargetAcquiredState.CanBeInState()) _context.SwitchState(_context.TargetAcquiredState);
            else _context.SwitchState(_context.IdleState);
        }
    }
}
