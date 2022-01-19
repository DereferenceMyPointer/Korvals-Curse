using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Simple random movement state
 * Navigates a character randomly, avoiding walls
 * No behavior for switcing to the next state (so far)
 */
public class MoveRandomlyState : State
{
    public State nextState;
    public Range distance;
    public Range stillTime;
    public float viewDistance;
    public LayerMask walls;
    public LayerMask enemies;

    public override void Begin()
    {
        character.target = (Vector2)character.transform.position + UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(distance.min, distance.max);
    }

    public override State Tick()
    {
        Vector2 direction = character.target - (Vector2)character.transform.position;
        RaycastHit2D wallHit = Physics2D.Raycast(character.transform.position, direction, direction.magnitude, walls, 0, 100);
        if (wallHit || Vector2.Distance(character.target, character.transform.position) < 0.1f)
        {
            Begin();
        }
        character.Move(direction);
        return this;
    }
}
