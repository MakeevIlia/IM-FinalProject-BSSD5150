using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody2D))]
public class MainCharacterController : MonoBehaviour
{
    [SerializeField]
    private Transform GroundCheck;
    [SerializeField]
    private LayerMask GroundLayer;
    [SerializeField]
    private Collider2D swordCollider;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private Transform projectileSpawnPointLeft;
    [SerializeField]
    private Transform projectileSpawnPointRight;
    private bool grounded = true;
    private float checkRadius = 0.3f;

    private float speed = 2.5f;
    private float jumpForce = 5.0f;

    private int health = 2;

    public int swordsLeft = 3;
    private const string dificultyLevelKey = "dificulty";
    Animator anim;

    private Rigidbody2D rb2d;

    private bool alive = true;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        swordCollider.enabled = false;
        CheckPlayerPrefs();
    }

    void CheckPlayerPrefs()
    {
        int choice = PlayerPrefs.GetInt(dificultyLevelKey, 0);
        GameObject pan = GameObject.Find("Panel");
        Image[] hudSwords = pan.GetComponentsInChildren<Image>();
        if (choice == 0)
        {
            swordsLeft = 3;
        }
        else if (choice == 1)
        {
            swordsLeft = 2;
        }
        else
        {
            swordsLeft = 1;
        }
        for (int i = 3; i > swordsLeft; i--) 
        {
            hudSwords[i].enabled = false;
        }
    }

    [System.Obsolete]
    private void Update()
    {
        float h = 0f;
        float v = 0f;

        if (alive)
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
        }
       
        if (Input.GetMouseButtonDown(1) && swordsLeft > 0)
        {
            anim.SetTrigger("Throw");
            GameObject pan = GameObject.Find("Panel");
            Image[] hudSwords = pan.GetComponentsInChildren<Image>();
            hudSwords[swordsLeft].enabled = false;

            if (!gameObject.GetComponent<SpriteRenderer>().flipX)
            {
                GameObject knife = Instantiate(Resources.Load("Knife"), projectileSpawnPointRight.transform.position,
                    Quaternion.identity) as GameObject;
            }else
            {
                GameObject knife = Instantiate(Resources.Load("Knife"), projectileSpawnPointLeft.transform.position,
                    Quaternion.identity) as GameObject;
            }

                
            swordsLeft--;
        }

        Vector2 movement = rb2d.velocity;

        if (h > 0.01f)
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        else if (h < -0.01f)
            gameObject.GetComponent<SpriteRenderer>().flipX = true;

        //walking movement
        if (grounded)
        {
            movement.x = h * speed;
        }
        else
        {
            movement.x = h * speed / 2;
        }
        
        //jumping movement
        if (v > 0 && grounded)
        {
            movement.y = v * jumpForce;
        }

        rb2d.velocity = movement;

        //walking animation
        if (h != 0 && grounded)
        {
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
        }

        //jumping animation
        if (movement.y < 0)
        {
            anim.SetBool("Falling", true);
            anim.SetBool("Jumping", false);
        }
        else if (movement.y > 0)
        {
            anim.SetBool("Jumping", true);
            anim.SetBool("Falling", false);
        }
        else
        {
            anim.SetBool("Falling", false);
            anim.SetBool("Jumping", false);
        }

        //attacking animation
        if (Input.GetKeyDown("space"))
        {
            anim.SetBool("Attacking", true);
        }
        else
        {
            anim.SetBool("Attacking", false);
        }

       
    }


    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(GroundCheck.position, checkRadius, GroundLayer);
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            swordCollider.enabled = true;
        }
        else
        {
            swordCollider.enabled = false;
        }

        if (health <= 0)
        {
            alive = false;
            anim.SetTrigger("Death");
            health = 3;
            StartCoroutine(deathhappened());
            /*GameObject[] objs = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject obj in objs) 
            {
                
            }*/
        }

    }

    IEnumerator deathhappened()
    {
        yield return new WaitForSeconds(2);
        gameObject.transform.position = spawnPoint.position;
        alive = true;
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && alive)
        {
            damaged();
        }

        if (collision.gameObject.tag == "Knife" && alive)
        {
            GetKnife();
            Destroy(collision.gameObject.transform.parent.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyWithKnife" && alive)
        {
            GetKnife();
            collision.gameObject.GetComponent<Animator>().SetTrigger("OutKnifing");
            collision.gameObject.tag = "Enemy";
        }
    }


    public void damaged()
    {
        health--;
        Vector2 currPos = rb2d.position;
        Vector3 change = transform.right;
        Vector2 move;
        move.x = currPos.x - 2 * change.x;
        move.y = currPos.y - 2 * change.y;
        rb2d.MovePosition(move);
        anim.SetTrigger("Damaged");
    }
    private void GetKnife()
    {
        swordsLeft++;
        GameObject pan = GameObject.Find("Panel");
        Image[] hudSwords = pan.GetComponentsInChildren<Image>();
        hudSwords[swordsLeft].enabled = true;
    }
}
