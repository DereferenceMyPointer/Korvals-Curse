using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rampage : KillItem
{
    private int stacks;
    public float duration;
    public float buffAmount;

    protected override void OnKill(object sender, EventArgs e)
    {
        if(stacks < count)
        {
            stacks++;
            StartCoroutine(Timeout());
        }
    }

    private IEnumerator Timeout()
    {
        damageX += buffAmount;
        yield return new WaitForSeconds(duration);
        damageX -= buffAmount;
        speedX -= buffAmount;
        if(stacks > 0)
            stacks--;
        if (damageX < 1)
            damageX = 1;
    }

}
