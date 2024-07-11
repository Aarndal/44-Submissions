using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
[RequireComponent(typeof(NavMeshAgent))]
public sealed class NavMeshMovement : MonoBehaviour, IAmAutonomousMovable
{
    private bool _reachedTarget;

    public NavMeshAgent NavMeshAgent { get; private set; }

    public bool ReachedTarget
    {
        get => _reachedTarget;
        set
        {
            if (!NavMeshAgent.pathPending &&
                (DistanceToTarget <= MinDistanceToTarget || NavMeshAgent.transform.position == NavMeshAgent.pathEndPosition))
                _reachedTarget = true;
            else
                _reachedTarget = false;
        }
    }

    public float DistanceToTarget => NavMeshAgent.remainingDistance;

    public float MinDistanceToTarget { get => NavMeshAgent.stoppingDistance; set => NavMeshAgent.stoppingDistance = value; }

    public Vector3 InitialPosition { get; private set; }

    public Vector3 CurrentPosition => this.transform.position;

    private void Awake()
        => NavMeshAgent = NavMeshAgent != null ? NavMeshAgent : GetComponent<NavMeshAgent>();

    private void OnEnable()
        => InitialPosition = this.transform.position;

    public void MoveTo(TargetProvider targetProvider)
    {
        if (!targetProvider.HasTarget)
        {
            //NavMeshAgent.isStopped = true;
            Debug.Log($"{gameObject.name} has no target");
        }

        if (targetProvider.HasTarget)
        {
            NavMeshAgent.isStopped = false;
            NavMeshAgent.SetDestination(targetProvider.Target.position);
        }
    }
}
