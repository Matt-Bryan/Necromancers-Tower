using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/**
	People who touched this script:
		Matt Bryan
        Lauren Kirk (Barely)
**/

public class CharSelectButtonManager : MonoBehaviour {

	private GameManager GameManager;

	// Use this for initialization
	void Start () {
		GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	}

	public void startButton() {
        if (GameManager.selectedCharPrefab.name == "Peasant_Pete")
        {
            GameManager.changeScene("PeteStoryScene");
        }
        else if (GameManager.selectedCharPrefab.name == "Sorceress_Sera")
        {
            GameManager.changeScene("SeraStoryScene");
        }
        else
        {
            GameManager.changeScene("MattTestScene");
        }
    }

    public void resetBestScore() {
        Text score = GameObject.Find("BestScoreText").GetComponent<Text>();
        score.text = "0";

        PlayerPrefs.DeleteKey("Pete Score");
        PlayerPrefs.DeleteKey("Sera Score");
    }
}
