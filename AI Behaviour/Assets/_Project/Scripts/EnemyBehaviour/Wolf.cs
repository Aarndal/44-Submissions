using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Wolf : Enemy
{
    public Func<bool> RoamCondition, IdleCondition, ChaseCondition;
    
    [SerializeField]
    private PlayerTargetProvider _playerTargetProvider;
    [SerializeField]
    private float _idleTime = 10f;

    private StateMachine _myFSM;
    private IdleState _idle;
    private State _roam, _chase;
    private Transition _toRoam, _toChase, _toIdle;

    protected override void Awake()
    {
        base.Awake();

        InitializeStates();
        InitializeConditions();
        InitializeTransitions();

        _myFSM = new(_idle);

        _myFSM.AddState(_roam);
        _myFSM.AddState(_chase);

        _idle.AddTransition(_toRoam);
        _idle.AddTransition(_toChase);

        _roam.AddTransition(_toChase);

        _chase.AddTransition(_toIdle);
    }

    private void OnEnable()
    {
        _idle.IdleTimeIsUp += OnIdleTimeIsUp;
    }

    private void Start()
    {
        
    }


    private void Update()
    {
        _myFSM.OnUpdate();
    }

    private void OnDisable()
    {
        _idle.IdleTimeIsUp -= OnIdleTimeIsUp;
    }

    private void OnIdleTimeIsUp(bool ctx) => RoamCondition = () => ctx;

    private void InitializeStates()
    {
        _idle = new IdleState(this, AutonomousMover, _idleTime);
        _roam = new RoamState(this, AutonomousMover);
        _chase = new ChaseState(this, AutonomousMover, _playerTargetProvider);
    }

    private void InitializeConditions()
    {
        RoamCondition = () => _idle.TimeIsUp;
        ChaseCondition = () => _playerTargetProvider.HasTarget;
        IdleCondition = () => _autonomousMover.ReachedTarget || !_playerTargetProvider.HasTarget;
    }

    private void InitializeTransitions()
    {
        _toRoam = new Transition(_roam, RoamCondition, "Transition to Roam");
        _toChase = new Transition(_chase, ChaseCondition, "Transition to Chase");
        _toIdle = new Transition(_idle, IdleCondition, "Transition to Idle");
    }
}
