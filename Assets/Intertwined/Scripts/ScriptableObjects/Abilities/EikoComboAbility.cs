using System.Collections.Generic;
using UnityEngine;

public class EikoComboAbility : AbilitySO
{
    public override void Activate(GameObject parent)
    {
        var statMods = new List<StatMod>() { new StatMod(StatType.Power, ModType.PercentAdd, 1) };
        var empower = new StatusEffect("Empower", true, 10, statMods);
        Debug.Log(empower.Name);
        var characterStats = parent.GetComponent<CharacterStats>();
        characterStats.Stamina -= energyCost;
        characterStats.ApplyStatusEffect(empower);
    }
}