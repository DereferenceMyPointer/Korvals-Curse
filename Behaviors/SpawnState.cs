using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnState : State
{
    public State nextState;
    public string animatorString;
    public Animator anim;

    public override State Tick()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName(animatorString))
            return nextState;
        return this;
    }

}
