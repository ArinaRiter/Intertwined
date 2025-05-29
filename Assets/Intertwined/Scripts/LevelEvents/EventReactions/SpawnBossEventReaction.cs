using UnityEngine;

public class SpawnBossEventReaction : BaseEventReaction
{
    [SerializeField] private AIController boss; 
    
    public override void React()
    {
        Instantiate(boss, transform.position, transform.rotation);
    }
}
