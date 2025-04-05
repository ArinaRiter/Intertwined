using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private float zoomSensitivity;
    private CinemachineFreeLook _freeLookCamera;
    private CinemachineFreeLook.Orbit[] _orbits;
    private PlayerInputActions _playerInputActions;
    private float _multiplier = 1;

    private void OnEnable()
    {
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.CameraZoom.performed += OnCameraZoom;
    }
    
    private void OnDisable()
    {
        _playerInputActions.Player.CameraZoom.performed -= OnCameraZoom;
        _playerInputActions.Player.Disable();
    }

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _freeLookCamera = GetComponent<CinemachineFreeLook>();
        _orbits = _freeLookCamera.m_Orbits;
    }

    private void OnCameraZoom(InputAction.CallbackContext context)
    {
        var input = _playerInputActions.Player.CameraZoom.ReadValue<float>() * zoomSensitivity;
        _multiplier = Mathf.Clamp(_multiplier - input, 0.5f, 2f);
        var orbits = _orbits.Select(orbit => new CinemachineFreeLook.Orbit(orbit.m_Height, orbit.m_Radius * _multiplier)).ToArray();
        _freeLookCamera.m_Orbits = orbits;
    }
}
