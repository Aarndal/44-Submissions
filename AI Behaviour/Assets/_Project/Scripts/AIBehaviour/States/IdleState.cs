using System;
using Unity.VisualScripting;
using UnityEngine;

//[CreateAssetMenu(fileName = "IdleState", menuName = "AI/States/IdleState")]
public class IdleState : State
{
    public event Action<bool> IdleTimeIsUp;

    private float _timer, _idleTime;
    private NavMeshMovement _autonomousMover;

    public bool TimeIsUp { get; private set; }

    public IdleState(Entity entity, NavMeshMovement autonomousMover, float idleTime) : base(entity)
    {
        _entity = entity;
        _autonomousMover = autonomousMover;
        _idleTime = idleTime;
        _timer = _idleTime;
    }

    public override void OnEnter()
    {
        _timer = _idleTime; 
        TimeIsUp = false;
        IdleTimeIsUp?.Invoke(false);
        _autonomousMover.NavMeshAgent.isStopped = true;
    }

    public override void OnUpdate()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            TimeIsUp = true;
            IdleTimeIsUp?.Invoke(true);
        }
    }

    public override void OnExit()
    {
        
    }
}
