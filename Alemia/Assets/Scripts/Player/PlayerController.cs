using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Entity stats;
    public float speed;
    public bool isAtacking;
    void Start()
    {
        isAtacking = false;
        stats = GetComponent<Entity>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAtacking)
        {
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            StartCoroutine(Attack(direction));
            
        }
    }

    private IEnumerator Attack(Vector2 direction)
    {
        isAtacking = true;
        stats.Attack(direction);
        yield return new WaitForSeconds(1 / stats.attackSpeed);
        isAtacking = false;
        yield return null;
    }
    void FixedUpdate()
    {
        float moveX, moveY;
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        rb.AddForce(new Vector2(moveX, moveY).normalized*speed*Time.fixedDeltaTime);
    }
}
