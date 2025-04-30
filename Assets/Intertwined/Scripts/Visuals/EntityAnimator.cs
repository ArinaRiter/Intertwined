using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EntityAnimator : MonoBehaviour
{
    [SerializeField] private List<Weapon> weapons;
    
    private Animator _animator;
    
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsRunning = Animator.StringToHash("IsRunning");
    private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
    private static readonly int IsDead = Animator.StringToHash("IsDead");
    private static readonly int Stagger = Animator.StringToHash("Stagger");

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
    
    public void SetStagger()
    {
        _animator.SetTrigger(Stagger);
    }
    
    public void SetIsDead(bool isDead)
    {
        _animator.SetBool(IsDead, isDead);
    }

    public void SetWeaponEnabled(int index = 0)
    {
        weapons[index].WeaponCollider.enabled = true;
    }

    public void SetWeaponDisabled(int index = 0)
    {
        weapons[index].WeaponCollider.enabled = false;
        weapons[index].ClearHitTargetsList();
    }
}
