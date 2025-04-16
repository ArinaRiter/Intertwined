public abstract class BaseAttackState : BaseState
{
    public override void UpdateState()
    {
        base.UpdateState();
        
        if (_context.IncapacitatedState.CanBeInState()) _context.SwitchState(_context.IncapacitatedState);
        else if (_context.DangerState.CanBeInState()) _context.SwitchState(_context.DangerState);
        else if (!CanBeInState())
        {
            if (_context.TargetAcquiredState.CanBeInState()) _context.SwitchState(_context.TargetAcquiredState);
            else if (_context.TargetLostState.CanBeInState()) _context.SwitchState(_context.TargetLostState);
        }
    }
}