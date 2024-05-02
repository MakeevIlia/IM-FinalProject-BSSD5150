using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private float speed = 4f;
    private Rigidbody2D rb2d;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void OnBecameVisible()
    {
        Transform MainCharPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if (MainCharPos.position.x > gameObject.transform.position.x)
        {
            rb2d.velocity = Vector2.left * speed;
        }
        else 
        {
            rb2d.velocity = Vector2.right * speed;
        }
        
    }

    private void Update()
    {
        if (Mathf.Abs(rb2d.velocity.x) > 1) 
            rb2d.rotation -= 2;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
        else
        {
            rb2d.velocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = -1 * collision.gameObject.GetComponent<Rigidbody2D>().velocity;
        }
    }
}
