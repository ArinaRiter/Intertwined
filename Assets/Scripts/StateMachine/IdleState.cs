using Pathfinding;
using UnityEngine;

public class IdleState : BaseState
{
    public override void EnterState(AIStateMachine aiStateMachine)
    {
        Debug.Log("Idle state entered");
    }

    public override void UpdateState(AIStateMachine aiStateMachine)
    {
    }

    public override void OnCollisionEnter(AIStateMachine aiStateMachine, Collision collision)
    {
    }

    public override void OnTriggerEnter(AIStateMachine aiStateMachine, Collider collider)
    {
        if (collider.TryGetComponent(out BaseController controller))
        {
            
        }
    }

    public override void OnTriggerStay(AIStateMachine aiStateMachine, Collider collider)
    {
        throw new System.NotImplementedException();
    }

    public override void OnTriggerExit(AIStateMachine aiStateMachine, Collider collider)
    {
        throw new System.NotImplementedException();
    }
}
