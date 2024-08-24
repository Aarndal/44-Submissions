using System;
using UnityEngine;

public class Player : Entity, IAmDamageable
{
    public event Action<float> HealthChanged;

    private float _health;

    public float Health
    {
        get => _health;
        private set
        {
            if (value < 0)
                value = 0;

            if (_health != value)
            {
                HealthChanged?.Invoke(value - _health);

                _health = value;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.LogFormat($"{name} took {damage} damage.");
    }
}
