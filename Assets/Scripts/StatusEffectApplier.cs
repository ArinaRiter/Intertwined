using System;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using Random = UnityEngine.Random;

public class StatusEffectApplier : MonoBehaviour
{
    private void Start()
    {
        var enemies = FindObjectsByType<AIController>(FindObjectsSortMode.None);
        foreach (var enemy in enemies)
        {
            var rarity = PickRandomRarity();
            if (rarity < 0) return;

            Expression<Func<StatusEffectItem, bool>> expression = item => item.Rarity == rarity && item.IsEntity;
            var statusEffects = RealmManager.QueryRealm(expression);
            while (!statusEffects.Any() && rarity > 0)
            {
                rarity--;
                expression = item => item.Rarity == rarity && item.IsEntity;
                statusEffects = RealmManager.QueryRealm(expression);
            }

            if (!statusEffects.Any()) return;
            var statusEffect = statusEffects.First();
            enemy.GetComponent<CharacterStats>().ApplyStatusEffect(statusEffect);
            Debug.Log($"Applied {statusEffect.Name} to {enemy.name}");
        }
    }

    public static void ApplyToEntity(CharacterStats character)
    {
        var rarity = PickRandomRarity();
        if (rarity < 0) return;

        Expression<Func<StatusEffectItem, bool>> expression = item => item.Rarity == rarity && item.IsEntity;
        var statusEffects = RealmManager.QueryRealm(expression);
        while (!statusEffects.Any() && rarity > 0)
        {
            rarity--;
            expression = item => item.Rarity == rarity && item.IsEntity;
            statusEffects = RealmManager.QueryRealm(expression);
        }

        if (!statusEffects.Any()) return;
        var statusEffect = statusEffects.First();
        character.ApplyStatusEffect(statusEffect);
        Debug.Log($"Applied {statusEffect.Name} to {character.name}");
    }

    private static int PickRandomRarity()
    {
        var value = Random.Range(-1f, 1f);
        if (value >= 0.95f) return 3;
        if (value >= 0.8f) return 2;
        if (value >= 0.5f) return 1;
        if (value >= 0f) return 0;
        return -1;
    }
}