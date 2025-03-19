using UnityEngine;

public abstract class BaseState
{
    public abstract void EnterState(AIStateMachine aiStateMachine);
    public abstract void UpdateState(AIStateMachine aiStateMachine);
    public abstract void OnCollisionEnter(AIStateMachine aiStateMachine, Collision collision);
    public abstract void OnTriggerEnter(AIStateMachine aiStateMachine, Collider collider);
    public abstract void OnTriggerStay(AIStateMachine aiStateMachine, Collider collider);
    public abstract void OnTriggerExit(AIStateMachine aiStateMachine, Collider collider);
}
