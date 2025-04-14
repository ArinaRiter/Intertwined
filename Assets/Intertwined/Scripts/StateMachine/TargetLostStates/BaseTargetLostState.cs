public abstract class BaseTargetLostState : BaseState
{
    public override void UpdateState()
    {
        base.UpdateState();
        if (_context.DangerState.CanBeInState()) _context.SwitchState(_context.DangerState);
        else if (!CanBeInState())
        {
            if (_context.TargetAcquiredState.CanBeInState()) _context.SwitchState(_context.TargetAcquiredState);
            else _context.SwitchState(_context.IdleState);
        }
    }
}
