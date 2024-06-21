using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private int _attackPower;

    public int AttackPower { get => _attackPower; set=> _attackPower = value; }

    public void Attack(IAmDamageable target)
    {
        target.TakeDamage(AttackPower);
    }
}
