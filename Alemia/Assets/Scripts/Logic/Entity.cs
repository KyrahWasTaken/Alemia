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
    public event changeOnHP OnHPChanged;
    public void changeHealth(int value)
    {
        health += value;
        if (health <= 0)
            Destroy(gameObject);
        OnHPChanged.Invoke(maxHealth, health);
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
}
