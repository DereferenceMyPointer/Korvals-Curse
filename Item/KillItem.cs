using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillItem : Item
{
    protected virtual void Start()
    {
        if (owner)
            owner.OnKillEnemy += OnKill;
    }

    public override void Equip(Character target)
    {
        transform.SetParent(target.inventory.transform);
        transform.localPosition = Vector3.zero;
        if (owner)
        {
            owner.inventory.RefreshItems();
            owner.OnKillEnemy -= OnKill;
        }
        target.inventory.RefreshItems();
        owner = target;
        owner.OnKillEnemy += OnKill;
    }

    protected virtual void OnKill(object sender, System.EventArgs e)
    {
    }

}
