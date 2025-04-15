using System;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] private Image staminaBar;
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private EikoController eikoController;
    private float _staminaValue;
    private float _maxStaminaValue;
    
    void Start()
    {
        _staminaValue = characterStats.Stamina;
        _maxStaminaValue = characterStats.Stats.TryGetValue(StatType.MaxStamina, out var maxStamina) ? maxStamina.Value : 0;
        staminaBar.fillAmount = (_staminaValue / _maxStaminaValue) * 90.0f / 360.0f;
    }

    private void OnEnable()
    {
        eikoController.OnStaminaChanged += UpdateStaminaBar;
        characterStats.OnStaminaChanged += UpdateStaminaBar;
    }

    private void OnDisable()
    {
        eikoController.OnStaminaChanged -= UpdateStaminaBar;
        characterStats.OnStaminaChanged -= UpdateStaminaBar;
    }

    private void UpdateStaminaBar()
    {
        _staminaValue = characterStats.Stamina;
        float amount = (_staminaValue / _maxStaminaValue) * 90.0f / 360.0f;
        staminaBar.fillAmount = amount;
        Debug.Log($"Stamina value: {_staminaValue} and {amount}");
    }
}
