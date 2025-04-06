using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Ability", fileName = "New Ability", order = 54)]
public class AbilitySO : ScriptableObject
{
    [SerializeField] private protected string abilityName;
    [SerializeField] private protected float energyCost;
    [SerializeField] private protected float cooldownTime;
    [SerializeField] private protected float activeTime;

    public string AbilityName => abilityName;
    public float EnergyCost => energyCost;
    public float CooldownTime => cooldownTime;
    public float ActiveTime => activeTime;

    public virtual void Activate(GameObject parent){}
}
