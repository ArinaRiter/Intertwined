using UnityEngine;

public class AIStateMachine : MonoBehaviour
{
    private BaseState _currentState;
    
    public IdleState IdleState = new();
    public PatrolState PatrolState = new();
    public ChaseState ChaseState = new();
    public AttackState AttackState = new();
    public AlarmedState AlarmedState = new();

    private void Start()
    {
        _currentState = IdleState;
        _currentState.EnterState(this);
    }

    private void Update()
    {
        _currentState.UpdateState(this);
    }

    public void SwitchState(BaseState state)
    {
        _currentState = state;
        state.EnterState(this);
    }
}
