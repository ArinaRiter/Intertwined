public abstract class BaseDangerState : BaseState
{
    public override void UpdateState()
    {
        base.UpdateState();
        if (_stateMachine.TryGetAvailableState(_stateMachine.IncapacitatedStates, out var incapacitatedState)) _stateMachine.SwitchState(incapacitatedState);
        else if (!CanBeInState())
        {
            if (_stateMachine.TryGetAvailableState(_stateMachine.AttackStates, out var attackState)) _stateMachine.SwitchState(attackState);
            else if (_stateMachine.TryGetAvailableState(_stateMachine.TargetAcquiredStates, out var targetAcquiredState)) _stateMachine.SwitchState(targetAcquiredState);
            else if (_stateMachine.TryGetAvailableState(_stateMachine.TargetLostStates, out var targetLostState)) _stateMachine.SwitchState(targetLostState);
            else if (_stateMachine.TryGetAvailableState(_stateMachine.IdleStates, out var idleState)) _stateMachine.SwitchState(idleState);
        }
    }
}