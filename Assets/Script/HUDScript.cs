using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDScript : MonoBehaviour {

    float playerScore = 0f;

    // Update is called once per frame
    void Update () {
        playerScore += Time.deltaTime;
        //Debug.Log(Time.deltaTime + " " + GameData.PlayerScore);
	}

    public void IncreaseScore(int amount)
    {
        playerScore += amount;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 30), "Score: " + (int)playerScore);
    }

    private void OnDisable()
    {
        GameData.PlayerScore = (int)playerScore;
    }
}
