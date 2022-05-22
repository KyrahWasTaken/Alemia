using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    public Transform self;
    public float speed;
    public float maxDistance;
    public Vector2 direction;
    IEnumerator move()
    {
        for(float i = 0; i<maxDistance; i+=speed*Time.fixedDeltaTime)
        {
            transform.Translate(direction.normalized*speed*Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
        yield return null;
    }
    void Start()
    {
        StartCoroutine(move());
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Entity a;
        if(other.TryGetComponent(out a))
        {
            if(other.transform != self)
            {
                //Here is your code to process Damage logic.
                //Don't forget about some Defaults (may differ from version to version):
                //1. base damage is provided by equiped Weapon
                //2. base armor is provided by armor equped on target. if None, then armor is 0;
                //3. Armor reduces damage by (int)2f^armor/damage
                //4. Be careful! Avoid possible zeroes in denumerator
                //5. You can add custom effects independent or based on equipment
                a.changeHealth(-3);
           }
        }
    }
}
