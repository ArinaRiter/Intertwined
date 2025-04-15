using UnityEngine;

[CreateAssetMenu(fileName = "StandIdleState", menuName = "AI State Machine/Idle States/StandIdleState")]
public class StandIdleState : BaseIdleState
{
    public override void EnterState()
    {
        base.EnterState();
        _context.NavMeshAgent.destination = _context.transform.position;
    }
}
