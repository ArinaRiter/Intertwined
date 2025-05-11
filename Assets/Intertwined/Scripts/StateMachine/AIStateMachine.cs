using System.Collections.Generic;
using System.Linq;

public class AIStateMachine
{
    private BaseState _currentState;

    public List<BaseIdleState> IdleStates { get; }
    public List<BaseTargetAcquiredState> TargetAcquiredStates { get; }
    public List<BaseTargetLostState> TargetLostStates { get; }
    public List<BaseDangerState> DangerStates { get; }
    public List<BaseAttackState> AttackStates { get; }
    public List<BaseIncapacitatedState> IncapacitatedStates { get; }

    public AIStateMachine(List<BaseIdleState> idleStates, List<BaseTargetAcquiredState> targetAcquiredStates, List<BaseTargetLostState> targetLostStates, List<BaseDangerState> dangerStates, List<BaseAttackState> attackStates, List<BaseIncapacitatedState> incapacitatedStates)
    {
        IdleStates = idleStates;
        TargetAcquiredStates = targetAcquiredStates;
        TargetLostStates = targetLostStates;
        DangerStates = dangerStates;
        AttackStates = attackStates;
        IncapacitatedStates = incapacitatedStates;
    }

    public void Initialize(AIController context)
    {
        InitializeStates(IdleStates, context);
        InitializeStates(TargetAcquiredStates, context);
        InitializeStates(TargetLostStates, context);
        InitializeStates(DangerStates, context);
        InitializeStates(AttackStates, context);
        InitializeStates(IncapacitatedStates, context);
        
        _currentState = IdleStates[0];
        _currentState.EnterState();
    }

    private void InitializeStates<T>(List<T> states, AIController context) where T : BaseState
    {
        foreach (var state in states) state.Initialize(this, context);
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

    public bool TryGetAvailableState<T>(List<T> states, out T availableState) where T : BaseState
    {
        availableState = states.FirstOrDefault(state => state.CanBeInState());
        return availableState is not null;
    }
}
