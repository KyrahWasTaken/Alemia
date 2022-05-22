using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType
{
    Beast, Humanoid, Sentient, Undead, Arachnid, Slime, Psyonic
}
public class Entity : MonoBehaviour
{
    public EntityType[] entityTags;
    public GameObject hitObject;
    public List<ItemStack> inventory;
    public Transform DeathContainer;

    #region characteristics
    public int baseHealth;
    public int maxHealth;
    public int health;

    public int baseHunger;
    public int maxHunger;
    public int hunger;

    public int baseThirst;
    public int maxThirst;
    public int thirst;

    public float speed;
    public float attackSpeed;
    public int damage;

    public int inventoryCapacity;
    #endregion
    #region skills
    public int physicalSkills;
    public int intellectualSkills;
    public int magicSkills;
    public int empathySkills;
    #endregion
    #region abilities
    #endregion
    public delegate void changeOnHP(int max, int current);
    public delegate void updateInventory(Entity Player);
    private void DoNothingHaha(int max, int current)
    {
        return;
    }
    public event changeOnHP OnHPChanged;
    public event updateInventory OnInventoryChanged;
    public void changeHealth(int value)
    {
        health += value;
        if (health <= 0)
            StartCoroutine(Die());
        OnHPChanged.Invoke(maxHealth, health);
    }

    private IEnumerator Die()
    {
        //do something e.g. start Animations or other activities
        SpawnContainer();
        Destroy(gameObject);
        yield return new WaitForFixedUpdate();
    }

    private void SpawnContainer()
    {
        LootTable loot = Instantiate(DeathContainer,transform.position,Quaternion.identity).GetComponent<LootTable>();
        foreach (ItemStack i in inventory)
        {
            loot.inventory.Add(i);
        }
    }

    void UpdateCharacteristics()
    {
        maxHealth = baseHealth + physicalSkills * 5 + empathySkills;
        maxHunger = physicalSkills * 2 + empathySkills + intellectualSkills;
        maxThirst = physicalSkills + empathySkills + intellectualSkills * 2;
        speed = 5 + physicalSkills / 5 + intellectualSkills / 10;
        attackSpeed = 5 + physicalSkills / 10 + intellectualSkills / 10;
        damage = 1 + physicalSkills + intellectualSkills / 10;
    }
    public void Attack(Vector2 direction)
    {
        GameObject a = Instantiate(hitObject, transform.position,Quaternion.identity);
        a.GetComponent<Hit>().direction = direction;
        a.GetComponent<Hit>().self = transform;
    }
    public bool tryPickUpItem(Item item, int count)
    {
        foreach(ItemStack a in inventory)
        {

            if (a.count < a.item.StackCount && a.item.nameID == item.nameID)
            {
                a.count += count;
                if (a.count > a.item.StackCount)
                {

                    count = a.count-item.StackCount;
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
        if (inventory.Count < inventoryCapacity)
        {
                inventory.Add(new ItemStack(item, count));
                OnInventoryChanged.Invoke(this);
            return true;
        }
        OnInventoryChanged.Invoke(this);
        return false;
    }
    public void Start()
    {
        OnHPChanged += DoNothingHaha;
    }

}
