using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public Animator anim;
    public string animString;
    public string attackName;
    public State returnState;

    public override void Begin()
    {
        anim.SetTrigger(animString);
        character.rb.velocity = Vector2.zero;
    }

    public override State Tick()
    {
        character.Attack(attackName);
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName(animString))
            return returnState;
        return this;
    }
}
