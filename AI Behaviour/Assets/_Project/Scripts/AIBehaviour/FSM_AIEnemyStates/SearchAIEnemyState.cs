using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class SearchAIEnemyState : AIEnemyState
{
    private bool _isSearchingLastKnownPosition = false;
    private bool _isMovingToCurrentPosition = false;

    private NavMeshPath _tempNavMeshPath;

    public bool LostTarget { get; private set; }

    public SearchAIEnemyState(StateMachine fsm, AIEnemy entity, TargetProvider targetProvider) : base(fsm, entity, targetProvider) { }

    public async override Task OnEnter()
    {
        LostTarget = false;
        _isMovingToCurrentPosition = false;

        AIEnemy.AutonomousMover.NavMeshAgent.autoBraking = false;

        AIEnemy.AutonomousMover.NavMeshAgent.isStopped = false;
        AIEnemy.AutonomousMover.NavMeshAgent.ResetPath();

        AIEnemy.AutonomousMover.NavMeshAgent.speed = 3.0f;

        //_currentTargetPos = TargetProvider.Target.position;

        await Task.Yield();

        AIEnemy.AutonomousMover.MoveTo(TargetProvider);
        _isSearchingLastKnownPosition = true;

        AIEnemy.Animator.Play("Base Layer.Run");
    }

    public override void OnUpdate()
    {
        if (_isSearchingLastKnownPosition)
            if (!AIEnemy.AutonomousMover.NavMeshAgent.pathPending && AIEnemy.AutonomousMover.DistanceToTarget <= AIEnemy.AutonomousMover.MinDistanceToTarget)
            {
                _isSearchingLastKnownPosition = false;

                MoveToFirstCornerOfPathToCurrentTargetPosition();
                _isMovingToCurrentPosition = true;
            }

        if (_isMovingToCurrentPosition)
            if (!AIEnemy.AutonomousMover.NavMeshAgent.pathPending && AIEnemy.AutonomousMover.DistanceToTarget <= AIEnemy.AutonomousMover.MinDistanceToTarget)
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

    private void MoveToFirstCornerOfPathToCurrentTargetPosition()
    {
        _tempNavMeshPath = new();

        if (AIEnemy.AutonomousMover.NavMeshAgent.CalculatePath(TargetProvider.Target.position, _tempNavMeshPath))
        {
            if (_tempNavMeshPath.corners.Length == 1)
                AIEnemy.AutonomousMover.NavMeshAgent.SetDestination(_tempNavMeshPath.corners[0]);

            if (_tempNavMeshPath.corners.Length > 1)
                AIEnemy.AutonomousMover.NavMeshAgent.SetDestination(_tempNavMeshPath.corners[1]);
        }
        else
            LostTarget = true;
    }
}