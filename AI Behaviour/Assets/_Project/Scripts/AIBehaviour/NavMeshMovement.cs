using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
[RequireComponent(typeof(NavMeshAgent))]
public sealed class NavMeshMovement : MonoBehaviour, IAmAutonomousMovable
{
    public NavMeshAgent NavMeshAgent { get; private set; }

    public bool ReachedTarget { get; private set; }

    public float DistanceToTarget => NavMeshAgent.remainingDistance;

    public float MaxDistanceToTarget { get => NavMeshAgent.stoppingDistance; set => NavMeshAgent.stoppingDistance = value; }

    public Vector3 CurrentPosition => transform.position;

    private void Awake()
    {
        NavMeshAgent = NavMeshAgent != null ? NavMeshAgent : GetComponent<NavMeshAgent>();
    }

    public void MoveTo(TargetProvider targetProvider)
    {
        if(!targetProvider.HasTarget)
            NavMeshAgent.isStopped = true;
        
        if (targetProvider.HasTarget)
        {
            NavMeshAgent.isStopped = false;
            NavMeshAgent.SetDestination(targetProvider.Target.position);

            ReachedTarget = DistanceToTarget <= MaxDistanceToTarget;
        }
    }
}
