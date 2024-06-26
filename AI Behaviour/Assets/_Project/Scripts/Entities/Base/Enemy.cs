using UnityEngine;

public class Enemy : Entity, IAmDamageable
{
    [Header("References")]
    [SerializeField]
    protected Health _health;

    public virtual void TakeDamage(int damage)
    {
        _health.DecreaseCurrentHealth(damage);
    }
}
