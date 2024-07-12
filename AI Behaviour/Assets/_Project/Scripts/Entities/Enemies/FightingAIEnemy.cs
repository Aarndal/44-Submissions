using System;
using UnityEngine;

public class FightingAIEnemy : AIEnemy, ICanAttack, ICanDie
{
    public event Action<ICanAttack, IAmDamageable> HasAttacked;

    [SerializeField]
    private Weapon _weapon;
    [SerializeField]
    private Collider _attackCollider;

    //[SerializeField]
    //private int _attackDelay = 500;

    public Weapon Weapon { get => _weapon; protected set => _weapon = value; }

    protected override void Awake()
    {
        base.Awake();

        if (_attackCollider == null)
            Debug.LogWarning($"{name} has no assigned TriggerCollider");

        if (_weapon == null)
            Debug.LogWarning($"{name} has no assigned Weapon.");
    }

    private void OnEnable()
    {
        _animationEventBroadcaster.AnimationEventTriggered += OnAttackAnimation;
    }

    private void Start()
    {
        _attackCollider.isTrigger = true;
        _attackCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.TryGetComponent<IAmDamageable>(out var target))
            return;

        if (collision.CompareTag("Player"))
        {
            //Debug.LogFormat($"{name} is attacking {collision.gameObject.tag}!");
            //await Task.Delay(_attackDelay);
            Attack(target);
            HasAttacked?.Invoke(this, target);
            Debug.LogFormat($"{name} has attacked {collision.gameObject.tag}!");
        }
    }

    private void OnDisable()
    {
        _animationEventBroadcaster.AnimationEventTriggered -= OnAttackAnimation;
    }

    private void OnAttackAnimation(AnimationEvent args)
    {
        if (args.stringParameter == "Attack")
        {
            if (args.intParameter == 0)
                _attackCollider.enabled = false;
            else
                _attackCollider.enabled = true;
        }
    }

    public virtual void Attack(IAmDamageable target)
    {
        if (Weapon == null)
        {
            Debug.LogWarning($"{name} has no assigned Weapon.");
            return;
        }

        Weapon.Attack(target);
    }

    public virtual void Die()
    {
        Destroy(this.gameObject);
    }

}
