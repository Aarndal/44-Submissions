using UnityEngine;

public class Player : Entity, IAmDamageable
{
    public void TakeDamage(int damage)
    {
        Debug.LogFormat($"{name} took {damage} damage.");
    }
}
