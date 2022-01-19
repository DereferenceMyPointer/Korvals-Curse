using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowState : State
{
    public State attackState;
    public State loseState;
    public float trackDistance;
    public float detectDistance;
    public float attackDistance;
    public Transform target;
    public Animator anim;
    public string animatorRunState;
    public LayerMask castTargets;

    public override void Begin()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(character.transform.position, detectDistance, castTargets);
        if (hit.Length > 0)
        {
            anim.SetBool(animatorRunState, true);
            target = hit[Random.Range(0, hit.Length - 1)].transform;
        }
    }

    public override State Tick()
    {
        if (!target)
        {
            return loseState;
        }
        character.Move(target.position - character.transform.position);
        character.target = target.transform.position;
        if((target.position - character.transform.position).magnitude <= attackDistance)
        {
            return attackState;
        }
        return this;
    }

    public override void End()
    {
        anim.SetBool(animatorRunState, false);
        target = null;
        character.rb.velocity = Vector2.zero;
    }

}
