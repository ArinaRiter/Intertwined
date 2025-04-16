using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    [SerializeField] private protected float jumpHeight;
    [SerializeField] private protected float turnTime;
    [SerializeField] private protected float airborneTurnTimeModificator;
    [SerializeField] private protected Transform groundCheck;
    [SerializeField] private protected float groundDistance;
    [SerializeField] private protected LayerMask groundMask;
    
    private protected EntityStats EntityStats;
    private protected CharacterAnimator _characterAnimator;
    private protected Vector3 _verticalVelocity = Vector3.zero;
    private protected float _movementSpeed;
    private protected float _turnSmoothVelocity;
    private protected float _speedModifier = 1;
    private protected bool _fallingForward;
    
    public bool IsMoving { get; private protected set; }
    public bool IsRunning { get; private protected set; }
    public bool IsJumping { get; private protected set; }
    public bool IsFalling { get; private protected set; }
    public bool IsGrounded { get; private protected set; }
    public bool IsPeaceful { get; private protected set; }
    public bool IsFocused { get; private protected set; }

    private protected virtual void Awake()
    {
        EntityStats = GetComponent<EntityStats>();
    }
    
    private protected virtual void Start()
    {
        _characterAnimator = GetComponentInChildren<CharacterAnimator>();
        if (EntityStats.Stats.TryGetValue(StatType.MovementSpeed, out var speed))
        {
            _movementSpeed = speed.Value;
            speed.ChangedValue += SetMovementSpeed;
        }
    }

    private protected virtual void SetMovementSpeed(float value)
    {
        _movementSpeed = value;
    }
}
