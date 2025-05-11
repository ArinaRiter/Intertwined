using System.Collections.Generic;

public class AIStateMachine
{
    private BaseState _currentState;

    public BaseIdleState IdleState { get; private set; }
    public BaseTargetAcquiredState TargetAcquiredState { get; private set; }
    public BaseTargetLostState TargetLostState { get; private set; }
    public BaseDangerState DangerState { get; private set; }
    public List<BaseAttackState> AttackStates { get; private set; }
    public BaseIncapacitatedState IncapacitatedState { get; private set; }
    public AIController Context { get; private set; }

    public AIStateMachine(BaseIdleState idleState, BaseTargetAcquiredState targetAcquiredState,
        BaseTargetLostState targetLostState, BaseDangerState dangerState, List<BaseAttackState> attackStates,
        BaseIncapacitatedState incapacitatedState)
    {
        IdleState = idleState;
        TargetAcquiredState = targetAcquiredState;
        TargetLostState = targetLostState;
        DangerState = dangerState;
        AttackStates = attackStates;
        IncapacitatedState = incapacitatedState;
    }

    public void Initialize(AIController aiController)
    {
        Context = aiController;
        IdleState.Initialize(this, Context);
        TargetAcquiredState.Initialize(this, Context);
        TargetLostState.Initialize(this, Context);
        DangerState.Initialize(this, Context);
        foreach (var attackState in AttackStates) attackState.Initialize(this, Context);
        IncapacitatedState.Initialize(this, Context);
        
        _currentState = IdleState;
        _currentState.EnterState();
    }
    
    public void Update()
    {
        _currentState.UpdateState();
    }
    
    public void SwitchState(BaseState state)
    {
        _currentState.ExitState();
        _currentState = state;
        _currentState.EnterState();
        _currentState.UpdateState();
    }
}
