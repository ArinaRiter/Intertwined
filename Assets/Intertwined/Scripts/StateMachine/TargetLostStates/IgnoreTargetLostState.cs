using UnityEngine;

[CreateAssetMenu(fileName = "IgnoreTargetLostState", menuName = "AI State Machine/Target Lost States/IgnoreTargetLostState")]
public class IgnoreTargetLostState : BaseTargetLostState
{
    public override bool CanBeInState()
    {
        return _context.Target is null;
    }
}
