using UnityEngine;
using UnityEngine.AI;

public class RandomWayPointTargetProvider : TargetProvider
{
    [Header("References")]
    [SerializeField]
    private NavMeshMovement _autonomousMover;

    [Header("Variables")]
    [SerializeField]
    private float _searchRadius = 20.0f;

    public float SearchRadius { get => _searchRadius; private set => _searchRadius = value; }

    private void Awake()
    {
        Target = new GameObject("RandomWaypoint").transform;
    }

    public override Transform GetTarget()
    {
        Target.transform.position = GenerateRandomWaypoint();
        return Target;
    }

    private Vector3 GenerateRandomWaypoint()
    {
        Vector2 rndPosInsideCircle = UnityEngine.Random.insideUnitCircle * SearchRadius;
        Vector3 rndPos = _autonomousMover.CurrentPosition + new Vector3(rndPosInsideCircle.x, 0, rndPosInsideCircle.y);

        if (NavMesh.SamplePosition(rndPos, out NavMeshHit hit, float.PositiveInfinity, NavMesh.AllAreas))
            return hit.position;

        return Vector3.zero;
    }
}
