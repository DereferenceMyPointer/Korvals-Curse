using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFade : MonoBehaviour
{
    public Animator anim;

    public void Fade()
    {
        anim.SetTrigger("Fade");
    }

}
