using UnityEngine;

public class AIStateMachine : MonoBehaviour
{
    public AIController Context;
    
    private BaseState _currentState;
    
    public IdleState IdleState = new();
    public PatrolState PatrolState = new();
    public ChaseState ChaseState = new();
    public AttackState AttackState = new();
    public AlarmedState AlarmedState = new();

    private void Start()
    {
        Context = GetComponent<AIController>();
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
    
    private void OnCollisionEnter(Collision other)
    {
        _currentState.OnCollisionEnter(this, other);
    }

    private void OnTriggerEnter(Collider other)
    {
        _currentState.OnTriggerEnter(this, other);
    }
    
    private void OnTriggerStay(Collider other)
    {
        _currentState.OnTriggerEnter(this, other);
    }
    
    private void OnTriggerExit(Collider other)
    {
        _currentState.OnTriggerEnter(this, other);
    }
}
