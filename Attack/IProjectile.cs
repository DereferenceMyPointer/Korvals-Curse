using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
    void Fire(Vector2 direction, Character owner);
}
