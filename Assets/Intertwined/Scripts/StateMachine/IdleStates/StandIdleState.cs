using UnityEngine;

[CreateAssetMenu(fileName = "StandIdleState", menuName = "AI State Machine/Idle States/StandIdleState")]
public class StandIdleState : BaseIdleState
{
    public override void EnterState()
    {
        Debug.Log("StandIdleState EnterState");
    }

    public override void UpdateState()
    {
        Debug.Log("StandIdleState UpdateState");
    }

    public override void ExitState()
    {
        Debug.Log("StandIdleState ExitState");
    }

    public override bool CanEnterState()
    {
        Debug.Log("StandIdleState CanEnterState");
        return true;
    }
}
