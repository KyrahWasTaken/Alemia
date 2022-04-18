using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3.0f;
    public float changeTime = 2.0f;
    new Rigidbody2D rigidbody;
    int direction = 0;
    Vector2 position;
    Vector2 aim;
    float timer;
    bool isCharSeen = false;
    //Animator animator;
    //public ParticleSystem smokeEffect;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        timer = changeTime;
        //animator = GetComponent<Animator>();
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = changeTime;
            direction++;
            if (direction == 4) direction = 0;
        }

        position = rigidbody.position;

        Vector2 nextPos = position;

        if (!isCharSeen)
        {
            switch (direction)
            {
                case 0:
                    nextPos.y = position.y + 1;
                    Move(nextPos);
                    break;
                case 1:
                    nextPos.x = position.x + 1;
                    Move(nextPos);
                    break;
                case 2:
                    nextPos.y = position.y - 1;
                    Move(nextPos);
                    break;
                case 3:
                    nextPos.x = position.x - 1;
                    Move(nextPos);
                    break;
                default:
                    break;
            }
        }
        else
        {
            Move(aim);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        isCharSeen = true;
        PlayerController character = collision.GetComponent<PlayerController>();
        Rigidbody2D charBody = character.GetComponent<Rigidbody2D>();
        aim = charBody.position;
        Debug.Log("Collision " + aim.x + ", " + aim.y);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isCharSeen = false;
    }

    void Move(Vector2 direction)
    {
        Vector2 path = direction - rigidbody.position;
        path.Normalize();
        position.x = position.x + path.x * Time.fixedDeltaTime * speed;
        position.y = position.y + path.y * Time.fixedDeltaTime * speed;
        rigidbody.MovePosition(position);
    }
}
