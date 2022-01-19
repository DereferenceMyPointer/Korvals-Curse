using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class TrackingProjectile : MonoBehaviour, IProjectile
{
    public Character owner;
    public float speed;
    public float baseDamage;
    public float lifetime;
    public float aggressiveness;
    public float detectRadius;
    public LayerMask canTarget;
    public Transform target;
    public Rigidbody2D rb;
    public bool rotates;
    public bool procsKill;
    public bool procsHit;
    private Vector2 direction;

    public void Fire(Vector2 direction, Character owner)
    {
        this.owner = owner;
        rb.velocity = direction;
        FindTarget();
        StartCoroutine(Life());
        
    }

    void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectRadius, canTarget);
        float distance = Mathf.Infinity;
        foreach(Collider2D hit in hits)
        {
            if(Vector2.Distance(hit.transform.position, owner.transform.position) < distance)
            {
                target = hit.transform;
            }
        }
        if (target && rb.velocity.magnitude < speed)
            rb.velocity = (target.position - transform.position).normalized * speed;
    }

    private void Update()
    {
        if (!target)
        {
            FindTarget();
        } else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, (target.position - transform.position).normalized * speed, aggressiveness);
        }
        if (rotates)
        {
            transform.rotation = Quaternion.AngleAxis(Vector3.SignedAngle(Vector3.right, rb.velocity.normalized, Vector3.forward), Vector3.forward);
        }
    }

    public IEnumerator Life()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(LayerMask.GetMask(LayerMask.LayerToName(collider.gameObject.layer)) == canTarget)
        {
            if(collider.TryGetComponent<Character>(out Character c))
            {
                switch (c.Damage(baseDamage * owner.inventory.GetMultiplier("damage"), IDamageable.Type.Wet, owner))
                {
                    case IDamageable.Result.Damaged:
                        if(procsHit)
                            owner.EnemyHit();
                        break;
                    case IDamageable.Result.Killed:
                        if(procsHit)
                            owner.EnemyHit();
                        if(procsKill)
                            owner.EnemyKilled();
                        break;
                    default:
                        break;
                }
            }
            Destroy(gameObject);
        }
    }

}
