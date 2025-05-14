using UnityEngine;

[CreateAssetMenu(fileName = "SpecialAttackState", menuName = "AI State Machine/Attack States/SpecialAttackState")]
public class SpecialAttackState : BaseAttackState
{
    [SerializeField] private float attackCooldown = 10;
    [SerializeField] private string animationTrigger = "SpecialAttack";

    private float _lastAttackTime;

    public override void EnterState()
    {
        base.EnterState();
        _context.Animator.SetTrigger(animationTrigger);
        _lastAttackTime = Time.timeSinceLevelLoad;
    }

    public override bool CanBeInState()
    {
        var isReady = Time.timeSinceLevelLoad - _lastAttackTime > attackCooldown;
        return isReady && _context.Target is not null && _context.IsTargetAttackable;
    }
}
