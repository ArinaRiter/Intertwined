using UnityEngine;

[CreateAssetMenu(fileName = "ChaseTargetAcquiredState", menuName = "AI State Machine/Target Acquired States/ChaseTargetAcquiredState")]
public class ChaseTargetAcquiredState : BaseTargetAcquiredState
{
    public override void EnterState()
    {
        Debug.Log("ChaseTargetAcquiredState EnterState");
    }

    public override void UpdateState()
    {
        Debug.Log("ChaseTargetAcquiredState UpdateState");
    }

    public override void ExitState()
    {
        Debug.Log("ChaseTargetAcquiredState ExitState");
    }

    public override bool CanEnterState()
    {
        Debug.Log("ChaseTargetAcquiredState CanEnterState");
        return true;
    }
}
