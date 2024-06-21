using System.Collections;
using UnityEngine;

public class RegeneratingHealth : LimitedHealth
{
    [SerializeField]
    private int _HealPerSecond = 1;
    [SerializeField]
    private int _HealDelay = 1;

    public override void IncreaseCurrentHealth(int amount)
    {
        StopAllCoroutines();
        StartCoroutine(Regenerate(amount));
    }

    private IEnumerator Regenerate (int healValue)
    {
        int remainingHeal = Mathf.Clamp(healValue, 0, _maxHealth - _currentHealth);

        while (remainingHeal > 0)
        {
            yield return new WaitForSeconds(_HealDelay);
            _currentHealth += _HealPerSecond;
            remainingHeal -= _HealPerSecond;
        }
    }
}
