using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Parent character class
 * Contains all default behaviors for a character that has items
 * Setup: Parent transform contains all behavior scripts; child objects for graphics,
 * inventory, state machine, attacks
 */

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour, IDamageable
{
    public float baseSpeed;
    public float acceleration;
    public float defaultHealth;

    // Reference to inventory
    public ItemInventory inventory;

    // Components
    public Rigidbody2D rb;
    public Transform graphics;

    // Handle attacks
    public List<GameObject> attackObjects;
    private Dictionary<string, Attack> allAttacks;

    public Vector2 target;

    // Character events
    public event EventHandler OnKillEnemy;
    public event EventHandler OnKillSelf;
    public event EventHandler OnHitEnemy;
    public event EventHandler OnHealSelf;
    public event EventHandler OnReceiveDamage;
    public event Action<Item> OnItemPickup;

    protected float health;

    public float Health { get => health * inventory.GetMultiplier("health"); }

    protected virtual void Start()
    {
        allAttacks = new Dictionary<string, Attack>();
        foreach(GameObject a in attackObjects)
        {
            allAttacks.Add(a.name, a.GetComponent<Attack>());
        }
        health = defaultHealth;
    }

    // Default damage behavior. Scales damage received with health, does not depend on type
    public virtual IDamageable.Result Damage(float amount, IDamageable.Type type, Character source)
    {
        health -= amount / inventory.GetMultiplier("health");
        if(Health > 0)
        {
            OnReceiveDamage?.Invoke(this, EventArgs.Empty);
            return IDamageable.Result.Damaged;
        } else
        {
            OnReceiveDamage?.Invoke(this, EventArgs.Empty);
            OnKillSelf?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
            return IDamageable.Result.Killed;
        }
    }

    public void Heal(float amount)
    {
        if(health < defaultHealth)
        {
            health += amount / inventory.GetMultiplier("health") * inventory.GetMultiplier("regen");
            if (health > defaultHealth)
                health = defaultHealth;
            SelfHealed();
        }
    }

    public void Move(Vector2 direction)
    {
        float maxSpeed = baseSpeed * inventory.GetMultiplier("speed");
        direction = direction.normalized;
        if(rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(direction * acceleration * inventory.GetMultiplier("speed"));
        } else
        {
            rb.velocity = direction * baseSpeed;
        }
        if(direction == Vector2.zero)
        {
            rb.velocity = Vector2.zero;
        }
        graphics.localScale = direction.x < 0 ? new Vector3(-1, transform.localScale.y, 0) : new Vector3(1, transform.localScale.y, 0);
    }

    public void Attack(string attackName)
    {
        if (allAttacks.TryGetValue(attackName, out Attack a))
        {
            a.Use();
        }
    }

    public void EnemyKilled()
    {
        OnKillEnemy?.Invoke(this, EventArgs.Empty);
    }

    public virtual void SelfKilled()
    {
        OnKillSelf?.Invoke(this, EventArgs.Empty);
    }

    public void EnemyHit()
    {
        OnHitEnemy?.Invoke(this, EventArgs.Empty);
    }

    public void SelfHealed()
    {
        OnHealSelf?.Invoke(this, EventArgs.Empty);
    }

    public void DamageReceived()
    {
        OnReceiveDamage?.Invoke(this, EventArgs.Empty);
    }

    public void PickedUpItem(Item i)
    {
        OnItemPickup?.Invoke(i);
    }

}
