using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Simple search state
 */
public class SearchState : State
{
    public State findState;
    public float viewDistance;
    public LayerMask canFind;
    public LayerMask wall;
    public Range moveDistance;
    public Range stillTime;
    public Animator anim;
    private bool ready;
    private bool waiting;

    public override void Begin()
    {
        if (Random.Range(0, 4) < 1)
        {
            StartCoroutine(Wait());
            waiting = true;
        }
        else
        {
            character.target = (Vector2)character.transform.position + UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(moveDistance.min, moveDistance.max);
            if (Physics2D.Raycast(character.transform.position, character.target - (Vector2)character.transform.position, viewDistance, wall))
                Begin();
            waiting = false;
        }
        ready = false;
    }

    public override State Tick()
    {
        if(Physics2D.OverlapCircle(character.transform.position, viewDistance, canFind))
        {
            return findState;
        }
        anim.SetBool("Running", !waiting);
        if (!waiting)
        {
            Vector2 direction = character.target - (Vector2)character.transform.position;
            if (direction.magnitude < 0.1f)
                Begin();
            character.Move(direction);
        }
        if (waiting)
            character.rb.velocity = Vector2.zero;
        if (ready && waiting)
        {
            Begin();
        }
        return this;
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(Random.Range(stillTime.min, stillTime.max));
        ready = true;
    }

}
