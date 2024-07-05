using System;
using UnityEngine;

public class Wolf : FastFightingAIEnemy
{
    public Func<bool> RoamCondition, IdleCondition, ChaseCondition, AttackCondition, CircleCondition, FleeCondition;

    [SerializeField]
    private RandomWayPointTargetProvider _wayPointProvider;
    [SerializeField]
    private PlayerTargetProvider _playerProvider;
    [SerializeField]
    private LineOfSightChecker _lineOfSightChecker;

    [Header("Variables")]
    [SerializeField]
    private float _idleTime = 5f;
    [SerializeField]
    private float _roamRadius = 20f;
    [SerializeField, Range(2.0f, 5.0f)]
    private float _attackRange = 4.0f;
    [SerializeField]
    private float _fleeDistance = 100f;

    private StateMachine _myFSM;
    private IdleAIEnemyState _idle;
    private ChaseAIEnemyState _chase;
    private FleeAIEnemyState _flee;
    private State _roam, _attack, _circle;
    private Transition _toRoam, _toChase, _toIdle, _toAttack, _toCircle, _toFlee;

    protected override void Awake()
    {
        base.Awake();

        InitializeStates();
        InitializeConditions();
        InitializeTransitions();

        _myFSM = new(_idle);
        _idle.AddTransition(_toRoam);
        _idle.AddTransition(_toChase);

        _myFSM.AddState(_roam);
        _roam.AddTransition(_toChase);

        _myFSM.AddState(_chase);
        _chase.AddTransition(_toIdle);
        _chase.AddTransition(_toAttack);
        _chase.AddTransition(_toCircle);

        _myFSM.AddState(_attack);
        _attack.AddTransition(_toChase);
        _attack.AddTransition(_toCircle);

        _myFSM.AddState(_circle);
        _circle.AddTransition(_toChase);
        _circle.AddTransition(_toAttack);

        _myFSM.AddState(_flee);
        _myFSM.AddAnyTransition(_toFlee);
        _flee.AddTransition(_toIdle);
    }

    private void OnEnable()
    {
        //    _idle.IdleTimeIsUp += OnIdleTimeIsUp;
        //    _playerTargetProvider.PlayerInSight += OnPlayerInSight;
    }

    private void Start()
    {
        AutonomousMover.NavMeshAgent.enabled = true;

        AutonomousMover.MinDistanceToTarget = AutonomousMover.MinDistanceToTarget >= _attackRange ? _attackRange - 0.1f : AutonomousMover.MinDistanceToTarget;

        _flee.FleeDistance = _fleeDistance;

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

    private void OnDisable()
    {
        //    _playerTargetProvider.PlayerInSight -= OnPlayerInSight;
        //    _idle.IdleTimeIsUp -= OnIdleTimeIsUp;
    }

    //private void OnIdleTimeIsUp(bool ctx) => _toRoam.Condition = () => ctx;
    //private void OnPlayerInSight(bool ctx) => _toChase.Condition = () => ctx;

    private void InitializeStates()
    {
        _idle = new IdleAIEnemyState(this, _idleTime);
        _roam = new RoamAIEnemyState(this, _roamRadius);
        _chase = new ChaseAIEnemyState(this, _playerProvider, _lineOfSightChecker);
        _attack = new AttackAIEnemyState(this, _playerProvider);
        _circle = new CircleAIEnemyState(this, _playerProvider);
        _flee = new FleeAIEnemyState(this, _playerProvider);
    }

    private void InitializeConditions()
    {
        IdleCondition = () =>
        _myFSM.CurrentState == _chase && !_lineOfSightChecker.TargetInSight ||
        _myFSM.CurrentState == _flee && _playerProvider.SqrDistanceToTarget >= _flee.FleeDistance * _flee.FleeDistance;

        RoamCondition = () => _idle.TimeIsUp;

        ChaseCondition = () => (_lineOfSightChecker.TargetInSight || _myFSM.CurrentState == _circle || _myFSM.CurrentState == _attack) && _playerProvider.SqrDistanceToTarget > _attackRange * _attackRange;

        AttackCondition = () => (_lineOfSightChecker.TargetInSight) && _playerProvider.SqrDistanceToTarget <= _attackRange * _attackRange && _playerProvider.TargetIsFleeing;
        CircleCondition = () => (_lineOfSightChecker.TargetInSight) && _playerProvider.SqrDistanceToTarget <= _attackRange * _attackRange && !_playerProvider.TargetIsFleeing;
        FleeCondition = () => false;
    }

    private void InitializeTransitions()
    {
        _toIdle = new Transition("Transition to Idle", IdleCondition, _idle);
        _toRoam = new Transition("Transition to Roam", RoamCondition, _roam);
        _toChase = new Transition("Transition to Chase", ChaseCondition, _chase);
        _toAttack = new Transition("Transition to Attack", AttackCondition, _attack);
        _toCircle = new Transition("Transition to Circle", CircleCondition, _circle);
        _toFlee = new Transition("Transition to Flee", FleeCondition, _flee);
    }
}
