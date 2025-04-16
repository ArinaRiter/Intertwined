public abstract class BaseIdleState : BaseState
{
    public override void UpdateState()
    {
        base.UpdateState();
        if (_context.IncapacitatedState.CanBeInState()) _context.SwitchState(_context.IncapacitatedState);
        else if (_context.DangerState.CanBeInState()) _context.SwitchState(_context.DangerState);
        else if (_context.TargetAcquiredState.CanBeInState()) _context.SwitchState(_context.TargetAcquiredState);
    }

    public override bool CanBeInState()
    {
        return true;
    }
}
