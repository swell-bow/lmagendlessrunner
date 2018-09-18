using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{

    public float maxSpeed = 10f;
    private bool facingRight = true;

    Animator anim;

    //ground, falling
    private bool grounded = false;
    public Transform groundCheck;
    float groundRadius = 0.2f;
    public LayerMask whatIsGround;

    public float jumpForce = 700f;

    private bool doubleJump = false;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var rbVelocityY = GetComponent<Rigidbody2D>().velocity.y;

        //on the ground?
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        anim.SetBool("Ground", grounded);

        if (grounded)
            doubleJump = false;

        //vertical speed
        anim.SetFloat("vSpeed", rbVelocityY);
        //Debug.Log(rbVelocityY);

        //left right, change anim, 
        float move = Input.GetAxis("Horizontal");
        if (move > 0.01)
            Debug.Log("Move: " + move);
        anim.SetFloat("Speed", Mathf.Abs(move));

        GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, rbVelocityY);

        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();
    }

    private void Update()
    {
        //"do not use get key down - do input manager create jump axis or jump button"
        if ((grounded || !doubleJump) && Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool("Ground", false);
            //Debug.Log("update ground false");
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));

            if (!doubleJump && !grounded)
                doubleJump = true;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
