using UnityEngine;

public class Weapon : MonoBehaviour
{
    private CharacterStats _characterStats;
    private float _damage;

    private void Start()
    {
        _characterStats = GetComponentInParent<CharacterStats>();
        if (_characterStats.Stats.TryGetValue(StatType.Damage, out Stat damage))
        {
            _damage = damage.Value;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Strike");
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<CharacterStats>().TakeDamage(DamageType.Phys, _damage, 0, 0);
        }
    }
}
