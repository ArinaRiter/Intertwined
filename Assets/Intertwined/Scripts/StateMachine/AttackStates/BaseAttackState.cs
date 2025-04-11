public abstract class BaseAttackState : BaseState
{
    public override void UpdateState()
    {
        base.UpdateState();
        if (!CanBeInState())
        {
            if (_context.DangerState.CanBeInState()) _context.SwitchState(_context.DangerState);
            else if (_context.TargetLostState.CanBeInState()) _context.SwitchState(_context.TargetLostState);
        }
    }
}