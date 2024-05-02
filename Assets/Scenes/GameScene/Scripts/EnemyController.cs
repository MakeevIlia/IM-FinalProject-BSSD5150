using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float speed = 1.0f;
    [SerializeField]
    private Transform projectileSpawnPointLeft;
    [SerializeField]
    private Transform projectileSpawnPointRight;
    [SerializeField]
    private BoxCollider2D LeftVision;
    [SerializeField]
    private BoxCollider2D RightVision;

    private const string dificultyLevelKey = "dificulty";
    public bool alive = true;
    private Rigidbody2D rb2d;
    Animator anim;
    BoxCollider2D col;
    private float fireIntensity;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
        CheckPlayerPrefs();
    }

    void CheckPlayerPrefs()
    {
        int choice = PlayerPrefs.GetInt(dificultyLevelKey, 0);
        if (choice == 0)
        {
            speed = 1.0f;
            fireIntensity = 1.5f;
        }
        else if (choice == 1)
        {
            speed = 2.0f;
            fireIntensity = 1f;
        }
        else 
        { 
            speed = 3.0f;
            fireIntensity = 2f;
        }
    }

    void Update()
    {
        if (alive)
        {
            rb2d.MovePosition(rb2d.position + Vector2.left * speed * Time.fixedDeltaTime);
        }

        if (speed > 0.01f)
        {
            RightVision.enabled = false;
            LeftVision.enabled = true;
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (speed < -0.01f)
        {
            RightVision.enabled = true;
            LeftVision.enabled = false;
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Edge")
        {
            speed *= -1;
        }

        if (collision.gameObject.tag == "VisibleHero" && alive)
        {
            InvokeRepeating("shoot", 0.1f, fireIntensity);
        }

        if (collision.gameObject.tag == "Bullet" && alive)
        {
            alive = false;
            anim.SetTrigger("Death");
            col.isTrigger = true;
            Destroy(collision.gameObject);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "VisibleHero")
        {
            CancelInvoke();
        }
    }

    private void shoot()
    {
        if (alive) 
        {
            if (gameObject.GetComponent<SpriteRenderer>().flipX)
            {
                GameObject Bullet = Instantiate(Resources.Load("Bullet"), projectileSpawnPointLeft.transform.position,
                    Quaternion.Euler(0, 0, 180)) as GameObject;
            }
            else
            {
                GameObject Bullet = Instantiate(Resources.Load("Bullet"), projectileSpawnPointRight.transform.position,
                    Quaternion.identity) as GameObject;
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Knife")
        {
            alive = false;
            anim.SetTrigger("DeathByKnife");
            col.isTrigger = true;
            gameObject.tag = "EnemyWithKnife";
            rb2d.gravityScale = 0;
            rb2d.constraints = RigidbodyConstraints2D.FreezePositionX| RigidbodyConstraints2D.FreezePositionY;
        }
    }
}
