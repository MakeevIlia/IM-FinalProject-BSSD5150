using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SwordController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyController>().alive = false;
            collision.gameObject.GetComponent<Animator>().SetTrigger("Death");
            collision.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }

        if (collision.gameObject.tag == "Bullet")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = -1 * collision.gameObject.GetComponent<Rigidbody2D>().velocity;
        }
    }
}

