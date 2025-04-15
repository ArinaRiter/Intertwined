using System;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesUI : MonoBehaviour
{
    [SerializeField] private Slider abilitySlider;
    [SerializeField] private AbilityHolder abilityHolder;
    [SerializeField] private Image abilityImage;
    
    void Start()
    {
        abilitySlider.maxValue = abilityHolder.MaxCooldownTime;
        abilitySlider.value = abilityHolder.MaxCooldownTime;
    }

    private void Update()
    {
        abilitySlider.value = abilitySlider.maxValue - abilityHolder.CooldownTime;
        if (abilityHolder.CooldownTime <= 0)
        {
            abilityImage.color = Color.white;
        }
        else
        {
            abilityImage.color = Color.white * 0.8f;
        }
    }
}
