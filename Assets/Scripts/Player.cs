using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Collider2D bottomCollider;
    public CompositeCollider2D terrainCollider;

    private Vector2 originPosition;

    public float speed = 7;
    public float jumpSpeed = 15;

    private float velocity = 0f;
    private float prevVx = 0f;
    private bool isGrounded;

    private void Start()
    {
        originPosition = transform.position;
    }

    public void Restart()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        GetComponent<Rigidbody2D>().angularVelocity = 0;
        GetComponent<BoxCollider2D>().enabled = true;

        transform.eulerAngles = Vector3.zero;
        transform.position = originPosition;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    private void Update()
    {
        velocity = Input.GetAxisRaw("Horizontal");

        if (velocity < 0f)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        if (velocity > 0f)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

        if (bottomCollider.IsTouching(terrainCollider))
        {
            if (isGrounded == false)
            {
                // ÂøÁö
                if (velocity == 0f)
                {
                    GetComponent<Animator>().SetTrigger("Idle");
                }
                else
                {
                    GetComponent<Animator>().SetTrigger("Run");
                }
            }
            else
            {
                // °è¼Ó °È´Â Áß
                if (prevVx != velocity)
                {
                    if (velocity == 0f)
                    {
                        GetComponent<Animator>().SetTrigger("Idle");
                    }
                    else
                    {
                        GetComponent<Animator>().SetTrigger("Run");
                    }
                }
            }

            isGrounded = true;
        }
        else
        {
            if (isGrounded)
            {
                // Á¡ÇÁ ½ÃÀÛ
                GetComponent<Animator>().SetTrigger("Jump");
            }

            isGrounded = false;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpSpeed;
        }

        prevVx = velocity;

        // ÃÑ¾Ë ¹ß»ç
        if (Input.GetButtonDown("Fire1")) // && lastShoot + 0.5f < Time.time)
        {
            Vector2 bulletV = Vector2.zero;

            if (GetComponent<SpriteRenderer>().flipX)
            {
                bulletV = new Vector2(-10f, 0f);
            }
            else
            {
                bulletV = new Vector2(10f, 0f);
            }

            GameObject bullet = ObjectPool.Instance.GetBullet();
            bullet.transform.position = transform.position;
            bullet.GetComponent<Bullet>().velocity = bulletV;
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector2.right * velocity * speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Die();
        }
    }

    private void Die()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        GetComponent<Rigidbody2D>().angularVelocity = 720f;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 10f),ForceMode2D.Impulse);
        GetComponent<BoxCollider2D>().enabled = false;

        GameManager.Instance.Die();
    }
}