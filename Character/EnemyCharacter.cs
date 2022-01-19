using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    public float dropChance;
    public Enemies.EnemyTier tier;

    public override IDamageable.Result Damage(float amount, IDamageable.Type type, Character source)
    {
        health -= amount / (inventory.GetMultiplier("health") * Mathf.Max((float)ProgressionManager.Instance.progressionLevel / 2f - 1f, 1));
        if (Health > 0)
        {
            DamageReceived();
            return IDamageable.Result.Damaged;
        }
        else
        {
            List<Item> choices = new List<Item>();
            if (UnityEngine.Random.Range(0, 1f) <= dropChance)
            {
                foreach (Item i in ProgressionManager.Instance.waves.allItems)
                {
                    if ((int)i.tier == (int)tier)
                    {
                        choices.Add(i);
                    }
                }
                if(choices.Count > 0)
                    source.inventory.EquipItem((choices[UnityEngine.Random.Range(0, choices.Count)].gameObject).GetComponent<Item>());
            }
            DamageReceived();
            SelfKilled();
            Destroy(gameObject);
            return IDamageable.Result.Killed;
        }
    }

}
