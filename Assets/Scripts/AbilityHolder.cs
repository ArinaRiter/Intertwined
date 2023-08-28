using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityHolder : MonoBehaviour
{
    [SerializeField] private AbilitySO ability;
    [SerializeField] private InputActionReference input;

    private AbilityState _state = AbilityState.Ready;
    private float _cost;
    private float _cooldownTime;

    public string Name { get; private set; }

    private void OnEnable()
    {
        input.asset.Enable();
        input.action.performed += OnActivate;
    }

    private void OnDisable()
    {
        input.action.performed -= OnActivate;
        input.asset.Disable();
    }

    private void Update()
    {
        if (_state == AbilityState.Cooldown)
        {
            _cooldownTime -= Time.deltaTime;
            if (_cooldownTime <= 0)
            {
                _state = AbilityState.Ready;
                _cooldownTime = 0;
            }
        }
        else if (_state == AbilityState.Active)
        {
            _state = AbilityState.Cooldown;
        }
    }

    private void OnActivate(InputAction.CallbackContext context)
    {
        Debug.Log("Skill");
        if (_state == AbilityState.Ready)
        {
            ability.Activate(gameObject);
            _state = AbilityState.Active;
            _cooldownTime = ability.CooldownTime;
        }
    }
}