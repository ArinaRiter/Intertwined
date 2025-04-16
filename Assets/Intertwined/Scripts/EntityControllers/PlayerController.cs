using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public abstract class PlayerController : BaseController
{
    [SerializeField] private protected Transform cameraTransform;

    private CharacterController _characterController;
    private PlayerInputActions _playerInputActions;

    private const float GRAVITY = -9.81f;

    private protected override void Awake()
    {
        base.Awake();
        _characterController = GetComponent<CharacterController>();
        _playerInputActions = new PlayerInputActions();
        _characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Jump.performed += OnJump;
        _playerInputActions.Player.Run.performed += OnRun;
        _playerInputActions.Player.Run.canceled += OnRun;
        _playerInputActions.Player.Attack.performed += OnAttack;
        _playerInputActions.Player.Block.performed += OnBlock;
        _playerInputActions.Player.Block.canceled += OnBlock;
        _playerInputActions.Player.SwitchMode.performed += OnSwitchMode;
        EntityStats.OnDeath += OnDie;
    }

    private void OnDisable()
    {
        _playerInputActions.Player.Jump.performed -= OnJump;
        _playerInputActions.Player.Run.performed -= OnRun;
        _playerInputActions.Player.Run.canceled -= OnRun;
        _playerInputActions.Player.Attack.performed -= OnAttack;
        _playerInputActions.Player.Block.performed -= OnBlock;
        _playerInputActions.Player.Block.canceled -= OnBlock;
        _playerInputActions.Player.SwitchMode.performed -= OnSwitchMode;
        _playerInputActions.Player.Disable();
        EntityStats.OnDeath -= OnDie;
    }

    private void Update()
    {
        IsGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (!_characterController.isGrounded)
        {
            _verticalVelocity.y += GRAVITY * Time.deltaTime;
        }
        else
        {
            _verticalVelocity = Vector3.zero;
        }

        if (IsGrounded) _speedModifier = IsRunning ? 2 : 1;
        IsFalling = !IsGrounded && _verticalVelocity.y < 0;

        var inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
        if (inputVector.sqrMagnitude == 0)
        {
            IsMoving = false;
            if (!IsGrounded && _fallingForward)
            {
                var fallDirection = Quaternion.Euler(0, transform.eulerAngles.y, 0) * Vector3.forward;
                _characterController.Move((fallDirection * (_speedModifier * _movementSpeed) + _verticalVelocity) *
                                          Time.deltaTime);
            }
            else
            {
                _characterController.Move(_verticalVelocity * Time.deltaTime);
                if (IsFocused)
                {
                    var lookingDirection = CalculateLookingDirection(cameraTransform.eulerAngles.y);
                    transform.rotation = lookingDirection;
                }
            }
            return;
        }

        IsMoving = true;
        var inputDirection = new Vector3(inputVector.x, 0, inputVector.y).normalized;
        var directionAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                             cameraTransform.eulerAngles.y;
        Vector3 moveDirection;
        if (IsFocused)
        {
            var lookingDirection = CalculateLookingDirection(cameraTransform.eulerAngles.y);
            transform.rotation = lookingDirection;
            moveDirection = Quaternion.Euler(0, directionAngle, 0) * Vector3.forward;
        }
        else
        {
            var lookingDirection = CalculateLookingDirection(directionAngle);
            transform.rotation = lookingDirection;
            moveDirection = lookingDirection * Vector3.forward;
        }
        _characterController.Move((moveDirection.normalized * _speedModifier + _verticalVelocity) *
                                  (_movementSpeed * Time.deltaTime));
    }

    private Quaternion CalculateLookingDirection(float target)
    {
        var lookingDirection = Mathf.SmoothDampAngle(transform.eulerAngles.y, target, ref _turnSmoothVelocity,
            turnTime * (IsGrounded ? 1 : airborneTurnTimeModificator));
        return Quaternion.Euler(0, lookingDirection, 0);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (!IsGrounded && !_characterController.isGrounded || IsJumping) return;
        _characterAnimator.Jump();
        IsJumping = true;
        _fallingForward = IsMoving;
        StartCoroutine(Jump());
    }

    private void OnRun(InputAction.CallbackContext context)
    {
        IsRunning = context.performed;
    }

    private protected abstract void OnAttack(InputAction.CallbackContext obj);

    private protected abstract void OnBlock(InputAction.CallbackContext obj);

    private protected abstract void OnSwitchMode(InputAction.CallbackContext obj);

    private IEnumerator Jump()
    {
        yield return new WaitForSeconds(0.05f);
        _verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2 * GRAVITY);
        _characterController.Move(_verticalVelocity * Time.deltaTime);
        IsJumping = false;
    }

    private void OnDie()
    {
        enabled = false;
        
    }
}