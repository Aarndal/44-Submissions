

using System;
using UnityEngine;
using UnityEngine.AI;

public class CircleAIEnemyState : AIEnemyState
{
    private float _prevAngularSpeed;
    private float _prevStoppingDistance;
    private float _circleRadius = 2.0f; // Radius of the circle around the target
    private float _currentAngle = 0.0f; // Current angle in radians
    private float _angleIncrement = Mathf.PI / 8; // Angle increment for each waypoint (45 degrees in this case)

    public CircleAIEnemyState(AIEnemy entity, TargetProvider targetProvider) : base(entity, targetProvider)
    {
    }

    public override void OnEnter()
    {
        _prevAngularSpeed = AIEnemy.AutonomousMover.NavMeshAgent.angularSpeed;
        _prevStoppingDistance = AIEnemy.AutonomousMover.MinDistanceToTarget;

        AIEnemy.AutonomousMover.NavMeshAgent.isStopped = false;
        AIEnemy.AutonomousMover.NavMeshAgent.speed = 2.0f;
        AIEnemy.AutonomousMover.NavMeshAgent.angularSpeed = 50f;
        AIEnemy.AutonomousMover.MinDistanceToTarget = 0.0f;
        AIEnemy.AutonomousMover.NavMeshAgent.SetDestination(GenerateRandomWaypoint());
    }

    public override void OnFixedUpdate()
    {
        if (AIEnemy.transform.position == AIEnemy.AutonomousMover.NavMeshAgent.pathEndPosition)
            AIEnemy.AutonomousMover.NavMeshAgent.SetDestination(GenerateRandomWaypoint());
    }

    public override void OnUpdate()
    {
        AIEnemy.Animator.Play("Base Layer.Walk");
    }

    public override void OnExit()
    {
        AIEnemy.AutonomousMover.NavMeshAgent.angularSpeed = _prevAngularSpeed;
        AIEnemy.AutonomousMover.MinDistanceToTarget = _prevStoppingDistance;
    }

    private Vector3 GenerateRandomWaypoint()
    {
        //return TargetProvider.Target.position + Vector3.Cross((TargetProvider.Target.position - AIEnemy.transform.position), Vector3.up);
        //NavMeshPath test = new();
        //NavMesh.CalculatePath();

        Vector3 targetPosition = TargetProvider.Target.position;
        float x = Mathf.Cos(_currentAngle) * _circleRadius + targetPosition.x;
        float z = Mathf.Sin(_currentAngle) * _circleRadius + targetPosition.z;
        _currentAngle += _angleIncrement; // Increment the angle for the next waypoint

        return new Vector3(x, targetPosition.y, z);
    }
}
