using UnityEngine;
using UnityEngine.AI;

public class RandomWayPointTargetProvider : TargetProvider
{
    [Header("References")]
    [SerializeField]
    private NavMeshMovement _autonomousMover;

    [Header("Variables")]
    [SerializeField]
    private float _generationRadius = 20.0f;

    public float GenerationRadius { get => _generationRadius; set => _generationRadius = value; }

    private void Awake()
    {
        //Prefab for WayPoint and Instantiate?
        Target = new GameObject("RandomWaypoint").transform;
        Target.transform.parent = this.gameObject.transform;
    }

    public override Transform GetTarget()
    {
        Target.transform.position = GenerateRandomWaypoint();
        return Target;
    }

    private Vector3 GenerateRandomWaypoint()
    {
        Vector2 rndPosInsideCircle = UnityEngine.Random.insideUnitCircle * GenerationRadius;
        Vector3 rndPos = _autonomousMover.InitialPosition + new Vector3(rndPosInsideCircle.x, 0, rndPosInsideCircle.y);

        if (NavMesh.SamplePosition(rndPos, out NavMeshHit hit, float.PositiveInfinity, NavMesh.AllAreas))
            return hit.position;

        return _autonomousMover.InitialPosition;
    }
}
