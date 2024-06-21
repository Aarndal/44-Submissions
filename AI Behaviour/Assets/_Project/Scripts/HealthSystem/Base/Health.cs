using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Responsible for player health management.
/// </summary>
public abstract class Health : MonoBehaviour
{
    public UnityEvent ZeroHealth;
    
    [Header("Values")]
    [SerializeField, Min(1)]
    protected int _currentHealth;
    
    public int CurrentHealth => _currentHealth;

    public abstract void IncreaseCurrentHealth(int amount);

    public virtual void DecreaseCurrentHealth(int amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - amount, 0, int.MaxValue);

        if(_currentHealth <= 0)
            ZeroHealth.Invoke();
    }
}
