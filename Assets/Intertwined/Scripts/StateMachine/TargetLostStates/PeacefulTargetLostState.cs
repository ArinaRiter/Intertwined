using UnityEngine;

[CreateAssetMenu(fileName = "PeacefulTargetLostState", menuName = "AI State Machine/Target Lost States/PeacefulTargetLostState")]
public class PeacefulTargetLostState : BaseTargetLostState
{
    public override bool CanBeInState()
    {
        return PeaceModeManager.IsPeaceMode && _context.Target is null;
    }
}
