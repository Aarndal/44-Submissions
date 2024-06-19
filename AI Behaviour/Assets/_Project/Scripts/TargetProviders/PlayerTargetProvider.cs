using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PlayerTargetProvider : TargetProvider
{
    [SerializeField]
    private AIEnemy _entity;

    [SerializeField]
    private string _targetTag = "Player";
    [SerializeField]
    private float _searchRadius = 10f;
    [SerializeField]
    private LayerMask _layerMask;
    private NavMeshPath tempNavMeshPath;

    public float SearchRadius => _searchRadius;

    private void Awake()
    {
    }

    public override Transform GetTarget()
    {
        Target = FindClosestPlayer();
        return Target;
    }

    private Transform FindClosestPlayer()
    {
        var allPreyInSearchRadius = Physics.OverlapSphere(transform.position, _searchRadius, _layerMask).ToList();
        var allPlayers = allPreyInSearchRadius.FindAll(prey => prey.transform.tag == _targetTag);

        float distanceToClosestPlayer = float.MaxValue;
        Transform closestPlayer = null;

        for (int i = 0; i < allPlayers.Count; i++)
        {
            if (allPlayers[i] == null)
                continue;

            var player = allPlayers[i].transform;
            tempNavMeshPath = new NavMeshPath();

            bool hasPathToTarget = NavMesh.CalculatePath(transform.position, player.position, _entity.AutonomousMover.NavMeshAgent.areaMask, tempNavMeshPath);
            if (hasPathToTarget == false)
                continue;

            float distance = Vector3.SqrMagnitude(transform.position - tempNavMeshPath.corners[0]);

            for (int j = 1; j < tempNavMeshPath.corners.Length; j++)
                distance += Vector3.SqrMagnitude(tempNavMeshPath.corners[j - 1] - tempNavMeshPath.corners[j]);

            if (distance >= distanceToClosestPlayer)
                continue;

            distanceToClosestPlayer = distance;
            closestPlayer = player;
        }

        return closestPlayer;
    }
}
