using UnityEngine;

public class EntityAnimator : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    
    private Animator _animator;
    
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsRunning = Animator.StringToHash("IsRunning");
    private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
    private static readonly int IsStaggered = Animator.StringToHash("IsStaggered");
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetIsWalking(bool isWalking)
    {
        _animator.SetBool(IsWalking, isWalking);
    }

    public void SetIsRunning(bool isRunning)
    {
        _animator.SetBool(IsRunning, isRunning);
    }

    public void SetIsAttacking(bool isAttacking)
    {
        _animator.SetBool(IsAttacking, isAttacking);
    }

    public void SetWeaponEnabled()
    {
        weapon.Collider.enabled = true;
    }

    public void SetWeaponDisabled()
    {
        weapon.Collider.enabled = false;
        weapon.ClearHitTargetsList();
    }
}
