using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class AIStateMachine : MonoBehaviour
{
    [SerializeField] private BaseIdleState idleState;
    [SerializeField] private BaseTargetAcquiredState targetAcquiredState;
    [SerializeField] private BaseTargetLostState targetLostState;
    [SerializeField] private BaseDangerState dangerState;
    [SerializeField] private BaseAttackState attackState;
    [SerializeField] private Detector targetDetector;
    [SerializeField] private Detector attackDetector;

    private BaseState _currentState;

    public BaseIdleState IdleState => idleState;
    public BaseTargetAcquiredState TargetAcquiredState => targetAcquiredState;
    public BaseTargetLostState TargetLostState => targetLostState;
    public BaseDangerState DangerState => dangerState;
    public BaseAttackState AttackState => attackState; 
    
    public NavMeshAgent NavMeshAgent { get; private set; }
    public Collider Target { get; private set; }
    public bool IsTargetAcquired { get; private set; }
    public bool IsTargetAttackable { get; private set; }
    
    private void Awake()
    {
        idleState = Instantiate(idleState);
        targetAcquiredState = Instantiate(targetAcquiredState);
        targetLostState = Instantiate(targetLostState);
        dangerState = Instantiate(dangerState);
        attackState = Instantiate(attackState);
        
        NavMeshAgent = GetComponent<NavMeshAgent>();
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

    private void OnEnable()
    {
        targetDetector.OnTargetsChanged += UpdateDetectedTargets;
        attackDetector.OnTargetsChanged += UpdateAttackableTargets;
    }

    private void OnDisable()
    {
        targetDetector.OnTargetsChanged -= UpdateDetectedTargets;
        attackDetector.OnTargetsChanged -= UpdateAttackableTargets;
    }
    
    private void Update()
    {
        _currentState.UpdateState();
    }

    private void UpdateDetectedTargets()
    {
        if (targetDetector.Targets.Any())
        {
            var minimumDistance = float.MaxValue;
            foreach (var target in targetDetector.Targets)
            {
                var distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance < minimumDistance)
                {
                    minimumDistance = distance;
                    Target = target;
                }
            }
        }
        else Target = null;
        IsTargetAcquired = Target is not null;
    }

    private void UpdateAttackableTargets()
    {
        IsTargetAttackable = attackDetector.Targets.Contains(Target);
    }

    public void SwitchState(BaseState state)
    {
        state.ExitState();
        _currentState = state;
        state.EnterState();
    }
}
