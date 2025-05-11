public abstract class BaseIncapacitatedState : BaseState
{
    public override void UpdateState()
    {
        base.UpdateState();
        if (!CanBeInState())
        {
            if (_stateMachine.DangerState.CanBeInState())
            {
                _stateMachine.SwitchState(_stateMachine.DangerState);
                return;
            }
            if (_stateMachine.TargetAcquiredState.CanBeInState()) _stateMachine.SwitchState(_stateMachine.TargetAcquiredState);
            else if (_stateMachine.TargetLostState.CanBeInState()) _stateMachine.SwitchState(_stateMachine.TargetLostState);
            else _stateMachine.SwitchState(_stateMachine.IdleState);
        }
    }
}