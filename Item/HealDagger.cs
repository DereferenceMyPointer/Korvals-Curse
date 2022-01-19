using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealDagger : Item
{
    public float spawnChance;
    public float spawnDistance;
    public GameObject projectile;

    private void Start()
    {
        if(owner)
            owner.OnReceiveDamage += OnReceiveDamage;
    }

    public override void Equip(Character target)
    {
        transform.SetParent(target.inventory.transform);
        transform.localPosition = Vector3.zero;
        if (owner)
        {
            owner.inventory.RefreshItems();
            owner.OnReceiveDamage -= OnReceiveDamage;
        }
        target.inventory.RefreshItems();
        owner = target;
        owner.OnReceiveDamage += OnReceiveDamage;
    }

    private void OnReceiveDamage(object sender, System.EventArgs e)
    {
        if(Random.Range(0, 1f) < spawnChance * count)
        {
            Instantiate(projectile, owner.transform.position + (Vector3)Random.insideUnitCircle * spawnDistance, Quaternion.identity)
                .GetComponent<IProjectile>().Fire(owner.target - (Vector2)owner.transform.position, owner);
        }
    }

}
