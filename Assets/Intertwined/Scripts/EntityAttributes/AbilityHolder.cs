using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class AbilityHolder : MonoBehaviour
{
    [SerializeField] private AbilitySO ability;
    [SerializeField] private InputActionReference input;
    [SerializeField] private int skillIndex;

    private CharacterAnimator _characterAnimator;
    private AbilityState _state = AbilityState.Ready;

    public float MaxCooldownTime { get; private set; }
    public float CooldownTime { get; private set; }
    public string Name { get; private set; }

    private void Awake()
    {
        MaxCooldownTime = ability.CooldownTime;
    }

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
            CooldownTime -= Time.deltaTime;
            if (CooldownTime <= 0)
            {
                _state = AbilityState.Ready;
                CooldownTime = 0;
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
            CooldownTime = ability.CooldownTime;
            StartCoroutine(ActiveTime(ability.ActiveTime));
        }
    }

    private IEnumerator ActiveTime(float time)
    {
        yield return new WaitForSeconds(time);
        _state = AbilityState.Cooldown;
    }
}