using UnityEngine;

[CreateAssetMenu(fileName = "PeacefulTargetAcquiredState", menuName = "AI State Machine/Target Acquired States/PeacefulTargetAcquiredState")]
public class PeacefulTargetAcquiredState : BaseTargetAcquiredState
{
    public override bool CanBeInState()
    {
        return PeaceModeManager.IsPeaceMode && _context.Target is not null;
    }
}
