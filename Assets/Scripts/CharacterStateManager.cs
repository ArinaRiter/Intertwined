using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateManager : MonoBehaviour
{

    CharacterBaseState currentState;
    public CharacterInteractionState InteractionState = new CharacterInteractionState();
    public CharacterImpactState ImpactState = new CharacterImpactState();
    public CharacterAttackState AttackState = new CharacterAttackState();
    public CharacterDeathState DeathState = new CharacterDeathState();
    public CharacterIdleState IdleState = new CharacterIdleState();
    public CharacterJumpState JumpState = new CharacterJumpState();
    public CharacterWalkState WalkState = new CharacterWalkState();    
    public CharacterRunState RunState = new CharacterRunState();

    private void Start()
    {
        currentState = IdleState;

        currentState.EnterState(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionStateEnter(this, collision);
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(CharacterBaseState state)
    {
        currentState = state; 
        state.EnterState(this);
    }

}
