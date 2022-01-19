using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chalice : KillItem
{
    public FireProjectiles attack;

    protected override void OnKill(object sender, EventArgs e)
    {
        attack.owner = owner;
        StartCoroutine(RepeatFire());
    }

    public IEnumerator RepeatFire()
    {
        Attack a = attack.GetComponent<Attack>();
        for (int i = 0; i < count; i++)
        {
            a.Use();
            yield return new WaitForSeconds(attack.baseReload);
        }
    }

}
