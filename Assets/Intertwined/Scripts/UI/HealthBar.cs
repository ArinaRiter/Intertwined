using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider basicHealthSlider;
    [SerializeField] private Slider easeHealthSlider;
    [SerializeField] private EntityStats entityStats;

    private const float LERP_SPEED = 0.1f;

    private void Start()
    {
        if (entityStats.Stats.TryGetValue(StatType.MaxHealth, out var maxHealth))
        {
            basicHealthSlider.maxValue = maxHealth.Value;
            basicHealthSlider.value = entityStats.Health;
            easeHealthSlider.maxValue = maxHealth.Value;
            easeHealthSlider.value = entityStats.Health;
        }
    }
    
    private void OnEnable()
    {
        entityStats.OnDamageTaken += UpdateHealthBar;
    }

    private void OnDisable()
    {
        entityStats.OnDamageTaken -= UpdateHealthBar;
    }

    void Update()
    {
        if (!Mathf.Approximately(basicHealthSlider.value, easeHealthSlider.value))
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, entityStats.Health, LERP_SPEED);
        }
    }

    private void UpdateHealthBar()
    {
        basicHealthSlider.value = entityStats.Health;
    }
}
