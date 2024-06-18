using UnityEngine;

//[CreateAssetMenu(fileName = "RoamState", menuName = "AI/States/RoamState")]
public class RoamState : State
{
    private NavMeshMovement _autonomousMover;
    //private TargetProvider _target;

    public RoamState(Entity entity, NavMeshMovement autonomousMover) : base(entity)
    {
        _entity = entity;
        _autonomousMover = autonomousMover;
        //_target = target;
    }

    public override void OnEnter()
    {
        _autonomousMover.NavMeshAgent.isStopped = false;
    }
}
