using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Generic Item Class
 * Use for items that give default stat buffs
 * Extend for more complicated items
 * Override equip to unsubscribe from previous owner and subscribe to new owner's
 * necessary events in addition to default behavior
 */

public class Item : MonoBehaviour
{
    // Character to which the item belongs (changes on transfer)
    public Character owner;

    // Item tiers
    public enum Tier
    {
        Peasant,
        Commoner,
        Lord,
        Saint,
        Deity
    }

    // Default stat multipliers
    public float damageX = 1;
    public float speedX = 1;
    public float regenX = 1;
    public float healthX = 1;
    public float reloadX = 1;

    // Info for inventory to display
    // Note that name will be stored by the GameObject
    public Tier tier;
    public Sprite icon;
    public float count = 1;
    public string description;

    // Reparent and reposition the item to a new character
    // Override to handle unsubscribing and subscribing to character events
    public virtual void Equip(Character target)
    {
        transform.SetParent(target.inventory.transform);
        transform.localPosition = Vector3.zero;
        if (owner)
        {
            owner.inventory.RefreshItems();
        }
        target.inventory.RefreshItems();
        owner = target;
    }

    // Find multiplier by name
    // Used by ItemInventory to total multipliers
    public float GetMultiplier(string name)
    {
        switch (name)
        {
            case "damage": return count * (damageX - 1) + 1;
            case "health": return count * (healthX - 1) + 1;
            case "regen": return count * (regenX - 1) + 1;
            case "speed": return count * (speedX - 1) + 1;
            case "reload": return count * (reloadX - 1) + 1;
            default: return 1;
        }
    }

    public void Stack()
    {
        count++;
    }

}
