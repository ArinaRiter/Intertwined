using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using Random = UnityEngine.Random;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> entities;
    
    private void Start()
    {
        InvokeRepeating(nameof(Spawn), 2, 2);
    }

    private void Spawn()
    {
        //var entity = Instantiate(entities[0], transform.position, Quaternion.identity);

        if (Random.value < 0.5) return;
        int rarity;
        var value = Random.value;
        if (value >= 0.95) rarity = 3;
        else if (value >= 0.8) rarity = 2;
        else if (value >= 0.5) rarity = 1;
        else rarity = 0;

        Expression<Func<StatModItem, bool>> expression = item => item.Rarity == rarity && item.IsEntity;
        var statusEffects = RealmManager.QueryRealm(expression);
        foreach (var statusEffect in statusEffects)
        {
            Debug.Log(statusEffect.Name);
        }
    }
}
