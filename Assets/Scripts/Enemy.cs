using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp = 3;
    public float speed = 3f;

    public Vector2 velocity;

    public Collider2D frontCollider;
    public Collider2D frontBottomCollider;
    public CompositeCollider2D terrainCollider;

    private void Start()
    {
        velocity = Vector2.right * speed;
    }

    private void Update()
    {
        if (frontCollider.IsTouching(terrainCollider) || frontBottomCollider.IsTouching(terrainCollider) == false)
        {
            velocity = -velocity;
            transform.localScale = new Vector2(-transform.localScale.x, 1f);
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(velocity * Time.fixedDeltaTime);
    }

    public void Hit(int damage)
    {
        hp -= damage;

        if (hp<=0)
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            GetComponent<Rigidbody2D>().angularVelocity = 720f;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 10f), ForceMode2D.Impulse);

            Invoke("DestroyThis", 2f);
        }
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }
}