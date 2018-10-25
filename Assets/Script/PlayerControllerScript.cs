using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public float jumpForce = 900f;

    HUDScript hud;

    public GameObject projectileShot;
    private float projectileShotSpeed = 2000f;

    //swipe stuff
    private Vector2 fingerDown;
    private Vector2 fingerUp;
    public bool detectSwipeOnlyAfterRelease = false;
    public float SWIPE_THRESHOLD = 20f;


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
        //Debug.Log("FixedUpdate grounded " + grounded);
        //vertical speed
        anim.SetFloat("vSpeed", rbVelocityY);
        //Debug.Log(rbVelocityY);

        //left right, change anim, 
        float move = 1f;

        anim.SetFloat("Speed", Mathf.Abs(move));

        GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, rbVelocityY);

    }

    private void Update()
    {
        CheckJump();

        PlayerRaycast();

        //touch stuff

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerUp = touch.position;
                fingerDown = touch.position;
            }

            //Detects Swipe while finger is still moving
            if (touch.phase == TouchPhase.Moved)
            {
                if (!detectSwipeOnlyAfterRelease)
                {
                    fingerDown = touch.position;
                    checkSwipe();
                }
            }

            //Detects swipe after finger is released
            if (touch.phase == TouchPhase.Ended)
            {
                fingerDown = touch.position;
                checkSwipe();
            }
        }
    }

    private void CheckJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void Jump(bool fromSwipe = false)
    {
        //Debug.Log("in jump " + grounded);
        if (grounded)
        {
            anim.SetBool("Ground", false);
            //Debug.Log("update ground false");
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce / (fromSwipe ? 2 : 1)));
        }
    }

    IEnumerator LoadLevelAfterDelay(float delay, string scene)
    {
        //Debug.Log("llad " + delay.ToString() + " " + scene);
        yield return new WaitForSeconds(delay);
        //Debug.Log("wtf mate!");
        SceneManager.LoadScene(scene);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hops")
        {
            Destroy(collision.gameObject);
            hud.IncreaseScore(100);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.tag);
        if (collision.collider.tag == "Enemy")
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOverScene");
        }
    }

    void PlayerRaycast()
    {
        return;
        //Debug.Log(transform.position);
        //FOR OUR OWN DEATH
        var origin = new Vector2(transform.position.x, transform.position.y - .3f);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.right);
        Debug.Log(System.DateTime.Now.ToLongTimeString() +  " PlayerRaycast " + hit.collider + " " + hit.distance + " " + (hit && hit.collider != null ? hit.collider.tag : ""));
        if (hit && hit.distance < 2f) // && hit.distance < 0.55f)
        {
            if (hit.collider.tag == "Enemy")
            {
                Destroy(this.gameObject, .75f); //, sound.clip.length);

                StartCoroutine(LoadLevelAfterDelay(1f, "GameOverScene"));

                //Destroy(hit.collider.gameObject);
                //Debug.Log("we are dead");
                ////this.gameObject.GetComponent<Animator>().SetTrigger("Hurt");
                //var thisRB = this.gameObject.GetComponent<Rigidbody2D>();
                //thisRB.AddForce(Vector2.up * 10);
                //thisRB.gravityScale = 0;
                //thisRB.freezeRotation = false;
                //this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                //this.gameObject.GetComponent<Animator>().SetTrigger("Hurt");
                ////Destroy(this.gameObject, .75f); //, sound.clip.length);

                //StartCoroutine(LoadLevelAfterDelay(2f, "GameOverScene"));

                ////LoadLevelAfterDelay(2f, "GameOverScene");
            }
        }

    }

    //swipe stuff


    void checkSwipe()
    {
        //Check if Vertical swipe
        if (verticalMove() > SWIPE_THRESHOLD && verticalMove() > horizontalValMove())
        {
            //Debug.Log("Vertical");
            if (fingerDown.y - fingerUp.y > 0)//up swipe
            {
                OnSwipeUp();
            }
            else if (fingerDown.y - fingerUp.y < 0)//Down swipe
            {
                OnSwipeDown();
            }
            fingerUp = fingerDown;
        }

        //Check if Horizontal swipe
        else if (horizontalValMove() > SWIPE_THRESHOLD && horizontalValMove() > verticalMove())
        {
            //Debug.Log("Horizontal");
            if (fingerDown.x - fingerUp.x > 0)//Right swipe
            {
                OnSwipeRight();
            }
            else if (fingerDown.x - fingerUp.x < 0)//Left swipe
            {
                OnSwipeLeft();
            }
            fingerUp = fingerDown;
        }

        //No Movement at-all
        else
        {
            //Debug.Log("No Swipe!");
        }
    }

    float verticalMove()
    {
        return Mathf.Abs(fingerDown.y - fingerUp.y);
    }

    float horizontalValMove()
    {
        return Mathf.Abs(fingerDown.x - fingerUp.x);
    }

    //////////////////////////////////CALLBACK FUNCTIONS/////////////////////////////
    void OnSwipeUp()
    {
        Jump(true);
    }

    void OnSwipeDown()
    {
        Debug.Log("Swipe Down");
    }

    void OnSwipeLeft()
    {
        Debug.Log("Swipe Left");
    }

    void OnSwipeRight()
    {
        Debug.Log("Swipe Right");
    }

}
