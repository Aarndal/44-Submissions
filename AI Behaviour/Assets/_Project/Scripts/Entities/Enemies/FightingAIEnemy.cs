using System;
using UnityEngine;

public class FightingAIEnemy : AIEnemy, ICanAttack, ICanDie
{
    public event Action<ICanAttack, IAmDamageable> HasAttacked;

    [SerializeField]
    private Weapon _weapon;

    public Weapon Weapon { get => _weapon; protected set => _weapon = value; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<IAmDamageable>(out var target))
            return;

        if (collision.CompareTag("Player"))
        {
            Debug.LogFormat($"Test: {name} attacked Player!");
            Attack(target);
            HasAttacked?.Invoke(this, target);
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
