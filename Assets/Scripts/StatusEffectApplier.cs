using System;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using Random = UnityEngine.Random;

public class StatusEffectApplier : MonoBehaviour
{
    private void Start()
    {
        var enemies = FindObjectsOfType<AIController>();
        foreach (var enemy in enemies)
        {
            var value = Random.Range(0, 1f);
            if (value < 0) return;
            int rarity;
            if (value >= 0.95) rarity = 3;
            else if (value >= 0.8) rarity = 2;
            else if (value >= 0.5) rarity = 1;
            else rarity = 0;

            Expression<Func<StatModItem, bool>> expression = item => item.Rarity == rarity && item.IsEntity;
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
            if (statusEffect.Name == "Swift") Debug.Log($"Applied {statusEffect.Name} to {enemy.name}");
        }
    }
}