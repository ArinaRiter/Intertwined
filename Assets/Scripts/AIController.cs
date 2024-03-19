using Pathfinding;
using UnityEngine;

public class AIController : BaseController
{
    [SerializeField] private AIStatsSO aiStatsSo;
    [SerializeField] private Transform destination;

    private AIPath _aiPath;
    private SphereCollider _detectionCollider;
    private Transform _target;
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
    }

    private protected override void Start()
    {
        base.Start();
        _aiPath.maxSpeed = _movementSpeed;
        _aiPath.rotationSpeed = 90 / turnTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (_target != null)
        {
            destination.position = _target.position;
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
        }
    }

    private protected override void SetMovementSpeed(float value)
    {
        base.SetMovementSpeed(value);
        _aiPath.maxSpeed = _movementSpeed;
    }
}
