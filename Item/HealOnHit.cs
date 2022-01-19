using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOnHit : Item
{
    private void Start()
    {
        if (owner)
            owner.OnHitEnemy += OnHitEnemy;
    }

    public override void Equip(Character target)
    {
        transform.SetParent(target.inventory.transform);
        transform.localPosition = Vector3.zero;
        if (owner)
        {
            owner.inventory.RefreshItems();
            owner.OnHitEnemy -= OnHitEnemy;
        }
        target.inventory.RefreshItems();
        owner = target;
        owner.OnHitEnemy += OnHitEnemy;
    }

    private void OnHitEnemy(object sender, System.EventArgs e)
    {
        owner.Heal(count);
    }

}
