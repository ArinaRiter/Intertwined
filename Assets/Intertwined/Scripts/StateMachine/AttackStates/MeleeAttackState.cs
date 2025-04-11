using UnityEngine;

[CreateAssetMenu(fileName = "MeleeAttackState", menuName = "AI State Machine/Attack States/MeleeAttackState")]
public class MeleeAttackState : BaseAttackState
{
    public override bool CanBeInState()
    {
        return false;
    }
}
