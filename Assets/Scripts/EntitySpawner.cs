using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using Random = UnityEngine.Random;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> entities;
    [SerializeField] private int dangerLevel;
    
    private void Start()
    {
        InvokeRepeating(nameof(Spawn), 2, 2);
    }

    private void Spawn()
    {
        var randomPos = new Vector3(Random.Range(-5, 5f), 0, Random.Range(-5, 5f));
        var entity = Instantiate(entities[0], transform.position + randomPos, Quaternion.identity);
        var stats = PickEntity();
        entity.GetComponent<CharacterStats>().UpdateStats(stats);
        StatusEffectApplier.ApplyToEntity(entity.GetComponent<CharacterStats>());
    }

    private Dictionary<StatType,Stat> PickEntity()
    {
        Expression<Func<EntityItem, bool>> expression = item => item.DangerLevel == dangerLevel && !item.IsBoss;
        return RealmManager.QueryRealmEntity(expression);
    }
}
