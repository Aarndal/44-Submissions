using UnityEngine;

public class Enemy : Entity, IAmDamageable
{
    [Header("References")]
    [SerializeField]
    private Health _health;

    public virtual void TakeDamage(int damage)
    {
        _health.DecreaseCurrentHealth(damage);
    }
}
