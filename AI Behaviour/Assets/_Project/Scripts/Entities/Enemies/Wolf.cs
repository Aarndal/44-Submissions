using System;
using UnityEngine;

public class Wolf : FastFightingAIEnemy
{
    public Func<bool> RoamCondition, IdleCondition, ChaseCondition;

    [SerializeField]
    private RandomWayPointTargetProvider _wayPointProvider;
    [SerializeField]
    private PlayerTargetProvider _playerProvider;
    [SerializeField]
    private LineOfSightChecker _lineOfSightChecker;

    [Header("Variables")]
    [SerializeField]
    private float _idleTime = 10f;
    [SerializeField]
    private float _roamRadius = 20f;

    
    private StateMachine _myFSM;
    private IdleAIEnemyState _idle;
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
        _idle = new IdleAIEnemyState(this, _idleTime);
        _roam = new RoamAIEnemyState(this, _roamRadius);
        _chase = new ChaseAIEnemyState(this, _playerProvider);
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
