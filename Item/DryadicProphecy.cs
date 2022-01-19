using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryadicProphecy : KillItem
{
    public GameObject projectile;
    public float spawnDistance;
    public float healAmount;
    public float spawnChance;
    public int maxSpawns;

    protected override void OnKill(object sender, System.EventArgs e)
    {
        for(int i = 0; i <maxSpawns; i++)
        {
            if (Random.Range(0, 1f) < spawnChance)
            {
                Instantiate(projectile, owner.transform.position + (Vector3)Random.insideUnitCircle * spawnDistance, Quaternion.identity)
                    .GetComponent<IProjectile>().Fire(owner.target - (Vector2)owner.transform.position, owner);
                owner.Heal(healAmount);
            }
        }
    }

}
