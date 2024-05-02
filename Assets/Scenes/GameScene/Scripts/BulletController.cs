using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletController : MonoBehaviour
{
    private float speed;
    private Rigidbody2D rb2d;
    private const string dificultyLevelKey = "dificulty";

    void CheckPlayerPrefs()
    {
        int choice = PlayerPrefs.GetInt(dificultyLevelKey, 0);
        if (choice == 0)
        {
            speed = 4.0f;
        }
        else if (choice == 1)
        {
            speed = 6.0f;
        }
        else
        {
            speed = 7.0f;
        }
    }
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        CheckPlayerPrefs();
    }

    private void OnBecameVisible()
    {
        rb2d.velocity = transform.right * speed;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }

    private void Update()
    {
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !collision.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
        {
            collision.gameObject.GetComponent<MainCharacterController>().damaged();
            Destroy(gameObject.transform.parent.gameObject);
        }

        if (collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
