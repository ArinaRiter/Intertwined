using UnityEngine;

[CreateAssetMenu(fileName = "SearchTargetLostState", menuName = "AI State Machine/Target Lost States/SearchTargetLostState")]
public class SearchTargetLostState : BaseTargetLostState
{
    [SerializeField] private float searchTime = 10f;
    [SerializeField] private float searchRange = 10f;
    [SerializeField] private float stoppingDistance = 2f;
    
    private float _searchTimer;

    public override void EnterState()
    {
        base.EnterState();
        _context.EntityAnimator.SetIsWalking(true);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_exitedState) return;
        if (_context.NavMeshAgent.remainingDistance < stoppingDistance)
        {
            var targetPoint = new Vector3(Random.Range(-searchRange, searchRange), 0f, Random.Range(-searchRange, searchRange));
            _context.NavMeshAgent.destination = _context.transform.position + targetPoint;
        }
        
        _searchTimer += Time.deltaTime;
    }

    public override void ExitState()
    {
        base.ExitState();
        _searchTimer = 0f;
        _context.EntityAnimator.SetIsWalking(false);
    }

    public override bool CanBeInState()
    {
        return _context.Target is null && _searchTimer < searchTime;
    }
}
