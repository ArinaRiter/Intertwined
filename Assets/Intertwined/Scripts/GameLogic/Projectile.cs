using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private int durability = 1;
    
    private Rigidbody _rigidbody;
    private DamageType _damageType;
    private float _damage;
    private float _pierce;
    private float _breach;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _rigidbody.linearVelocity = transform.forward * speed;
    }

    public void SetupProjectile(DamageType damageType, float damage, float pierce, float breach)
    {
        _damageType = damageType;
        _damage = damage;
        _pierce = pierce;
        _breach = breach;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out CharacterStats characterStats))
        {
            characterStats.TakeDamage(_damageType, _damage, _pierce, _breach);
        }

        durability--;
        if (durability <= 0)
        {
            Destroy(gameObject);
        }
    }
}
