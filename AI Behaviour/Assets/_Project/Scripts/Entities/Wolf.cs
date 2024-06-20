using System;
using UnityEngine;

public class Wolf : AIEnemy
{
    public Func<bool> RoamCondition, IdleCondition, ChaseCondition;

    [SerializeField]
    private TargetProvider _targetProvider;
    [SerializeField]
    private float _idleTime = 10f;

    private LineOfSightChecker _lineOfSightChecker;
    
    private StateMachine _myFSM;
    private IdleState _idle;
    private State _roam, _chase;
    private Transition _toRoam, _toChase, _toIdle;

    protected override void Awake()
    {
        base.Awake();
        
        _lineOfSightChecker = GetComponentInChildren<LineOfSightChecker>();

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

    //private void OnEnable()
    //{
    //    _idle.IdleTimeIsUp += OnIdleTimeIsUp;
    //    _playerTargetProvider.PlayerInSight += OnPlayerInSight;
    //}

    private void Start()
    {
        Debug.LogWarning("Start State: " + _myFSM.CurrentState);
    }
    
    private void FixedUpdate()
    {
        _myFSM.OnFixedUpdate();
    }

    private void Update()
    {
        _myFSM.OnUpdate();
    }

    private void LateUpdate()
    {
        _myFSM.OnLateUpdate();
    }

    //private void OnDisable()
    //{
    //    _playerTargetProvider.PlayerInSight -= OnPlayerInSight;
    //    _idle.IdleTimeIsUp -= OnIdleTimeIsUp;
    //}

    //private void OnIdleTimeIsUp(bool ctx) => _toRoam.Condition = () => ctx;
    //private void OnPlayerInSight(bool ctx) => _toChase.Condition = () => ctx;

    private void InitializeStates()
    {
        _idle = new IdleState(this, _idleTime);
        _roam = new RoamState(this);
        _chase = new ChaseState(this, _targetProvider);
    }

    private void InitializeConditions()
    {
        RoamCondition = () => _idle.TimeIsUp;
        ChaseCondition = () => _lineOfSightChecker.TargetInSight;
        IdleCondition = () => _autonomousMover.ReachedTarget || !_lineOfSightChecker.TargetInSight;
    }

    private void InitializeTransitions()
    {
        _toRoam = new Transition("Transition to Roam", RoamCondition, _roam);
        _toChase = new Transition("Transition to Chase", ChaseCondition, _chase);
        _toIdle = new Transition("Transition to Idle", IdleCondition, _idle);
    }
}
