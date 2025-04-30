public abstract class BaseTargetAcquiredState : BaseState
{
    public override void UpdateState()
    {
        base.UpdateState();
        if (_context.IncapacitatedState.CanBeInState()) _context.SwitchState(_context.IncapacitatedState);
        else if (_context.DangerState.CanBeInState()) _context.SwitchState(_context.DangerState);
        else if (!CanBeInState())
        {
            foreach (var attackState in _context.AttackStates)
            {
                if (attackState.CanBeInState())
                {
                    _context.SwitchState(attackState);
                    return;
                }
            }
            if (_context.TargetLostState.CanBeInState()) _context.SwitchState(_context.TargetLostState);
        }
    }
}
