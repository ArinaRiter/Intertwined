public abstract class BaseDangerState : BaseState
{
    public override void UpdateState()
    {
        base.UpdateState();
        if (_stateMachine.IncapacitatedState.CanBeInState()) _stateMachine.SwitchState(_stateMachine.IncapacitatedState);
        else if (!CanBeInState())
        {
            foreach (var attackState in _stateMachine.AttackStates)
            {
                if (attackState.CanBeInState())
                {
                    _stateMachine.SwitchState(attackState);
                    return;
                }
            }
            if (_stateMachine.TargetAcquiredState.CanBeInState()) _stateMachine.SwitchState(_stateMachine.TargetAcquiredState);
            else if (_stateMachine.TargetLostState.CanBeInState()) _stateMachine.SwitchState(_stateMachine.TargetLostState);
            else _stateMachine.SwitchState(_stateMachine.IdleState);
        }
    }
}