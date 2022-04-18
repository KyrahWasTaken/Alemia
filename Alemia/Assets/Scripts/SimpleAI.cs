using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : MonoBehaviour
{
    public Collider2D ViewZoneTrigger;

    private Entity entityData;
    private Rigidbody2D rb2d;
    private Transform target;

    bool isWandering;

    void Start()
    {
        isWandering = false;
        entityData = GetComponent<Entity>();
        rb2d = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            target = collision.transform;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            target = null;
    }
    private void FixedUpdate()
    {
        if (target != null)
        {
            StopCoroutine(Wander());
            isWandering = false;
            Vector2 direction = target.position - transform.position;
            direction.Normalize();
            Move(direction);
        }
        else if(!isWandering)
            StartCoroutine(Wander());
    }
    private IEnumerator Wander()
    {
        isWandering = true;
        while(isWandering)
        {
        Debug.Log("Корутина Началась");
        float x, y;
        x = Random.Range(-3, 3);
        y = Random.Range(-3, 3);
        Vector2 destination = (Vector2)transform.position + new Vector2(x, y);
        while (Vector2.Distance(transform.position, destination) >= 1f)
        {
                Debug.Log($"Distance:{Vector2.Distance(transform.position, destination)}");
            Move(new Vector2(x, y).normalized);
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(Random.Range(0.5f,2));
        }
    }
    void Move(Vector2 direction)
    {
        rb2d.AddForce(direction*entityData.speed*Time.fixedDeltaTime);
    }
}
