using System.Threading.Tasks;
using UnityEngine;

public class CircleAIEnemyState : AIEnemyState
{
    private float _prevAngularSpeed;
    private float _prevStoppingDistance;
    private float _circleRadius = 4.0f; // Radius of the circle around the target
    private float _currentAngle = Mathf.PI / 6; // Current angle in radians
    private float _angleIncrement = Mathf.PI / 6; // Angle increment for each waypoint (15 degrees in this case)

    public CircleAIEnemyState(StateMachine fsm, AIEnemy entity, TargetProvider targetProvider) : base(fsm, entity, targetProvider)
    {
    }

    public async override Task OnEnter()
    {
        _prevAngularSpeed = AIEnemy.AutonomousMover.NavMeshAgent.angularSpeed;
        _prevStoppingDistance = AIEnemy.AutonomousMover.MinDistanceToTarget;

        AIEnemy.AutonomousMover.NavMeshAgent.autoRepath = true;
        AIEnemy.AutonomousMover.NavMeshAgent.autoBraking = false;

        AIEnemy.AutonomousMover.NavMeshAgent.isStopped = false;
        AIEnemy.AutonomousMover.NavMeshAgent.ResetPath();

        AIEnemy.AutonomousMover.NavMeshAgent.speed = 3.0f;
        AIEnemy.AutonomousMover.NavMeshAgent.angularSpeed = 800f;
        AIEnemy.AutonomousMover.MinDistanceToTarget = 0.0f;

        await Task.Yield();

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

    public async override Task OnExit()
    {
        AIEnemy.AutonomousMover.NavMeshAgent.angularSpeed = _prevAngularSpeed;
        AIEnemy.AutonomousMover.MinDistanceToTarget = _prevStoppingDistance;

        AIEnemy.AutonomousMover.NavMeshAgent.autoBraking = true;

        await Task.Yield();
    }

    private Vector3 GenerateRandomWaypoint()
    {
        //return TargetProvider.Target.position + Vector3.Cross((TargetProvider.Target.position - AIEnemy.transform.position), Vector3.up);
        //NavMeshPath test = new();
        //NavMesh.CalculatePath();

        Vector3 targetPosition = TargetProvider.Target.position;
        float x = targetPosition.x + Mathf.Cos(_currentAngle) * _circleRadius;
        float z = targetPosition.z + Mathf.Sin(_currentAngle) * _circleRadius;
        _currentAngle -= _angleIncrement; // Increment the angle for the next waypoint

        return new Vector3(x, targetPosition.y, z);
    }
}
