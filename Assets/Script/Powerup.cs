using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {

    HUDScript hud;
    AudioSource audioSource;
    Renderer rend;

    private void Awake()
    {
        hud = GameObject.FindObjectOfType<Camera>().GetComponent<HUDScript>();
        audioSource = GetComponent<AudioSource>();
        rend = GetComponent<Renderer>();
        rend.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Playa")
        {
            hud.IncreaseScore(10);
            Debug.Log("play audio on powerup");
            audioSource.Play();
            rend.enabled = false;
            Destroy(this.gameObject,audioSource.clip.length);
        }
    }
}
