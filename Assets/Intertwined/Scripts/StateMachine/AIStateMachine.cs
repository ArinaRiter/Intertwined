using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class AIStateMachine : MonoBehaviour
{
    [Header("States")]
    [SerializeField] private BaseIdleState idleState;
    [SerializeField] private BaseTargetAcquiredState targetAcquiredState;
    [SerializeField] private BaseTargetLostState targetLostState;
    [SerializeField] private BaseDangerState dangerState;
    [SerializeField] private BaseAttackState attackState;
    
    [Header("Detectors")]
    [SerializeField] private List<Detector> targetDetectors;
    [SerializeField] private List<Detector> attackDetectors;
    
    [Header("Attributes")]
    [SerializeField] private float detectionAngle;

    private BaseState _currentState;
    private readonly Dictionary<Collider, int> _detectedTargets = new();
    private readonly Dictionary<Collider, int> _attackableTargets = new();

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
        foreach (var detector in targetDetectors) detector.OnTargetsChanged += UpdateDetectedTargets;
        foreach (var detector in attackDetectors) detector.OnTargetsChanged += UpdateAttackableTargets;
    }

    private void OnDisable()
    {
        foreach (var detector in targetDetectors) detector.OnTargetsChanged -= UpdateDetectedTargets;
        foreach (var detector in attackDetectors) detector.OnTargetsChanged -= UpdateAttackableTargets;
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

    private void UpdateDetectedTargets(Collider target, bool detected)
    {
        UpdateTargetDictionary(_detectedTargets, target, detected);

        Target = GetClosestTarget(_detectedTargets);
        IsTargetAcquired = Target is not null;
    }

    private void UpdateAttackableTargets(Collider target, bool detected)
    {
        UpdateTargetDictionary(_attackableTargets, target, detected);

        var attackableTarget = GetClosestTarget(_attackableTargets);
        if (attackableTarget is not null) Target = attackableTarget;
        IsTargetAttackable = attackableTarget is not null;
    }

    private void UpdateTargetDictionary(Dictionary<Collider, int> targets, Collider target, bool detected)
    {
        if (detected)
        {
            targets.TryAdd(target, 0);
            targets[target]++;
        }
        else
        {
            if (targets[target] > 1) targets[target]--;
            else targets.Remove(target);
        }
    }
    
    private Collider GetClosestTarget(Dictionary<Collider, int> targets)
    {
        if (!targets.Any()) return null;
        
        Collider closestTarget = null;
        var minimumDistance = float.MaxValue;
        foreach (var target in targets.Keys)
        {
            var distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < minimumDistance)
            {
                minimumDistance = distance;
                closestTarget = target;
            }
        }

        return closestTarget;
    }
}
