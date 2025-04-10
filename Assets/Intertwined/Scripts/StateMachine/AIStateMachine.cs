using UnityEngine;

public class AIStateMachine : MonoBehaviour
{
    [SerializeField] private BaseIdleState idleState;
    [SerializeField] private BaseTargetAcquiredState targetAcquiredState;
    [SerializeField] private BaseTargetLostState targetLostState;
    [SerializeField] private BaseDangerState dangerState;
    [SerializeField] private BaseAttackState attackState;
    
    private BaseState _currentState;

    private void Awake()
    {
        idleState = Instantiate(idleState);
        targetAcquiredState = Instantiate(targetAcquiredState);
        targetLostState = Instantiate(targetLostState);
        dangerState = Instantiate(dangerState);
        attackState = Instantiate(attackState);
    }

    private void Start()
    {
        idleState.Initialize(this);
        targetAcquiredState.Initialize(this);
        targetLostState.Initialize(this);
        dangerState.Initialize(this);
        attackState.Initialize(this);
        
        _currentState = idleState;
        _currentState.EnterState();
    }

    private void Update()
    {
        _currentState.UpdateState();
    }

    public void SwitchState(BaseState state)
    {
        state.ExitState();
        _currentState = state;
        state.EnterState();
    }
}
