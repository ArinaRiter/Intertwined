using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private float baseHealth;
    [SerializeField] private float baseStamina;
    [SerializeField] private float baseArmor;
    [SerializeField] private float baseDamage;
    [SerializeField] private float baseSpeed;

    private float _maxHealth;
    private float _maxStamina;
    private float _armor;
    private float _minDamage;
    private float _maxDamage;
    private float _speed;
    private float _currentHealth;
    private float _currentStamina;
    private float _damageReduction;
    private float _resistance;
    private bool _isDead;

    public void TakeDamage(float damage)
    {
        damage *= 1 - _damageReduction;
        _currentHealth -= damage;
    }
}
