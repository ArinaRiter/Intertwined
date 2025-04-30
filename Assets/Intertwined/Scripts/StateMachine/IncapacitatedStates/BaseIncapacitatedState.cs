public abstract class BaseIncapacitatedState : BaseState
{
    public override void UpdateState()
    {
        base.UpdateState();
        if (!CanBeInState())
        {
            if (_context.DangerState.CanBeInState())
            {
                _context.SwitchState(_context.DangerState);
                return;
            }
            foreach (var attackState in _context.AttackStates)
            {
                if (attackState.CanBeInState())
                {
                    _context.SwitchState(attackState);
                    return;
                }
            }
            if (_context.TargetAcquiredState.CanBeInState()) _context.SwitchState(_context.TargetAcquiredState);
            else if (_context.TargetLostState.CanBeInState()) _context.SwitchState(_context.TargetLostState);
            else _context.SwitchState(_context.IdleState);
        }
    }
}