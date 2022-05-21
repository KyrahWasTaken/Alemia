using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTable : MonoBehaviour
{
    public List<ItemStack> inventory;
    // Start is called before the first frame update
    void Start()
    {

    }
    public delegate void updateInventory(LootTable inv);
    public event updateInventory OnInventoryChanged;
    public bool tryPickUpItem(Item item, int count)
    {
        foreach (ItemStack a in inventory)
        {

            if (a.count < a.item.StackCount && a.item.nameID == item.nameID)
            {
                a.count += count;
                if (a.count > a.item.StackCount)
                {

                    count = a.count - item.StackCount;
                    a.count = a.item.StackCount;
                }
                else
                {
                    count = 0;
                    OnInventoryChanged.Invoke(this);
                    return true;
                }
            }
        }
            inventory.Add(new ItemStack(item, count));
            OnInventoryChanged.Invoke(this);
            return true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
