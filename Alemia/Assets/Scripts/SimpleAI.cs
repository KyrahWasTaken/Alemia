using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : MonoBehaviour
{
    public Collider2D ViewZoneTrigger;

    public float attackDistance;
    
    private Entity entityData;
    private Rigidbody2D rb2d;
    private Transform target;

    private bool isWandering;
    private bool isAtacking;

    void Start()
    {
        isAtacking = false;
        isWandering = false;
        entityData = GetComponent<Entity>();
        rb2d = GetComponent<Rigidbody2D>();
        StartCoroutine(Wander());
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
    private IEnumerator Chase()
    {
        StopCoroutine(Wander());
        while (target != null)
        {
            Vector2 direction = target.position - transform.position;
            direction.Normalize();
            Move(direction);
            if (Vector3.Distance(target.position, transform.position) < GetComponent<Entity>().hitObject.GetComponent<Hit>().maxDistance)
                if (!isAtacking)
                    StartCoroutine(Attack(direction));
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(Wander());
    }
    private IEnumerator Attack(Vector2 direction)
    {
        isAtacking = true;
        GetComponent<Entity>().Attack(direction);
        yield return new WaitForSeconds(1f / GetComponent<Entity>().attackSpeed);
        isAtacking = false;
        yield return null;
    }
    private IEnumerator Wander()
    {
        StopCoroutine(Chase());
        while (target == null)
        {
            float x, y;
            x = Random.Range(-3, 3);
            y = Random.Range(-3, 3);
            Vector2 destination = (Vector2)transform.position + new Vector2(x, y);
            while (Vector2.Distance(transform.position, destination) >= 1f)
            {
                Move(new Vector2(x, y).normalized);
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForSeconds(Random.Range(0.5f, 2));
        }
        StartCoroutine(Chase());
    }
    void Move(Vector2 direction)
    {
        rb2d.AddForce(direction * entityData.speed * Time.fixedDeltaTime);
    }
}
