using UnityEngine;

public class AttackState : BaseState
{
    public override void EnterState(AIStateMachine aiStateMachine)
    {
        Debug.Log("Attack state entered");
    }

    public override void UpdateState(AIStateMachine aiStateMachine)
    {
    }

    public override void OnCollisionEnter(AIStateMachine aiStateMachine, Collision collision)
    {
    }
    
    public override void OnTriggerEnter(AIStateMachine aiStateMachine, Collider collider)
    {
    }
}
