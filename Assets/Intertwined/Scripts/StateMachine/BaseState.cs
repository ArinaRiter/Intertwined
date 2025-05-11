using UnityEngine;

public abstract class BaseState: ScriptableObject
{
    private protected AIStateMachine _stateMachine;
    private protected AIController _context;
    private protected bool _exitedState;

    public void Initialize(AIStateMachine stateMachine, AIController context)
    {
        if (_stateMachine == null) _stateMachine = stateMachine;
        if (_context == null) _context = context;
    }

    public virtual void EnterState()
    {
        if (_context.DebugLogging) Debug.Log($"{this} Enter State");
        _exitedState = false;
    }

    public virtual void UpdateState()
    {
        if (_context.DebugLogging) Debug.Log($"{this} Update State");
    }

    public virtual void ExitState()
    {
        if (_context.DebugLogging) Debug.Log($"{this} Exit State");
        _exitedState = true;
    }
    
    public abstract bool CanBeInState();
}
