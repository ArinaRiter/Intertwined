using UnityEngine;

[CreateAssetMenu(fileName = "LowHealthDangerState", menuName = "AI State Machine/Danger States/LowHealthDangerState")]
public class LowHealthDangerState : BaseDangerState
{
    [SerializeField] private float dangerHealth = 20;

    public override void EnterState()
    {
        base.EnterState();
        _context.EntityAnimator.SetIsRunning(true);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_exitedState) return;
        if (_context.Target is not null)
        {
            _context.NavMeshAgent.destination = _context.transform.position + (_context.transform.position - _context.Target.transform.position);
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        _context.EntityAnimator.SetIsRunning(false);
    }

    public override bool CanBeInState()
    {
        return _context.Target is not null && _context.GetComponent<CharacterStats>().Health <= dangerHealth;
    }
}