using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMove : MonoBehaviour {

    public int enemySpeed = 2;
    public int xMoveDirection = -1;

    void Update()
    {
        var rb = gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = new Vector2(xMoveDirection, 0) * enemySpeed;
    }

}
