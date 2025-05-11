public abstract class BaseTargetAcquiredState : BaseState
{
    public override void UpdateState()
    {
        base.UpdateState();
        if (_stateMachine.TryGetAvailableState(_stateMachine.IncapacitatedStates, out var incapacitatedState)) _stateMachine.SwitchState(incapacitatedState);
        else if (_stateMachine.TryGetAvailableState(_stateMachine.DangerStates, out var dangerState)) _stateMachine.SwitchState(dangerState);
        else if (!CanBeInState())
        {
            if (_stateMachine.TryGetAvailableState(_stateMachine.AttackStates, out var attackState)) _stateMachine.SwitchState(attackState);
            else if (_stateMachine.TryGetAvailableState(_stateMachine.TargetLostStates, out var targetLostState)) _stateMachine.SwitchState(targetLostState);
        }
    }
}
