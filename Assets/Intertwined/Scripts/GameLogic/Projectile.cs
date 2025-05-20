using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private int durability = 1;
    [SerializeField] private SoundType impactSound;
    
    private Rigidbody _rigidbody;
    private DamageType _damageType;
    private float _damage;
    private float _pierce;
    private float _breach;
    private Collider _targetCollider;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        var targetDirection = (_targetCollider.transform.position - transform.position).normalized;
        _rigidbody.linearVelocity = targetDirection * speed;
    }

    public void SetupProjectile(DamageType damageType, float damage, float pierce, float breach, Collider targetCollider)
    {
        _damageType = damageType;
        _damage = damage;
        _pierce = pierce;
        _breach = breach;
        _targetCollider = targetCollider;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out EntityStats characterStats))
        {
            characterStats.TakeDamage(_damageType, _damage, _pierce, _breach);
            AudioManagerSO.Play(impactSound, transform.position);
        }

        durability--;
        if (durability <= 0)
        {
            Destroy(gameObject);
        }
    }
}
