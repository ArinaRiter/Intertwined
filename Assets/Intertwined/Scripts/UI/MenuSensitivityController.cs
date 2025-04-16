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
    [SerializeField] private SensitivityAxis sensitivityAxis;

    private void OnEnable()
    {
        var initialSensitivity = PlayerPrefs.GetFloat(sensitivityAxis.ToString(), defaultSensitivity);
        sensitivityText.text = Math.Round(initialSensitivity, 1).ToString(CultureInfo.CurrentCulture);
        sensitivitySlider.value = initialSensitivity;
        sensitivitySlider?.onValueChanged.AddListener(HandleSensitivityChanged);
    }

    private void OnDisable()
    {
        sensitivitySlider?.onValueChanged.RemoveListener(HandleSensitivityChanged);
    }

    private void HandleSensitivityChanged(float value)
    {
        sensitivityText.text = Math.Round(value, 1).ToString(CultureInfo.CurrentCulture);
    }

    public void SaveSensitivityChanged()
    {
        PlayerPrefs.SetFloat(sensitivityAxis.ToString(), sensitivitySlider.value);
        PlayerPrefs.Save();
    }
}

public enum SensitivityAxis
{
    MouseSensitivityX,
    MouseSensitivityY
}