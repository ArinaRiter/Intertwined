using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider basicHealthSlider;
    [SerializeField] private Slider easeHealthSlider;
    [SerializeField] private CharacterStats characterStats;
    
    private readonly float _lerpSpeed = 0.1f;

    private void Start()
    {
        if (characterStats.Stats.TryGetValue(StatType.MaxHealth, out var maxHealth))
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
