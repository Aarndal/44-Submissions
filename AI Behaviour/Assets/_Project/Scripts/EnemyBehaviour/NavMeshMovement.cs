using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshMovement : MonoBehaviour, IAmAutonomousMovable
{
    public NavMeshAgent NavMeshAgent { get; private set; }

    public bool IsTargetReached { get; private set; }

    public float DistanceToTarget => NavMeshAgent.remainingDistance;

    public float MaxDistanceToTarget { get => NavMeshAgent.stoppingDistance; set => NavMeshAgent.stoppingDistance = value; }

    public Vector3 CurrentPosition => transform.position;

    private void Awake()
    {
        NavMeshAgent = NavMeshAgent != null ? NavMeshAgent : GetComponent<NavMeshAgent>();
    }

    public void Move(TargetProvider targetProvider)
    {
        if(!targetProvider.HasTarget)
            NavMeshAgent.isStopped = true;
        
        if (targetProvider.HasTarget)
        {
            NavMeshAgent.isStopped = false;
            NavMeshAgent.SetDestination(targetProvider.Target.position);

            //IsTargetReached = NavMeshAgent.ReachedDestinationOrGaveUp();
        }

    }
}
