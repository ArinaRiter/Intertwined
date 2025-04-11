using UnityEngine;

[CreateAssetMenu(fileName = "RetreatDangerState", menuName = "AI State Machine/Danger States/RetreatDangerState")]
public class RetreatDangerState : BaseDangerState
{
    public override bool CanBeInState()
    {
        return false;
    }
}
