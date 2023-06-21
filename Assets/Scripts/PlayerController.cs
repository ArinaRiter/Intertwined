using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float turnSmoothTime;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundMask;

    private CharacterController _characterController;
    private PlayerInputActions _playerInputActions;
    private float _turnSmoothVelocity;
    private bool _isRunning;
    
    private Vector3 _verticalVelocity = Vector3.zero;
    
    private const float GRAVITY = -9.81f * 2;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Jump.performed += OnJump;
        _playerInputActions.Player.Run.performed += OnRun;
        _playerInputActions.Player.Run.canceled += OnRun;
    }

    private void OnDisable()
    {
        _playerInputActions.Player.Jump.performed -= OnJump;
        _playerInputActions.Player.Run.performed -= OnRun;
        _playerInputActions.Player.Run.canceled -= OnRun;
        _playerInputActions.Player.Disable();
    }

    private void Update()
    {
        if (!_characterController.isGrounded)
        {
            _verticalVelocity.y += GRAVITY * Time.deltaTime;
            _characterController.Move(_verticalVelocity * Time.deltaTime);
        }
        else
        {
            _verticalVelocity = Vector3.zero;
        }
        
        var inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
        if (inputVector.sqrMagnitude == 0) return;
        
        var inputDirection = new Vector3(inputVector.x, 0, inputVector.y).normalized;
        var directionAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
        var lookingDirection = Mathf.SmoothDampAngle(transform.eulerAngles.y, directionAngle, ref _turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0, lookingDirection, 0);
        var moveDirection = Quaternion.Euler(0, lookingDirection, 0) * Vector3.forward;
        var speedModifier = _isRunning ? 2 : 1;
        _characterController.Move(moveDirection.normalized * (movementSpeed * speedModifier * Time.deltaTime));
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (!Physics.CheckSphere(groundCheck.position, groundDistance, groundMask) && !_characterController.isGrounded) return;
        _verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2 * GRAVITY);
        _characterController.Move(_verticalVelocity * Time.deltaTime);
    }

    private void OnRun(InputAction.CallbackContext context)
    {
        _isRunning = context.performed;
    }
}