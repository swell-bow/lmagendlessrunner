using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{


    public GameObject[] obj;
    public float spawnMin = 3f;
    public float spawnMax = 5f;

    // Use this for initialization
    void Start()
    {
        Spawn();
    }

    void Spawn()
    {
        Instantiate(obj[Random.Range(0, obj.Length)], transform.position, Quaternion.identity);
        Invoke("Spawn", Random.Range(spawnMin, spawnMax));
    }
}
