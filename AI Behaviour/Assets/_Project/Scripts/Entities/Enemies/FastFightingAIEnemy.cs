using UnityEngine;

public class FastFightingAIEnemy : FightingAIEnemy
{
    public int EvadeChance { get ; protected set; }

    public override void TakeDamage(int damage)
    {
        if (EvadeChance > 100 || EvadeChance < 1)
            EvadeChance = 20;

        bool evaded = Random.Range(1, 101) == EvadeChance;

        if (evaded == true)
            Debug.LogFormat($"{name} evaded the attack.");

        if (evaded == false)
            base.TakeDamage(damage);
    }
}
