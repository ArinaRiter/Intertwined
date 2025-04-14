using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider basicHealthSlider;
    [SerializeField] private Slider easeHealthSlider;
    [SerializeField] private CharacterStats characterStats;
    private float _damageTimer;
    private float _lerpSpeed = 0.01f;
    private const float DAMAGE_INTERVAL = 5f;
    void Start()
    {
        if (characterStats != null && characterStats.Stats.TryGetValue(StatType.MaxHealth, out var maxHealth))
        {
            basicHealthSlider.maxValue = maxHealth.Value;
            basicHealthSlider.value = characterStats.Health;
            easeHealthSlider.maxValue = maxHealth.Value;
            easeHealthSlider.value = characterStats.Health;
        }
    }
    
    private void OnEnable()
    {
        characterStats.OnDamageTaken += UpdateHealthBar;
    }

    private void OnDisable()
    {
        characterStats.OnDamageTaken -= UpdateHealthBar;
    }

    void Update()
    {
        _damageTimer += Time.deltaTime;
        if (_damageTimer >= DAMAGE_INTERVAL)
        {
            _damageTimer = 0f;
            //characterStats.TakeDamage(DamageType.Phys, 50f, 0f, 0f);
        }
        
        if (!Mathf.Approximately(basicHealthSlider.value, easeHealthSlider.value))
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, characterStats.Health, _lerpSpeed);
        }
    }

    private void UpdateHealthBar()
    {
        basicHealthSlider.value = characterStats.Health;
    }
}
