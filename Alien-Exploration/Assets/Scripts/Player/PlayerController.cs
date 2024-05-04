using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D myRB;
    private float speed = 4f;
    private Animator myAnimator;
    private int maxJumps = 2;
    private int amountJumps;
    [SerializeField] private LayerMask myLayerCol;
    private Vector2 directionWall;
    private bool onWall = false;
    private float speedWallFall = -1f;
    private float jumpFinish;
    private float forceWallJump = 2f;
    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>(); 
        myAnimator = GetComponent<Animator>();
        amountJumps = maxJumps;
        directionWall = new Vector2(0.2f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }

    private void FixedUpdate()
    {
        RayCast();
    }

    private void Move()
    {
        jumpFinish -= Time.deltaTime;

        if (!onWall && jumpFinish <= 0)
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
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && amountJumps > 0f && !onWall)
        {
            myRB.velocity = new Vector2(myRB.velocity.x, speed);
            myAnimator.SetBool("InFloor", false);
            if (amountJumps == maxJumps -1)
            {
                myAnimator.SetBool("DoubleJump", true);
            }
            amountJumps--;
        }

        if (Input.GetButtonDown("Jump") && onWall)
        {
            myRB.velocity = new Vector2((transform.localScale.x * -1) * speed, speed);
            jumpFinish = 0.2f;
        }

        if (myRB.velocity.y != 0f)
        {
            myAnimator.SetFloat("VSpeed", Mathf.Sign(myRB.velocity.y));
        }
    }

    private void EndAnimation(string parameter)
    {
        myAnimator.SetBool(parameter, false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }

    private bool RayCast()
    {
        BoxCollider2D myBoxCollider = GetComponent<BoxCollider2D>();
        bool rayCast = Physics2D.Raycast(myBoxCollider.bounds.center, Vector2.down, 0.4f, myLayerCol);

        myAnimator.SetBool("InFloor", rayCast);

        if (rayCast && myRB.velocity.y <= 0)
        {
            amountJumps = maxJumps;
            onWall = false;
        }

        bool circleCastRight = Physics2D.OverlapCircle(myBoxCollider.bounds.center + new Vector3(directionWall.x, 0f, 0f), 0.15f, myLayerCol);
        bool circleCastLeft = Physics2D.OverlapCircle(myBoxCollider.bounds.center + new Vector3(-directionWall.x, 0f, 0f), 0.15f, myLayerCol);

        if ((circleCastLeft || circleCastRight) && !rayCast)
        {
            onWall = true;
        }
        else
        {
            onWall = false;
        }

        myAnimator.SetBool("WallJump", false);
        if (onWall && jumpFinish <= 0 && myRB.velocity.y < 0)
        {
            myRB.velocity = new Vector2(myRB.velocity.x, speedWallFall);
            myAnimator.SetBool("WallJump", true);
        }

        Debug.DrawRay(myBoxCollider.bounds.center, Vector2.down * 0.4f);

        return rayCast;
    }

    private void OnDrawGizmos()
    {
        BoxCollider2D myBoxCollider = GetComponent<BoxCollider2D>();

        Gizmos.DrawWireSphere(myBoxCollider.bounds.center + new Vector3(directionWall.x, 0f, 0f), 0.15f);
        Gizmos.DrawWireSphere(myBoxCollider.bounds.center + new Vector3(-directionWall.x, 0f, 0f), 0.15f);
    }
}
