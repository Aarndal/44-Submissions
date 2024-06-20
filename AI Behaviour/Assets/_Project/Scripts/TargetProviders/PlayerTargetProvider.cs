using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
[RequireComponent(typeof(SphereCollider))]
public class PlayerTargetProvider : TargetProvider
{
    [Header("References")]
    [SerializeField]
    private NavMeshMovement _autonomousMover;

    [Header("Variables")]
    [SerializeField]
    private string _targetTag = "Player";
    [SerializeField]
    private LayerMask _layerMask;
    [SerializeField]
    private float _searchRadius = 10f;
    [SerializeField]
    private int _maxTargetsToSearch = 5;

    private int _numTargetsFound = 0;
    private SphereCollider _sphereCollider;
    private Collider[] _allTargetsInSearchRadius;
    private List<Collider> _closestPlayers;
    private NavMeshPath _tempNavMeshPath;

    public float SearchRadius => _searchRadius;

    private void Awake()
    {
        _allTargetsInSearchRadius = new Collider[_maxTargetsToSearch];
        _closestPlayers = new();
        _tempNavMeshPath = new NavMeshPath();

        _sphereCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        _sphereCollider.radius = _searchRadius;
        _sphereCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        GetTarget();
    }

    private void OnTriggerExit(Collider other)
    {
        GetTarget();
    }

    public override Transform GetTarget()
    {
        Target = FindClosestPlayer();
        return Target;
    }

    private Transform FindClosestPlayer()
    {
        _closestPlayers.Clear();

        _numTargetsFound = Physics.OverlapSphereNonAlloc(transform.position, _searchRadius, _allTargetsInSearchRadius, _layerMask, QueryTriggerInteraction.Ignore);
        for (int i = 0; i < _numTargetsFound; i++)
        {
            if (_allTargetsInSearchRadius[i].transform.CompareTag(_targetTag))
                _closestPlayers.Add(_allTargetsInSearchRadius[i]);
        }

        float distanceToClosestPlayer = float.MaxValue;
        Transform closestPlayer = null;

        for (int i = 0; i < _closestPlayers.Count; i++)
        {
            if (_closestPlayers[i] == null)
                continue;

            bool hasPathToTarget = NavMesh.CalculatePath(transform.position, _closestPlayers[i].transform.position, _autonomousMover.NavMeshAgent.areaMask, _tempNavMeshPath);
            if (hasPathToTarget == false)
                continue;

            float sqrDistance = Vector3.SqrMagnitude(_tempNavMeshPath.corners[0] - transform.position);

            for (int j = 1; j < _tempNavMeshPath.corners.Length; j++)
                sqrDistance += Vector3.SqrMagnitude(_tempNavMeshPath.corners[j] - _tempNavMeshPath.corners[j - 1]);

            if (sqrDistance >= distanceToClosestPlayer)
                continue;

            distanceToClosestPlayer = sqrDistance;
            closestPlayer = _closestPlayers[i].transform;
        }

        return closestPlayer;
    }
}
