using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    private bool isImmune;
    public float iDuration;
    public float regenTimer;

    protected override void Start()
    {
        StartCoroutine(Heal());
        base.Start();
    }

    private IEnumerator Heal()
    {
        while (this)
        {
            Heal(1);
            yield return new WaitForSeconds(regenTimer);
        }
    }

    public override IDamageable.Result Damage(float amount, IDamageable.Type type, Character source)
    {
        if (isImmune)
        {
            return IDamageable.Result.Undamaged;
        }
        return base.Damage(amount, type, source);
    }

    private IEnumerator IFrames()
    {
        isImmune = true;
        yield return new WaitForSeconds(iDuration * inventory.GetMultiplier("iframes"));
        isImmune = false;
    }

}
