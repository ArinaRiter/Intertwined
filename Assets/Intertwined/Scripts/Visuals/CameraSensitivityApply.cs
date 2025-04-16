using UnityEngine;
using Unity.Cinemachine; 

public class CameraSensitivityApplier : MonoBehaviour
{
    private CinemachineInputAxisController _freeLookCamera;
    private const string SENSITIVITY_KEY = "MouseSensitivity";

    private void Start()
    {
        ApplySensitivity();
    }

    private void ApplySensitivity()
    {
        var sensitivity = PlayerPrefs.GetFloat(SENSITIVITY_KEY);
        
        if (_freeLookCamera is not null)
        {
            GetComponent<CinemachineInputAxisController>().Controllers[0].Input.Gain = sensitivity;
        }
    }
}