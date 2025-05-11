public abstract class BaseTargetLostState : BaseState
{
    public override void UpdateState()
    {
        base.UpdateState();
        if (_stateMachine.IncapacitatedState.CanBeInState()) _stateMachine.SwitchState(_stateMachine.IncapacitatedState);
        else if (_stateMachine.DangerState.CanBeInState()) _stateMachine.SwitchState(_stateMachine.DangerState);
        else if (!CanBeInState())
        {
            if (_stateMachine.TargetAcquiredState.CanBeInState()) _stateMachine.SwitchState(_stateMachine.TargetAcquiredState);
            else _stateMachine.SwitchState(_stateMachine.IdleState);
        }
    }
}
