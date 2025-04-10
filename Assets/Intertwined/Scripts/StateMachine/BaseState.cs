using UnityEngine;

public abstract class BaseState: ScriptableObject
{
    private AIStateMachine _context;

    public void Initialize(AIStateMachine context)
    {
        if (context == null) _context = context;
    }
    
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract bool CanEnterState();
}
