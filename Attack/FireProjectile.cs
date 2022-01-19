using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Simple projectile firing attack
 * Requires projectile prefab with Projectile behavior
 */
public class FireProjectile : MonoBehaviour, Attack
{
    // Base speed allowed
    public float baseReload;
    //Distance to spawn away from source
    public float distanceToSpawn;
    public AudioSource sound;
    public GameObject projectile;
    public Character owner;
    private bool usable = true;

    // Fire the projectile; rotates projectile to direction
    // Fires in direction of owner character's current target
    public virtual void Use()
    {
        if (usable)
        {
            Vector2 direction = (owner.target - (Vector2)owner.transform.position).normalized;
            Instantiate(projectile, (Vector2)owner.transform.position + direction * distanceToSpawn, Quaternion.identity)
                .GetComponent<IProjectile>().Fire(direction, owner);
            usable = false;
            sound.Play();
            StartCoroutine(Wait());
        }
    }

    // Cooldown. Scales with player items
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(baseReload / owner.inventory.GetMultiplier("reload"));
        usable = true;
    }

}