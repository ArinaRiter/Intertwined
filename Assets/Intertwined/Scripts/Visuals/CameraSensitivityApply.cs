using System;
using UnityEngine;
using Unity.Cinemachine; 

public class CameraSensitivityApplier : MonoBehaviour
{
    private CinemachineInputAxisController _cameraInputAxisController;

    private void Awake()
    {
        _cameraInputAxisController = GetComponent<CinemachineInputAxisController>();
    }

    private void Start()
    {
        ApplySensitivity();
    }

    private void ApplySensitivity()
    {
        var axis = Enum.GetNames(typeof(SensitivityAxis));
        for (var i = 0; i < axis.Length; i++)
        {
            var sensitivity = PlayerPrefs.GetFloat(axis[i], 0);
            if (sensitivity == 0) continue;
            if (i == 1) sensitivity *= -1; 
            _cameraInputAxisController.Controllers[i].Input.Gain = sensitivity;
        }
    }
}