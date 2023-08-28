using UnityEngine;

public class EikoComboAbility : AbilitySO
{
    public override void Activate(GameObject parent)
    {
        var empower = new StatusEffect("Empower", StatType.Power, new StatMod(ModType.PercentAdd, 1), 10);
        var characterStats = parent.GetComponent<CharacterStats>();
        characterStats.Energy -= energyCost;
        characterStats.ApplyStatusEffect(empower);
    }
}
