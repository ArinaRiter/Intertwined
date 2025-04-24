using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> entities;
    [SerializeField] private float spawnRadius = 20;
    [SerializeField] private float spawnInterval = 30;
    
    private void Start()
    {
        InvokeRepeating(nameof(Spawn), spawnInterval, spawnInterval);
    }

    private void Spawn()
    {
        var randomPos = new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, Random.Range(-spawnRadius, spawnRadius));
        var entity = Instantiate(entities[Random.Range(0, entities.Count)], transform.position + randomPos, Quaternion.identity);
        //StatusEffectApplier.ApplyToEntity(entity);
    }
}
