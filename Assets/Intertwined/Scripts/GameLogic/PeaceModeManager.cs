using UnityEngine;

public class PeaceModeManager : MonoBehaviour
{
    private AIStateMachine[] _stateMachines;

    public void SetPeaceMode(bool peaceful)
    {
        if (peaceful)
        {
            _stateMachines = FindObjectsByType<AIStateMachine>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        }

        foreach (var stateMachine in _stateMachines)
        {
            stateMachine.SetPeaceful(peaceful);
        }
    }
}
