using UnityEngine;

[CreateAssetMenu(fileName = "SearchTargetLostState", menuName = "AI State Machine/Target Lost States/SearchTargetLostState")]
public class SearchTargetLostState : BaseTargetLostState
{
    [SerializeField] private float searchTime = 10f;
    
    private float _searchTimer;
    
    public override void UpdateState()
    {
        base.UpdateState();
        if (_context.NavMeshAgent.remainingDistance < _context.NavMeshAgent.stoppingDistance)
        {
            var targetPoint = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
            _context.NavMeshAgent.destination = _context.transform.position + targetPoint;
        }
        
        _searchTimer += Time.deltaTime;
    }

    public override void ExitState()
    {
        base.ExitState();
        _searchTimer = 0f;
    }

    public override bool CanBeInState()
    {
        return _context.Target is null && _searchTimer < searchTime;
    }
}
