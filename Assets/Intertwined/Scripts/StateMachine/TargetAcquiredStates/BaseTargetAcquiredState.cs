public abstract class BaseTargetAcquiredState : BaseState
{
    public override void UpdateState()
    {
        base.UpdateState();
        if (_stateMachine.IncapacitatedState.CanBeInState()) _stateMachine.SwitchState(_stateMachine.IncapacitatedState);
        else if (_stateMachine.DangerState.CanBeInState()) _stateMachine.SwitchState(_stateMachine.DangerState);
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
            if (_stateMachine.TargetLostState.CanBeInState()) _stateMachine.SwitchState(_stateMachine.TargetLostState);
        }
    }
}
