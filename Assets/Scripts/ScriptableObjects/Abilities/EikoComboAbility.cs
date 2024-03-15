using UnityEngine;

public class EikoComboAbility : AbilitySO
{
    public override void Activate(GameObject parent)
    {
        var empower = new StatusEffect("Empower", true, 10, new StatMod(StatType.Power, ModType.PercentAdd, 1));
        var characterStats = parent.GetComponent<CharacterStats>();
        characterStats.Energy -= energyCost;
        characterStats.ApplyStatusEffect(empower);
    }
}
