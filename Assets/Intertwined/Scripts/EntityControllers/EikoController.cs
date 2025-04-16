using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class EikoController : PlayerController
{
    [SerializeField] private float chargedAttackCost;

    public event Action OnStaminaChanged;
    private protected override void OnAttack(InputAction.CallbackContext obj)
    {
        if (IsPeaceful)
        {
            IsPeaceful = !IsPeaceful;
            _characterAnimator.SwitchMode(IsPeaceful);
        }
        else if (!IsFocused)
        {
            if (obj.interaction is TapInteraction)
            {
                _characterAnimator.Attack();
            }
            else if (obj.interaction is HoldInteraction && EntityStats.Stamina > chargedAttackCost)
            {
                EntityStats.Stamina -= chargedAttackCost;
                OnStaminaChanged?.Invoke();
                _characterAnimator.ChargedAttack();
            }
        }
    }

    private protected override void OnBlock(InputAction.CallbackContext obj)
    {
        IsFocused = obj.performed;
        if (IsPeaceful)
        {
        }
        else
        {
            _characterAnimator.Block(IsFocused);
        }
    }

    private protected override void OnSwitchMode(InputAction.CallbackContext obj)
    {
        IsPeaceful = !IsPeaceful;
        _characterAnimator.SwitchMode(IsPeaceful);
    }
}