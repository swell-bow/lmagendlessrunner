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
        if (hit && hit.distance < 0.2f)
        {
            hud.IncreaseScore(50);
            if (hit.collider.tag == "Enemy")
            {
                Debug.Log("HIT");
                //var sound = hit.collider.gameObject.GetComponent<AudioSource>();
                //sound.Play();
                //rend.enabled = false;
                Destroy(hit.collider.gameObject);//, sound.clip.length);
                Destroy(this.gameObject);
            }
        }
    }
}
