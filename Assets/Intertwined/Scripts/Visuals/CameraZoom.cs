using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private float zoomSensitivity;
    private CinemachineOrbitalFollow _freeLookCamera;
    private Cinemachine3OrbitRig.Settings _orbits;
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
        _freeLookCamera = GetComponent<CinemachineOrbitalFollow>();
        _orbits = _freeLookCamera.Orbits;
    }

    private void OnCameraZoom(InputAction.CallbackContext context)
    {
        var input = _playerInputActions.Player.CameraZoom.ReadValue<float>() * zoomSensitivity;
        _multiplier = Mathf.Clamp(_multiplier - input, 0.5f, 2f);
        var orbit = _orbits;
        orbit.Top.Radius = _orbits.Top.Radius * _multiplier;
        orbit.Center.Radius = _orbits.Center.Radius * _multiplier;
        orbit.Bottom.Radius = _orbits.Bottom.Radius * _multiplier;
        _freeLookCamera.Orbits = orbit;
    }
}
