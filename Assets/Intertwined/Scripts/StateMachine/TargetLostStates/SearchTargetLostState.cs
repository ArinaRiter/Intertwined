using UnityEngine;

[CreateAssetMenu(fileName = "SearchTargetLostState", menuName = "AI State Machine/Target Lost States/SearchTargetLostState")]
public class SearchTargetLostState : BaseTargetLostState
{
    public override bool CanBeInState()
    {
        return !_context.IsTargetAcquired;
    }
}
