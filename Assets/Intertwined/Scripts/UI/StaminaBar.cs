using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] private Image staminaBar;
    [FormerlySerializedAs("characterStats")] [SerializeField] private EntityStats entityStats;
    [SerializeField] private EikoController eikoController;
    private float _staminaValue;
    private float _maxStaminaValue;
    
    void Start()
    {
        _staminaValue = entityStats.Stamina;
        _maxStaminaValue = entityStats.Stats.TryGetValue(StatType.MaxStamina, out var maxStamina) ? maxStamina.Value : 0;
        staminaBar.fillAmount = (_staminaValue / _maxStaminaValue) * 90.0f / 360.0f;
    }

    private void OnEnable()
    {
        eikoController.OnStaminaChanged += UpdateStaminaBar;
        entityStats.OnStaminaChanged += UpdateStaminaBar;
    }

    private void OnDisable()
    {
        eikoController.OnStaminaChanged -= UpdateStaminaBar;
        entityStats.OnStaminaChanged -= UpdateStaminaBar;
    }

    private void UpdateStaminaBar()
    {
        _staminaValue = entityStats.Stamina;
        float amount = (_staminaValue / _maxStaminaValue) * 90.0f / 360.0f;
        staminaBar.fillAmount = amount;
        Debug.Log($"Stamina value: {_staminaValue} and {amount}");
    }
}
