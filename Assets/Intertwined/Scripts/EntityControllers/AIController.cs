using UnityEngine;
using UnityEngine.AI;

public class AIController : BaseController
{

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
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        EntityStats.OnDeath += Die;
    }
    
    private void OnDisable()
    {
        EntityStats.OnDeath -= Die;
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
