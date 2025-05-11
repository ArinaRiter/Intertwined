public abstract class BaseIdleState : BaseState
{
    public override void UpdateState()
    {
        base.UpdateState();
        if (_stateMachine.IncapacitatedState.CanBeInState()) _stateMachine.SwitchState(_stateMachine.IncapacitatedState);
        else if (_stateMachine.DangerState.CanBeInState()) _stateMachine.SwitchState(_stateMachine.DangerState);
        else if (_stateMachine.TargetAcquiredState.CanBeInState()) _stateMachine.SwitchState(_stateMachine.TargetAcquiredState);
    }

    public override bool CanBeInState()
    {
        return true;
    }
}
