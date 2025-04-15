using System;
using UnityEngine;

[CreateAssetMenu(fileName = "StandardIncapacitatedState", menuName = "AI State Machine/Incapacitated States/StandardIncapacitatedState")]
public class StandardIncapacitatedState : BaseIncapacitatedState
{
    public override void EnterState()
    {
        base.EnterState();
        switch (_context.EntityStatus)
        {
            case EntityStatus.Clear:
                break;
            case EntityStatus.Staggered:
                _context.EntityAnimator.SetStagger();
                break;
            case EntityStatus.Dead:
                _context.EntityAnimator.SetIsDead(true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        _context.NavMeshAgent.updateRotation = false;
    }

    public override void ExitState()
    {
        base.ExitState();
        _context.NavMeshAgent.updateRotation = true;
    }

    public override bool CanBeInState()
    {
        return _context.EntityStatus != EntityStatus.Clear;
    }
}