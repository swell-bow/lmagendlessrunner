using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour {

    int score = 0;

    private void Start()
    {
        score = GameData.PlayerScore;
    }
    private void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - 40, 50, 200, 30), "GAME OVER MAN");
        GUI.Label(new Rect(Screen.width / 2 - 40, 300, 200, 30), "Score: " + score);

        GUI.Label(new Rect(Screen.width / 2 - 40, 390, 150, 30), "Or press fire");

        if (GUI.Button(new Rect(Screen.width / 2 -30, 350, 100, 30), "Play Again") || Input.GetButtonDown("Fire1"))
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}
