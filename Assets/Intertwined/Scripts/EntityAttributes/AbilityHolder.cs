using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityHolder : MonoBehaviour
{
    [SerializeField] private AbilitySO ability;
    [SerializeField] private InputActionReference input;
    [SerializeField] private int skillIndex;

    private CharacterAnimator _characterAnimator;
    private AbilityState _state = AbilityState.Ready;
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

    private void Start()
    {
        _characterAnimator = GetComponentInChildren<CharacterAnimator>();
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
    }

    private void OnActivate(InputAction.CallbackContext context)
    {
        if (_state == AbilityState.Ready)
        {
            ability.Activate(gameObject);
            _characterAnimator.Skill(skillIndex);
            _state = AbilityState.Active;
            _cooldownTime = ability.CooldownTime;
            StartCoroutine(ActiveTime(ability.ActiveTime));
        }
    }

    private IEnumerator ActiveTime(float time)
    {
        yield return new WaitForSeconds(time);
        _state = AbilityState.Cooldown;
    }
}