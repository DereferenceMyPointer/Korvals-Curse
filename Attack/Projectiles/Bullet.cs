using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Basic projectile implementation, parent class
 * Flies in given direction at constant velocity and has collision behavior
 */
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour, IProjectile
{
    public float speed;
    public float damage;
    public Explosion collisionEffect;
    public IDamageable.Type damageType;
    public Character owner;
    private Rigidbody2D rb;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Fly and rotate toward flying direction
    // Can be overriden to alter fire behavior
    public virtual void Fire(Vector2 direction, Character owner)
    {
        rb.velocity = direction * speed;
        transform.Rotate(Vector3.forward, Vector2.SignedAngle(Vector2.right, direction));
        this.owner = owner;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CollisionBehavior(collision);
    }

    // Effect on collide
    // Base implementation damages target if tag is different from owner's and destroys the object after instantiating its collision effect
    protected virtual void CollisionBehavior(Collider2D collision)
    {
        Instantiate(collisionEffect.gameObject, transform.position, Quaternion.identity).GetComponent<Explosion>();
        Destroy(this.gameObject);
        if (!owner && collision.TryGetComponent<IDamageable>(out IDamageable i))
        {
            i.Damage(damage, damageType, null);
            return;
        }
        if (collision.TryGetComponent<IDamageable>(out IDamageable d) && !owner.CompareTag(collision.tag))
        {
            switch (d.Damage(damage, damageType, owner))
            {
                case IDamageable.Result.Damaged: owner.EnemyHit(); break;
                case IDamageable.Result.Killed: owner.EnemyHit(); owner.EnemyKilled(); break;
                default: break;
            }
        }
    }

}
