using System;
using UnityEngine;

public class Wolf : FastFightingAIEnemy
{
    public Func<bool> RoamCondition, IdleCondition, ChaseCondition, SearchCondition, AttackCondition, CircleCondition, FleeCondition;

    //[SerializeField]
    //private RandomWayPointTargetProvider _wayPointProvider;
    [SerializeField]
    private PlayerTargetProvider _playerProvider;
    [SerializeField]
    private LineOfSightChecker _lineOfSightChecker;

    [Header("Variables")]
    [SerializeField]
    private float _idleTime = 5f;
    [SerializeField]
    private float _roamRadius = 20f;
    [SerializeField]
    private float _fleeDistance = 100f;
    [SerializeField, Range(2.0f, 5.0f)]
    private float _attackRange = 4.0f;
    [SerializeField, Range(1, 100)]
    private int _evadeChance = 20;

    private StateMachine _myFSM;
    private State _roam, _chase, _attack, _circle;
    private IdleAIEnemyState _idle;
    private SearchAIEnemyState _search;
    private FleeAIEnemyState _flee;
    private Transition _toIdle, _toRoam, _toChase, _toSearch, _toAttack, _toCircle, _toFlee;

    protected override void Awake()
    {
        base.Awake();
        
        _myFSM = new();

        InitializeStates();
        InitializeConditions();
        InitializeTransitions();

        _myFSM.SetInitialState(_idle);
        _idle.AddTransition(_toRoam);
        _idle.AddTransition(_toChase);

        _myFSM.AddState(_roam);
        _roam.AddTransition(_toChase);

        _myFSM.AddState(_search);
        _search.AddTransition(_toChase);
        _search.AddTransition(_toIdle);

        _myFSM.AddState(_chase);
        _chase.AddTransition(_toSearch);
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

    protected override void OnEnable()
    {
        base.OnEnable();
        //    _idle.IdleTimeIsUp += OnIdleTimeIsUp;
        //    _lineOfSightChecker.GainedSight += OnGainedSight;
    }

    protected override void Start()
    {
        base.Start();

        this.AutonomousMover.NavMeshAgent.enabled = true;
        this.AutonomousMover.MinDistanceToTarget = AutonomousMover.MinDistanceToTarget >= _attackRange ? _attackRange - 0.1f : AutonomousMover.MinDistanceToTarget;

        this.EvadeChance = _evadeChance;

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

    protected override void OnDisable()
    {
        base.OnDisable();
        //    _lineOfSightChecker.GainedSight -= OnGainedSight;
        //    _idle.IdleTimeIsUp -= OnIdleTimeIsUp;
    }

    //private void OnIdleTimeIsUp(bool ctx) => _toRoam.Condition = () => ctx;
    //private void OnGainedSight(bool ctx) => _toChase.Condition = () => ctx;

    private void InitializeStates()
    {
        _idle = new IdleAIEnemyState(_myFSM, this, _idleTime);
        _roam = new RoamAIEnemyState(_myFSM, this, _roamRadius);
        _chase = new ChaseAIEnemyState(_myFSM, this, _playerProvider);
        _search = new SearchAIEnemyState(_myFSM, this, _playerProvider);
        _attack = new AttackAIEnemyState(_myFSM, this, _playerProvider);
        _circle = new CircleAIEnemyState(_myFSM, this, _playerProvider);
        _flee = new FleeAIEnemyState(_myFSM, this, _playerProvider);
    }

    private void InitializeConditions()
    {
        IdleCondition = () =>
        _search.LostTarget ||
        !_playerProvider.HasTarget && !_lineOfSightChecker.TargetInSight ||
        _myFSM.CurrentState == _flee && _playerProvider.SqrDistanceToTarget >= _flee.FleeDistance * _flee.FleeDistance;

        RoamCondition = () => _idle.TimeIsUp;
        //RoamCondition = () => false;
        
        ChaseCondition = () => (_lineOfSightChecker.TargetInSight || _myFSM.CurrentState == _circle || _myFSM.CurrentState == _attack) && _playerProvider.SqrDistanceToTarget > _attackRange * _attackRange;
        //ChaseCondition = () => false;

        SearchCondition = () => _myFSM.CurrentState == _chase && !_lineOfSightChecker.TargetInSight;

        //AttackCondition = () => (_lineOfSightChecker.TargetInSight) && _playerProvider.SqrDistanceToTarget <= _attackRange * _attackRange && _playerProvider.TargetIsFleeing;
        AttackCondition = () => (_lineOfSightChecker.TargetInSight) && _playerProvider.SqrDistanceToTarget <= _attackRange * _attackRange;
        //AttackCondition = () => false;
        
        //CircleCondition = () => (_lineOfSightChecker.TargetInSight) && _playerProvider.SqrDistanceToTarget <= _attackRange * _attackRange && !_playerProvider.TargetIsFleeing;
        CircleCondition = () => false;
        
        FleeCondition = () => false;
    }

    private void InitializeTransitions()
    {
        _toIdle = new Transition("Transition to Idle", IdleCondition, _idle);
        _toRoam = new Transition("Transition to Roam", RoamCondition, _roam);
        _toChase = new Transition("Transition to Chase", ChaseCondition, _chase);
        _toSearch = new Transition("Transition to Search", SearchCondition, _search);
        _toAttack = new Transition("Transition to Attack", AttackCondition, _attack);
        _toCircle = new Transition("Transition to Circle", CircleCondition, _circle);
        _toFlee = new Transition("Transition to Flee", FleeCondition, _flee);
    }
}
