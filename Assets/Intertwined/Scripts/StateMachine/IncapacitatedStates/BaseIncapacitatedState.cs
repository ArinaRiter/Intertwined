public abstract class BaseIncapacitatedState : BaseState
{
    public override void UpdateState()
    {
        base.UpdateState();
        if (!CanBeInState())
        {
            if (_stateMachine.TryGetAvailableState(_stateMachine.DangerStates, out var dangerState)) _stateMachine.SwitchState(dangerState);
            else if (_stateMachine.TryGetAvailableState(_stateMachine.TargetAcquiredStates, out var targetAcquiredState)) _stateMachine.SwitchState(targetAcquiredState);
            else if (_stateMachine.TryGetAvailableState(_stateMachine.TargetLostStates, out var targetLostState)) _stateMachine.SwitchState(targetLostState);
            else if (_stateMachine.TryGetAvailableState(_stateMachine.IdleStates, out var idleState)) _stateMachine.SwitchState(idleState);
        }
    }
}