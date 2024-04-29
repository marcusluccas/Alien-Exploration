using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D myRB;
    private float speed = 4f;
    private Animator myAnimator;
    private int maxJumps = 2;
    private int amountJumps;
    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>(); 
        myAnimator = GetComponent<Animator>();
        amountJumps = maxJumps;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        float horizontalSpd = Input.GetAxisRaw("Horizontal") * speed;
        myRB.velocity = new Vector2(horizontalSpd, myRB.velocity.y);
        myAnimator.SetBool("Move", false);

        if (myRB.velocity.x != 0f)
        {
            transform.localScale = new Vector3(Mathf.Sign(myRB.velocity.x), 1f, 1f);
            myAnimator.SetBool("Move", true);
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && amountJumps > 0f)
        {
            myRB.velocity = new Vector2(myRB.velocity.x, speed);
            amountJumps--;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            amountJumps = maxJumps;
        }
    }
}
