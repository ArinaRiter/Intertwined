using System;
using Pathfinding;
using UnityEngine;

public class AIController : BaseController
{
    [SerializeField] private AIStatsSO aiStatsSo;

    private AIPath _aiPath;
    private SphereCollider _detectionCollider;
    private Transform _target;
    private AIDestinationSetter _aiDestinationSetter;
    private float _detectionRange;
    private float _detectionAngle;
    private float _detectionSpeed;

    private protected override void Awake()
    {
        base.Awake();
        _detectionRange = aiStatsSo.DetectionRange;
        _detectionAngle = aiStatsSo.DetectionAngle;
        _detectionSpeed = aiStatsSo.DetectionSpeed;
        _aiPath = GetComponent<AIPath>();
        _detectionCollider = GetComponent<SphereCollider>();
        _detectionCollider.radius = _detectionRange;
        _aiDestinationSetter = GetComponent<AIDestinationSetter>();
    }

    private protected override void Start()
    {
        base.Start();
        _aiPath.maxSpeed = _movementSpeed;
        _aiPath.rotationSpeed = 90 / turnTime;
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
            _aiDestinationSetter.target = _target;
        }
        else if (other.TryGetComponent(out BaseController controller))
        {
            _target = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == _target)
        {
            _target = null;
            _aiDestinationSetter.target = null;
        }
    }

    private protected override void SetMovementSpeed(float value)
    {
        base.SetMovementSpeed(value);
        Debug.Log("Speed changed to" + _movementSpeed);
        _aiPath.maxSpeed = _movementSpeed;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
