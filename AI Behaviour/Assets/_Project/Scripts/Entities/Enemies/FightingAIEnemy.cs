using System;
using System.Threading.Tasks;
using UnityEngine;

public class FightingAIEnemy : AIEnemy, ICanAttack, ICanDie
{
    public event Action<ICanAttack, IAmDamageable> HasAttacked;

    [SerializeField]
    private Weapon _weapon;
    //[SerializeField]
    //private int _attackDelay = 500;

    public Weapon Weapon { get => _weapon; protected set => _weapon = value; }

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
