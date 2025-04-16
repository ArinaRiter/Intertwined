using System;
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

    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int RunningHash = Animator.StringToHash("IsRunning");
    private static readonly int FallingHash = Animator.StringToHash("IsFalling");
    private static readonly int BlockingHash = Animator.StringToHash("IsBlocking");
    private static readonly int PeacefulHash = Animator.StringToHash("IsPeaceful");
    private static readonly int SwitchModeHash = Animator.StringToHash("SwitchMode");
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int ChargedAttackHash = Animator.StringToHash("ChargedAttack");
    private static readonly int SkillOneHash = Animator.StringToHash("Skill1");
    private static readonly int SkillTwoHash = Animator.StringToHash("Skill2");
    private static readonly int SkillThreeHash = Animator.StringToHash("Skill3");
    private static readonly int JumpHash = Animator.StringToHash("Jump");
    private static readonly int IsDead = Animator.StringToHash("IsDead");
    private static readonly int Stagger = Animator.StringToHash("Stagger");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _controller = GetComponentInParent<CharacterController>();
    }

    private void OnEnable()
    {
        playerController.GetComponent<CharacterStats>().OnDeath += OnDie;
        playerController.GetComponent<CharacterStats>().OnStagger += OnStagger;
    }

    private void OnDisable()
    {
        playerController.GetComponent<CharacterStats>().OnDeath -= OnDie;
        playerController.GetComponent<CharacterStats>().OnStagger -= OnStagger;
    }

    private void Update()
    {
        if (_speedVector != _controller.velocity)
        {
            _speedVector = _controller.velocity;
            var horizontalVector = new Vector2(_speedVector.x, _speedVector.z);
            _animator.SetFloat(SpeedHash, horizontalVector.magnitude);
        }
        else if (!playerController.IsMoving)
        {
            _animator.SetFloat(SpeedHash, 0);
        }

        _animator.SetBool(RunningHash, playerController.IsRunning);
        _animator.SetBool(FallingHash, playerController.IsFalling);
    }

    public void Attack()
    {
        _animator.ResetTrigger(ChargedAttackHash);
        _animator.SetTrigger(AttackHash);
    }
    
    public void ChargedAttack()
    {
        _animator.ResetTrigger(AttackHash);
        _animator.SetTrigger(ChargedAttackHash);
    }

    public void Skill(int index)
    {
        switch (index)
        {
            case 1:
                _animator.SetTrigger(SkillOneHash);
                break;
            case 2:
                _animator.SetTrigger(SkillTwoHash);
                break;
            case 3:
                _animator.SetTrigger(SkillThreeHash);
                break;
        }
    }

    public void Block(bool isBlocking)
    {
        _animator.SetBool(BlockingHash, isBlocking);
    }

    public void Jump()
    {
        _animator.SetTrigger(JumpHash);
    }

    public void SwitchMode(bool isPeaceful)
    {
        _animator.SetBool(PeacefulHash, isPeaceful);
        _animator.SetTrigger(SwitchModeHash);
    }
    
    public void WeaponEnable(int attackType)
    {
        weapon.WeaponCollider.enabled = true;
        weapon.SetAttackType(attackType);
    }
    
    public void WeaponDisable()
    {
        weapon.WeaponCollider.enabled = false;
        weapon.SetAttackType(0);
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

    private void OnDie()
    {
        _animator.SetBool(IsDead, true);
    }

    private void OnStagger()
    {
        _animator.SetTrigger(Stagger);
    }
}