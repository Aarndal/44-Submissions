using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class SearchAIEnemyState : AIEnemyState
{
    private bool _isSearchingLastKnownPosition = false;
    private bool _isMovingToCurrentPosition = false;
    private Vector3 _lastKnownPosition;

    private NavMeshPath _tempNavMeshPath;
    private float _prevStoppingDistance;

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

        _prevStoppingDistance = AIEnemy.AutonomousMover.MinDistanceToTarget;
        AIEnemy.AutonomousMover.MinDistanceToTarget = 0.5f;

        AIEnemy.AutonomousMover.NavMeshAgent.ResetPath();

        await Task.Yield();

        _lastKnownPosition = TargetProvider.Target.position;
        AIEnemy.AutonomousMover.MoveTo(TargetProvider);

        await Task.Yield();

        _isSearchingLastKnownPosition = true;

        AIEnemy.Animator.Play("Base Layer.Run");
    }

    public override void OnFixedUpdate()
    {
        if (_isSearchingLastKnownPosition)
            MoveToLastKnownPosition();

        if (_isMovingToCurrentPosition && !LostTarget)
            if (AIEnemy.AutonomousMover.ReachedTarget || AIEnemy.AutonomousMover.NavMeshAgent.isPathStale || !AIEnemy.AutonomousMover.NavMeshAgent.hasPath)
            {
                //Debug.LogFormat("Reached corner.");
                _isMovingToCurrentPosition = false;
                LostTarget = true;
            }
    }

    public async override Task OnExit()
    {
        LostTarget = false;
        _isSearchingLastKnownPosition = false;
        _isMovingToCurrentPosition = false;

        AIEnemy.AutonomousMover.MinDistanceToTarget = _prevStoppingDistance;
        await Task.Yield();
    }

    private async void MoveToLastKnownPosition()
    {
        if (AIEnemy.AutonomousMover.NavMeshAgent.isPathStale || !AIEnemy.AutonomousMover.NavMeshAgent.hasPath)
            AIEnemy.AutonomousMover.NavMeshAgent.SetDestination(_lastKnownPosition);

        if (AIEnemy.AutonomousMover.ReachedTarget)
        {
            //Debug.LogFormat("Reached Last Known Position!");

            _tempNavMeshPath = new();

            await SetPathToCurrentTargetPosition();

            _isSearchingLastKnownPosition = false;
            _isMovingToCurrentPosition = true;
        }
    }

    private async Task SetPathToCurrentTargetPosition()
    {
        if (AIEnemy.AutonomousMover.NavMeshAgent.CalculatePath(TargetProvider.Target.position, _tempNavMeshPath))
        {
            if (_tempNavMeshPath.corners.Length == 1)
                AIEnemy.AutonomousMover.NavMeshAgent.SetDestination(_tempNavMeshPath.corners[0]);

            if (_tempNavMeshPath.corners.Length > 1)
                AIEnemy.AutonomousMover.NavMeshAgent.SetDestination(_tempNavMeshPath.corners[1]);

            //if (_tempNavMeshPath.corners.Length >= 1)
            //    AIEnemy.AutonomousMover.NavMeshAgent.SetDestination(_tempNavMeshPath.corners[^1]);

            //Debug.LogFormat("Moving towards corner.");
            await Task.Yield();
        }
        else
        {
            LostTarget = true;
            //Debug.LogFormat("Lost Target because couldn't find path to corner.");
        }
    }
}