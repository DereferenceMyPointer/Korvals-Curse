using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Character inventory for items
 * Place on a GameObject where each child is an item in the inventory
 * Used to calculate buffs and manage item equips
 */

public class ItemInventory : MonoBehaviour
{
    // Contains items
    private Dictionary<string, Item> inv;

    // Character to which this inventory belongs
    public Character owner;

    // Initialize items
    private void Awake()
    {
        inv = new Dictionary<string, Item>();
        RefreshItems();
    }

    // Update inventory dictionary
    public void RefreshItems()
    {
        inv.Clear();
        foreach(Item i in GetComponentsInChildren<Item>())
        {
            inv.Add(i.gameObject.name, i);

        }
    }

    // Transfer an item to this inventory
    public void EquipItem(Item item)
    {
        string newName = item.name.Split('(')[0];
        item.name = newName;
        if (inv.ContainsKey(item.name) && inv.TryGetValue(item.gameObject.name, out Item i))
        {
            i.Stack();
        } else
        {
            GameObject o = Instantiate(item.gameObject);
            o.name = newName;
            o.GetComponent<Item>().Equip(owner);
        }
        owner.PickedUpItem(item);
        RefreshItems();
    }

    // Get the overall multiplier for a stat
    // type is the stat name in all lowercase
    // "regen", "speed", "damage", "health"
    public float GetMultiplier(string type)
    {
        float output = 1;
        foreach(Item i in inv.Values)
        {
            output *= i.GetMultiplier(type);
        }
        return output;
    }

    public List<Item> GetAllItems()
    {
        List<Item> output = new List<Item>();
        foreach(Item i in inv.Values) { output.Add(i); }
        return output;
    }

}
