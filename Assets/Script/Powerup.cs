using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {

    HUDScript hud;

    private void Start()
    {
        hud = GameObject.FindObjectOfType<Camera>().GetComponent<HUDScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Playa")
        {
            hud.IncreaseScore(10);
            Destroy(this.gameObject);
        }
    }
}
