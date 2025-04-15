using UnityEngine;
using UnityEngine.AI;

public class AgentRootMotionSynchronizer : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    private void Awake()
    {
        _navMeshAgent = GetComponentInParent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        
        _navMeshAgent.updatePosition = false;
        _navMeshAgent.updateRotation = true;
        _animator.applyRootMotion = true;
    }

    private void OnAnimatorMove()
    {
        var rootPosition = _animator.rootPosition;
        rootPosition.y = _navMeshAgent.nextPosition.y;
        transform.position = rootPosition;
        _navMeshAgent.nextPosition = rootPosition;
    }
}
