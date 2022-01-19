using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Fire multiple projectiles in a circle
 */
public class FireProjectiles : MonoBehaviour, Attack
{
    // Base speed allowed
    public float baseReload;
    //Distance to spawn away from source
    public float distanceToSpawn;
    public int numberToSpawn;
    public AudioSource sound;
    public GameObject projectile;
    public Character owner;
    private bool usable = true;

    // Fire the projectile; rotates projectile to direction
    // Fires in a circle
    public virtual void Use()
    {
        if (usable)
        {
            for(int i = 0; i < numberToSpawn; i++)
            {
                float degrees = ((float)i - (float)numberToSpawn / 2f) / (float)numberToSpawn * 360;
                Vector2 direction = Quaternion.AngleAxis(degrees, Vector3.forward) * Vector3.up;
                Instantiate(projectile, (Vector2)owner.transform.position + direction * distanceToSpawn, Quaternion.identity)
                    .GetComponent<IProjectile>().Fire(direction, owner);
                usable = false;
                sound.Play();
                StartCoroutine(Wait());
            }
        }
    }

    // Cooldown. Scales with player items
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(baseReload / owner.inventory.GetMultiplier("reload"));
        usable = true;
    }
}
