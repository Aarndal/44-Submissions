using UnityEngine;

//[CreateAssetMenu(fileName = "ChaseState", menuName = "AI/States/ChaseState")]
public class ChaseState : State
{
    private NavMeshMovement _autonomousMover;
    private TargetProvider _target;

    public NavMeshMovement AutonomousMover => _autonomousMover;

    public ChaseState(Entity entity, NavMeshMovement autonomousMover, TargetProvider target) : base(entity)
    {
        _entity = entity;
        _autonomousMover = autonomousMover;
        _target = target;
    }

    public override void OnEnter()
    {
        AutonomousMover.NavMeshAgent.enabled = true;
        AutonomousMover.NavMeshAgent.isStopped = false;
    }

    public override void OnUpdate()
    {
        AutonomousMover.MoveTo(_target);
    }
}
