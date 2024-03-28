using System.Collections.Generic;
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
        var randomPos = new Vector3(Random.Range(-5, 5f), 0, Random.Range(-5, 5f));
        var entity = Instantiate(entities[0], transform.position + randomPos, Quaternion.identity);
    }
}
