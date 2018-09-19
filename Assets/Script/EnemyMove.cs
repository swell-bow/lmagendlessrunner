using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMove : MonoBehaviour {

    public int enemySpeed = 2;
    public int xMoveDirection = -1;

    private void Start()
    {
        //enemySpeed = 2;
        //xMoveDirection = -1;
    }

    void Update()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(xMoveDirection, 0) * enemySpeed;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(xMoveDirection, 0));
        if ( hit && hit.distance < 0.4f)
        {
            if (hit.collider.tag == "Playa")
            {
                Destroy(hit.collider.gameObject);
                SceneManager.LoadScene("GameOverScene");
            }
        }
    }

}
