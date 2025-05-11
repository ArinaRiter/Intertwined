using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    [Header("States")]
    [SerializeField] private List<BaseIdleState> idleStates;
    [SerializeField] private List<BaseTargetAcquiredState> targetAcquiredStates;
    [SerializeField] private List<BaseTargetLostState> targetLostStates;
    [SerializeField] private List<BaseDangerState> dangerStates;
    [SerializeField] private List<BaseAttackState> attackStates;
    [SerializeField] private List<BaseIncapacitatedState> incapacitatedStates;
    
    [Header("Detectors")]
    [SerializeField] private List<Detector> targetDetectors;
    [SerializeField] private List<Detector> attackDetectors;
    
    [Header("Attributes")]
    [SerializeField] private float memoryTime = 5f;
    [SerializeField] private bool debugLogging;

    private readonly Dictionary<Collider, int> _detectedTargets = new();
    private readonly Dictionary<Collider, int> _attackableTargets = new();
    private AIStateMachine _aiStateMachine;
    private Coroutine _loseTargetCoroutine;
    private bool _isLosingTarget;
    
    public bool DebugLogging => debugLogging;
    
    public NavMeshAgent NavMeshAgent { get; private set; }
    public EntityAnimator EntityAnimator { get; private set; }
    public Animator Animator { get; private set; }
    public EntityStats EntityStats { get; private set; }
    public Collider EntityCollider { get; private set; }
    public Collider Target { get; private set; }
    public bool IsTargetAttackable { get; private set; }
    
    [HideInInspector] public EntityStatus EntityStatus;
    
    private void Awake()
    {
        InstantiateStates(idleStates);
        InstantiateStates(targetAcquiredStates);
        InstantiateStates(targetLostStates);
        InstantiateStates(dangerStates);
        InstantiateStates(attackStates);
        InstantiateStates(incapacitatedStates);
        
        _aiStateMachine = new AIStateMachine(idleStates, targetAcquiredStates, targetLostStates, dangerStates, attackStates, incapacitatedStates);
        
        NavMeshAgent = GetComponent<NavMeshAgent>();
        EntityAnimator = GetComponent<EntityAnimator>();
        Animator = GetComponent<Animator>();
        EntityStats = GetComponent<EntityStats>();
        EntityCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        _aiStateMachine.Initialize(this);
    }

    private void OnEnable()
    {
        foreach (var detector in targetDetectors) detector.OnTargetsChanged += UpdateDetectedTargets;
        foreach (var detector in attackDetectors) detector.OnTargetsChanged += UpdateAttackableTargets;
        EntityStats.OnStagger += OnStagger;
        EntityStats.OnDeath += OnDeath;
    }

    private void OnDisable()
    {
        foreach (var detector in targetDetectors) detector.OnTargetsChanged -= UpdateDetectedTargets;
        foreach (var detector in attackDetectors) detector.OnTargetsChanged -= UpdateAttackableTargets;
        EntityStats.OnStagger -= OnStagger;
        EntityStats.OnDeath -= OnDeath;
    }

    private void Update()
    {
        _aiStateMachine.Update();
    }

    private void InstantiateStates<T>(List<T> states) where T : BaseState
    {
        for (var i = 0; i < states.Count; i++) states[i] = Instantiate(states[i]);
    }

    private void UpdateDetectedTargets(Collider target, bool detected)
    {
        UpdateTargetDictionary(_detectedTargets, target, detected);
        UpdateTarget();
    }

    private void UpdateAttackableTargets(Collider target, bool detected)
    {
        UpdateTargetDictionary(_attackableTargets, target, detected);
        UpdateTarget();
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

    private void UpdateTarget()
    {
        if (_detectedTargets.Any())
        {
            var detectedAttackableTargets = _attackableTargets.Keys.Intersect(_detectedTargets.Keys).ToList();
            if (detectedAttackableTargets.Any())
            {
                Target = GetClosestTarget(detectedAttackableTargets);
                IsTargetAttackable = true;
            }
            else
            {
                Target = GetClosestTarget(_detectedTargets.Keys);
                IsTargetAttackable = false;
            }

            if (_isLosingTarget)
            {
                StopCoroutine(_loseTargetCoroutine);
                _isLosingTarget = false;
            }
        }
        else {
            IsTargetAttackable = false;
            if (!_isLosingTarget)
            {
                _loseTargetCoroutine = StartCoroutine(LoseTarget());
                _isLosingTarget = true;
            }
        }
    }
    
    private Collider GetClosestTarget(IEnumerable<Collider> targets)
    {
        var targetList = targets.ToList();
        if (!targetList.Any()) return null;
        
        Collider closestTarget = null;
        var minimumDistance = float.MaxValue;
        foreach (var target in targetList)
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
    
    private IEnumerator LoseTarget()
    {
        yield return new WaitForSeconds(memoryTime);
        Target = null;
        _isLosingTarget = false;
    }

    private void OnStagger()
    {
        EntityStatus = EntityStatus.Staggered;
    }

    private void OnDeath()
    {
        EntityStatus = EntityStatus.Dead;
    }
}
