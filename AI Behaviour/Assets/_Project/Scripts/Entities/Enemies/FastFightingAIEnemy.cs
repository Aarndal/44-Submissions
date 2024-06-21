using UnityEngine;

public class FastFightingAIEnemy : FightingAIEnemy
{
    private int _evadeChance = 20;

    public int EvadeChance { get => _evadeChance; set => _evadeChance = value; }

    public override void TakeDamage(int damage)
    {
        bool evaded = Random.Range(1, 101) == EvadeChance;

        if (evaded == true)
            Debug.LogFormat($"{name} evaded the attack.");

        if (evaded == false)
            base.TakeDamage(damage);
    }
}
