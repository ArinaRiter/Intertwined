using UnityEngine;

public class AlarmedState : BaseState
{
    public override void EnterState(AIStateMachine aiStateMachine)
    {
        Debug.Log("Alarmed state entered");
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