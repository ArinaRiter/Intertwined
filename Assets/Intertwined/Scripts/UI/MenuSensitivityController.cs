using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MenuSensitivityController : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private float defaultSensitivity = 3f;
    [SerializeField] private TextMeshProUGUI sensitivityText;
    [SerializeField] private GameObject settingsPanel;
    private const string SENSITIVITY_KEY = "MouseSensitivity";
    private float _initialSensitivity;

    private void Start()
    {
        InitializeSlider();
    }

    private void InitializeSlider()
    {
        _initialSensitivity = PlayerPrefs.GetFloat(SENSITIVITY_KEY, defaultSensitivity);
        sensitivitySlider.value = _initialSensitivity;
        sensitivitySlider.onValueChanged.AddListener(HandleSensitivityChanged);
        sensitivityText.text = Math.Round(_initialSensitivity, 1).ToString(CultureInfo.CurrentCulture);
    }
    
    
    private void HandleSensitivityChanged(float value)
    {
        sensitivityText.text = Math.Round(value, 1).ToString(CultureInfo.CurrentCulture);
    }

    private void Update()
    {
        if (!settingsPanel.activeSelf)
        {
            NormalizeSensitivity();
        }
    }

    public void NormalizeSensitivity()
    {
        var savedSensitivity = PlayerPrefs.GetFloat(SENSITIVITY_KEY, defaultSensitivity);
        sensitivitySlider.value = savedSensitivity;
        sensitivityText.text = Math.Round(savedSensitivity, 1).ToString(CultureInfo.CurrentCulture);
    }

    public void SaveSensitivityChanged()
    {
        _initialSensitivity = sensitivitySlider.value;
        PlayerPrefs.SetFloat(SENSITIVITY_KEY, _initialSensitivity);
        PlayerPrefs.Save();
        Debug.Log($"Sensitivity saved: {_initialSensitivity}");
        
    }

    private void OnDestroy()
    {
        sensitivitySlider?.onValueChanged.RemoveListener(HandleSensitivityChanged);
    }
}