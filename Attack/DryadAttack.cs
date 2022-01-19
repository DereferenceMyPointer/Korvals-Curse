using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryadAttack : MonoBehaviour, Attack
{
    public Character owner;
    public float baseReload;
    public float radius;
    public float initialDamage;
    public LayerMask canHit;
    public GameObject projectile;
    public int numberToSpawn;
    private bool canUse = true;

    public void Use()
    {
        if (canUse)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(owner.transform.position, radius, canHit);
            foreach (Collider2D c in hits)
            {
                if (c.TryGetComponent<Character>(out Character ch))
                {
                    switch (ch.Damage(initialDamage * owner.inventory.GetMultiplier("damage"), IDamageable.Type.Wet, owner))
                    {
                        case IDamageable.Result.Damaged:
                            owner.EnemyHit();
                            break;
                        case IDamageable.Result.Killed:
                            owner.EnemyHit();
                            owner.EnemyKilled();
                            break;
                        default:
                            break;
                    }
                }
            }
            if(hits.Length == 0)
            {
                Shoot();
            }
            canUse = false;
            StartCoroutine(Wait());
        }
    }

    private void Shoot()
    {
        for(int i = 0; i < numberToSpawn; i++)
        {
            Vector3 direction = Random.insideUnitCircle;
            Instantiate(projectile, owner.transform.position + direction * radius, Quaternion.identity)
                .GetComponent<IProjectile>().Fire(direction, owner);
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(baseReload / owner.inventory.GetMultiplier("reload"));
        canUse = true;
    }

}
