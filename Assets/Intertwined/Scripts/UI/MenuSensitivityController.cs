using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MenuSensitivityController : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private float defaultSensitivity;
    [SerializeField] private TextMeshProUGUI sensitivityText;
    private const string SENSITIVITY_KEY = "MouseSensitivity";
    private float initialSensitivity;

    private void Start()
    {
        InitializeSlider();
    }

    private void InitializeSlider()
    {
        initialSensitivity = PlayerPrefs.GetFloat(SENSITIVITY_KEY, defaultSensitivity);
        sensitivitySlider.value = initialSensitivity;
        sensitivitySlider.onValueChanged.AddListener(HandleSensitivityChanged);
        sensitivityText.text = Math.Round(initialSensitivity, 1).ToString(CultureInfo.CurrentCulture);
    }
    
    
    private void HandleSensitivityChanged(float value)
    {
        sensitivityText.text = Math.Round(value, 1).ToString(CultureInfo.CurrentCulture);
    }

    public void NormalizeSensitivity()
    {
        float savedSensitivity = PlayerPrefs.GetFloat(SENSITIVITY_KEY, defaultSensitivity);
        sensitivitySlider.value = savedSensitivity;
        sensitivityText.text = Math.Round(savedSensitivity, 1).ToString(CultureInfo.CurrentCulture);
    }

    public void SaveSensitivityChanged()
    {
        initialSensitivity = sensitivitySlider.value;
        PlayerPrefs.SetFloat(SENSITIVITY_KEY, initialSensitivity);
        PlayerPrefs.Save();
        Debug.Log($"Sensitivity saved: {initialSensitivity}");
    }

    private void OnDestroy()
    {
        sensitivitySlider?.onValueChanged.RemoveListener(HandleSensitivityChanged);
    }
}