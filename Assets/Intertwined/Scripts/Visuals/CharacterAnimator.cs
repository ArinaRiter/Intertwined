using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Weapon weapon;
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform leftHand;
    private CharacterController _controller;
    private Animator _animator;
    private Vector3 _speedVector = Vector3.zero;

    private readonly int _speedHash = Animator.StringToHash("Speed");
    private readonly int _runningHash = Animator.StringToHash("IsRunning");
    private readonly int _fallingHash = Animator.StringToHash("IsFalling");
    private readonly int _blockingHash = Animator.StringToHash("IsBlocking");
    private readonly int _peacefulHash = Animator.StringToHash("IsPeaceful");
    private readonly int _switchModeHash = Animator.StringToHash("SwitchMode");
    private readonly int _attackHash = Animator.StringToHash("Attack");
    private readonly int _chargedAttackHash = Animator.StringToHash("ChargedAttack");
    private readonly int _skillOneHash = Animator.StringToHash("Skill1");
    private readonly int _skillTwoHash = Animator.StringToHash("Skill2");
    private readonly int _skillThreeHash = Animator.StringToHash("Skill3");
    private readonly int _jumpHash = Animator.StringToHash("Jump");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _controller = GetComponentInParent<CharacterController>();
    }

    private void Update()
    {
        if (_speedVector != _controller.velocity)
        {
            _speedVector = _controller.velocity;
            var horizontalVector = new Vector2(_speedVector.x, _speedVector.z);
            _animator.SetFloat(_speedHash, horizontalVector.magnitude);
        }
        else if (!playerController.IsMoving)
        {
            _animator.SetFloat(_speedHash, 0);
        }

        _animator.SetBool(_runningHash, playerController.IsRunning);
        _animator.SetBool(_fallingHash, playerController.IsFalling);
    }

    public void Attack()
    {
        _animator.ResetTrigger(_chargedAttackHash);
        _animator.SetTrigger(_attackHash);
    }
    
    public void ChargedAttack()
    {
        _animator.ResetTrigger(_attackHash);
        _animator.SetTrigger(_chargedAttackHash);
    }

    public void Skill(int index)
    {
        switch (index)
        {
            case 1:
                _animator.SetTrigger(_skillOneHash);
                break;
            case 2:
                _animator.SetTrigger(_skillTwoHash);
                break;
            case 3:
                _animator.SetTrigger(_skillThreeHash);
                break;
        }
    }

    public void Block(bool isBlocking)
    {
        _animator.SetBool(_blockingHash, isBlocking);
    }

    public void Jump()
    {
        _animator.SetTrigger(_jumpHash);
    }

    public void SwitchMode(bool isPeaceful)
    {
        _animator.SetBool(_peacefulHash, isPeaceful);
        _animator.SetTrigger(_switchModeHash);
    }
    
    public void WeaponEnable(int attackType)
    {
        weapon.Collider.enabled = true;
        weapon.SetAttackType(attackType);
    }
    
    public void WeaponDisable()
    {
        weapon.Collider.enabled = false;
        weapon.ClearHitTargetsList();
    }

    public void Arm()
    {
        var weaponTransform = weapon.transform;
        weaponTransform.SetParent(leftHand);
        weaponTransform.localPosition = Vector3.zero;
        weaponTransform.localRotation = Quaternion.identity;
    }

    public void Disarm()
    {
        weapon.transform.SetParent(transform);
    }
}