using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D myRB;
    private float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>(); 
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

        if (myRB.velocity.y != 0f)
        {
            transform.localScale = new Vector3(Mathf.Sign(myRB.velocity.x), 1f, 1f);
        }
    }
}
