using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D myRB;
    private float speed = 4f;
    private Animator myAnimator;
    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>(); 
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
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
}
