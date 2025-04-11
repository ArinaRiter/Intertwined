using UnityEngine;

public abstract class BaseState: ScriptableObject
{
    private protected AIStateMachine _context;

    public void Initialize(AIStateMachine context)
    {
        if (_context == null) _context = context;
    }

    public virtual void EnterState()
    {
        Debug.Log($"{this} Enter State");
    }

    public virtual void UpdateState()
    {
        Debug.Log($"{this} Update State");
    }

    public virtual void ExitState()
    {
        Debug.Log($"{this} Exit State");
    }
    
    public abstract bool CanBeInState();
}
