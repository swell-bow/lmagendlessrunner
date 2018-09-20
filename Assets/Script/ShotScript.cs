using UnityEngine;

public class ShotScript : MonoBehaviour {

    HUDScript hud;
    Renderer rend;

    private void Awake()
    {
        hud = GameObject.FindObjectOfType<Camera>().GetComponent<HUDScript>();
        rend = GetComponent<Renderer>();
        rend.enabled = true;
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(1, 0));
        //Debug.Log("hit - " + (hit ? hit.distance.ToString() : "nohit"));
        if (hit && hit.distance < 0.55f)
        {
            hud.IncreaseScore(50);
            if (hit.collider.tag == "Enemy")
            {
                //Debug.Log("HIT");
                //var sound = hit.collider.gameObject.GetComponent<AudioSource>();
                //sound.Play();
                //rend.enabled = false;
                hit.collider.isTrigger = true;
                hit.collider.GetComponent<BoxCollider2D>().isTrigger = true;
                hit.collider.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                hit.collider.gameObject.GetComponent<Animator>().SetTrigger("Death");
                //Destroy(hit.collider.gameObject.GetComponent<BoxCollider2D>());
                //Destroy(hit.collider.gameObject.GetComponent<Rigidbody2D>());
                Destroy(hit.collider.gameObject, 0.75f); //, sound.clip.length);
                Destroy(this.gameObject);
            }
        }
    }
}
