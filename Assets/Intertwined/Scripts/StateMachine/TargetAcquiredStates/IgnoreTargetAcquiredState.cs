using UnityEngine;

[CreateAssetMenu(fileName = "IgnoreTargetAcquiredState", menuName = "AI State Machine/Target Acquired States/IgnoreTargetAcquiredState")]
public class IgnoreTargetAcquiredState : BaseTargetAcquiredState
{
    public override bool CanBeInState()
    {
        return _context.Target is not null;
    }
}
