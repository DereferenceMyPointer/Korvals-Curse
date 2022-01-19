using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Parent state class; defines necessary methods for a state
 */
public class State : MonoBehaviour
{
    [SerializeField]
    protected Character character;

    [System.Serializable]
    public struct Range
    {
        public float min;
        public float max;
    }

    // Return state to change to, this if no change
    public virtual State Tick() { return this; }
    // Executes on entering state
    public virtual void Begin() { }
    // Executes on leaving state
    public virtual void End() { }
}
