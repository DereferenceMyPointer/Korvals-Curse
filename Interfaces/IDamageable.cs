using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    enum Type
    {
        Fire,
        Wet,
        Freezing,
        Electric
    }

    enum Result
    {
        Undamaged,
        Killed,
        Damaged
    }

    Result Damage(float amount, Type type, Character source);
}