using UnityEngine;
using UnityEngine.AI;

public class AIController : BaseController
{
    [SerializeField] private AIStatsSO aiStatsSo;

    private NavMeshAgent _navMeshAgent;
    private SphereCollider _detectionCollider;
    private Transform _target;
    private float _detectionRange;
    private float _detectionAngle;
    private float _detectionTime;
    private float _detectionTimer;

    private protected override void Awake()
    {
        base.Awake();
        _detectionRange = aiStatsSo.DetectionRange;
        _detectionAngle = aiStatsSo.DetectionAngle;
        _detectionTime = aiStatsSo.DetectionTime;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _detectionCollider = GetComponent<SphereCollider>();
        _detectionCollider.radius = _detectionRange;
    }

    private void OnEnable()
    {
        _characterStats.OnDeath += Die;
    }
    
    private void OnDisable()
    {
        _characterStats.OnDeath -= Die;
    }

    private void OnTriggerStay(Collider other)
    {
        if (_target != null)
        {
            if (_detectionTimer < _detectionTime) _detectionTimer += Time.deltaTime;
            else _navMeshAgent.SetDestination(_target.position);
        }
        else if (other.TryGetComponent(out PlayerController _))
        {
            _target = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == _target)
        {
            _target = null;
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
