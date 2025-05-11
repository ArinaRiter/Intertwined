public abstract class BaseAttackState : BaseState
{
    public override void UpdateState()
    {
        base.UpdateState();
        
        if (_stateMachine.TryGetAvailableState(_stateMachine.IncapacitatedStates, out var incapacitatedState)) _stateMachine.SwitchState(incapacitatedState);
        else if (_stateMachine.TryGetAvailableState(_stateMachine.DangerStates, out var dangerState)) _stateMachine.SwitchState(dangerState);
        else if (!CanBeInState())
        {
            if (_stateMachine.TryGetAvailableState(_stateMachine.TargetAcquiredStates, out var targetAcquiredState)) _stateMachine.SwitchState(targetAcquiredState);
            else if (_stateMachine.TryGetAvailableState(_stateMachine.TargetLostStates, out var targetLostState)) _stateMachine.SwitchState(targetLostState);
        }
    }
}