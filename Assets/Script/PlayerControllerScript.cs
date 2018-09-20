using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControllerScript : MonoBehaviour
{
    public bool AllowTwoWay = false;

    public float maxSpeed = 10f;
    private bool facingRight = true;

    Animator anim;

    //ground, falling
    private bool grounded = false;
    public Transform groundCheck;
    float groundRadius = 0.2f;
    public LayerMask whatIsGround;

    public float jumpForce = 900f;

    private bool doubleJump = false;

    HUDScript hud;

    public GameObject projectileShot;
    private float projectileShotSpeed = 2000f;

    // Use this for initialization
    void Awake()
    {
        anim = GetComponent<Animator>();
        hud = GameObject.FindObjectOfType<Camera>().GetComponent<HUDScript>();
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
        float move = 0.7f;
        if (AllowTwoWay)
            move = Input.GetAxis("Horizontal");

        anim.SetFloat("Speed", Mathf.Abs(move));

        GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, rbVelocityY);

        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject proj = Instantiate(projectileShot, transform.position, Quaternion.identity) as GameObject;
            proj.GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0) * 20;

        }

        //"do not use get key down - do input manager create jump axis or jump button"
        if ((grounded || !doubleJump) && Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool("Ground", false);
            //Debug.Log("update ground false");
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));

            if (!doubleJump && !grounded)
                doubleJump = true;
        }

        PlayerRaycast();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void PlayerRaycast()
    {
        //FOR OUR OWN DEATH
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right);
        if (hit && hit.distance < 0.55f)
        {
            if (hit.collider.tag == "Enemy")
            {
                Destroy(this.gameObject);
                SceneManager.LoadScene("GameOverScene");
            }
        }

        //for clobbering enemies from above
        RaycastHit2D rayDown = Physics2D.Raycast(transform.position, Vector2.down);

        //Debug.Log(rayDown.collider.tag + " " + rayDown.distance);
        if (rayDown && rayDown.distance < 1f)
        {
            //Debug.Log("wtf got it???");
            //how do we do this properly????
            if (rayDown.collider.tag == "Enemy")
            {
                //Debug.Log("touched Enemy");
                //make bounce up!
                GetComponent<Rigidbody2D>().AddForce(Vector2.up * 1000);
                //kill enemy with animation
                var enemyRB = rayDown.collider.gameObject.GetComponent<Rigidbody2D>();
                enemyRB.AddForce(Vector2.right * 200);
                enemyRB.gravityScale = 20;
                enemyRB.freezeRotation = false;
                rayDown.collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                rayDown.collider.gameObject.GetComponent<EnemyMove>().enabled = false;
                //Destroy(hit.collider.gameObject);
                hud.IncreaseScore(100);

                //var sound = rayDown.collider.gameObject.GetComponent<AudioSource>();
                //sound.Play();
            }

        }
    }

}
