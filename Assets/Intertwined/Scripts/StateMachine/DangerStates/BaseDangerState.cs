public abstract class BaseDangerState : BaseState
{
    public override void UpdateState()
    {
        base.UpdateState();
        if (!CanBeInState()) _context.SwitchState(_context.IdleState);
    }
}