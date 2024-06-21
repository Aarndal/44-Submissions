using UnityEngine;

public class LimitedHealth : Health
{
    [SerializeField, Min(1)]
    protected int _maxHealth = 10;

    public int MaxHealth => _maxHealth;

    public override void IncreaseCurrentHealth(int amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, _maxHealth);
        Debug.LogFormat($"{name} Health: {CurrentHealth} / {MaxHealth}");
    }

    public override void DecreaseCurrentHealth(int amount)
    {
        base.DecreaseCurrentHealth(amount);
        Debug.LogFormat($"{name} Health: {CurrentHealth} / {MaxHealth}");
    }
}
