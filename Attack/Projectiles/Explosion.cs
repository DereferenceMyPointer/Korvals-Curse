using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Explosion effect base class
 */
public class Explosion : MonoBehaviour
{
    public Animator anim;
    
    public virtual void Explode()
    {
        //anim.SetTrigger("Explode");
    }

    // Typically called by animation event to remove GameObject after animation
    public virtual void Destruct()
    {
        Destroy(gameObject);
    }

}
