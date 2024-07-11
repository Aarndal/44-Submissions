using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class SearchAIEnemyState : AIEnemyState
{
    private bool _isSearchingLastKnownPosition = false;
    private bool _isMovingToCurrentPosition = false;
    private Vector3 _lastKnownPosition;

    private NavMeshPath _tempNavMeshPath;

    public bool LostTarget { get; private set; }

    public SearchAIEnemyState(StateMachine fsm, AIEnemy entity, TargetProvider targetProvider) : base(fsm, entity, targetProvider) { }

    public async override Task OnEnter()
    {
        LostTarget = false;
        _isMovingToCurrentPosition = false;

        AIEnemy.AutonomousMover.NavMeshAgent.autoBraking = false;
        AIEnemy.AutonomousMover.NavMeshAgent.autoRepath = true;
        AIEnemy.AutonomousMover.NavMeshAgent.isStopped = false;

        AIEnemy.AutonomousMover.NavMeshAgent.speed = 3.0f;

        AIEnemy.AutonomousMover.NavMeshAgent.ResetPath();
        
        await Task.Yield();

        _lastKnownPosition = TargetProvider.Target.position;
        
        AIEnemy.AutonomousMover.MoveTo(TargetProvider);
        _isSearchingLastKnownPosition = true;

        AIEnemy.Animator.Play("Base Layer.Run");
    }

    public override void OnUpdate()
    {
        if (_isSearchingLastKnownPosition)
        {
            if (AIEnemy.AutonomousMover.NavMeshAgent.isPathStale || !AIEnemy.AutonomousMover.NavMeshAgent.hasPath)
                AIEnemy.AutonomousMover.NavMeshAgent.SetDestination(_lastKnownPosition);

            if (AIEnemy.AutonomousMover.ReachedTarget)
            {
                _isSearchingLastKnownPosition = false;

                FollowPathToCurrentTargetPosition();
                _isMovingToCurrentPosition = true;
            }
        }

        if (_isMovingToCurrentPosition && !LostTarget)
            if (AIEnemy.AutonomousMover.ReachedTarget || AIEnemy.AutonomousMover.NavMeshAgent.isPathStale || !AIEnemy.AutonomousMover.NavMeshAgent.hasPath)
            {
                _isMovingToCurrentPosition = false;
                LostTarget = true;
            }
    }

    public async override Task OnExit()
    {
        //LostTarget = false;
        _isSearchingLastKnownPosition = false;
        _isMovingToCurrentPosition = false;

        await Task.Yield();
    }

    private void FollowPathToCurrentTargetPosition()
    {
        _tempNavMeshPath = new();

        if (AIEnemy.AutonomousMover.NavMeshAgent.CalculatePath(TargetProvider.Target.position, _tempNavMeshPath))
        {
            if (_tempNavMeshPath.corners.Length == 1)
                AIEnemy.AutonomousMover.NavMeshAgent.SetDestination(_tempNavMeshPath.corners[0]);

            if (_tempNavMeshPath.corners.Length > 1)
                AIEnemy.AutonomousMover.NavMeshAgent.SetDestination(_tempNavMeshPath.corners[1]);

            //if (_tempNavMeshPath.corners.Length >= 1)
            //    AIEnemy.AutonomousMover.NavMeshAgent.SetDestination(_tempNavMeshPath.corners[^1]);
        }
        else
            LostTarget = true;
    }
}