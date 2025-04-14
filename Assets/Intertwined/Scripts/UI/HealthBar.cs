using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider basicHealthSlider;
    [SerializeField] private Slider easeHealthSlider;
    [SerializeField] private CharacterStats targetCharacterStats;
    private float _damageTimer;
    private float _lerpSpeed = 0.01f;
    private const float DAMAGE_INTERVAL = 5f;
    void Start()
    {
        if (targetCharacterStats == null)
        {
            targetCharacterStats = gameObject.GetComponent<CharacterStats>();
        }

        if (targetCharacterStats != null && targetCharacterStats.Stats.TryGetValue(StatType.MaxHealth, out var maxHealth))
        {
            basicHealthSlider.maxValue = maxHealth.Value;
            basicHealthSlider.value = targetCharacterStats.Health;
            easeHealthSlider.maxValue = maxHealth.Value;
            easeHealthSlider.value = targetCharacterStats.Health;
        }
    }
    
    private void OnEnable()
    {
        targetCharacterStats.OnDamageTaken += UpdateHealthBar;
    }

    private void OnDisable()
    {
        targetCharacterStats.OnDamageTaken -= UpdateHealthBar;
    }

    void Update()
    {
        _damageTimer += Time.deltaTime;
        if (_damageTimer >= DAMAGE_INTERVAL)
        {
            _damageTimer = 0f;
            targetCharacterStats.TakeDamage(DamageType.Phys, 50f, 0f, 0f);
        }
        
        if (!Mathf.Approximately(basicHealthSlider.value, easeHealthSlider.value))
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, targetCharacterStats.Health, _lerpSpeed);
        }
    }

    private void UpdateHealthBar()
    {
        basicHealthSlider.value = targetCharacterStats.Health;
    }
}
