using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * People that worked on this script: 
 * Will Belden Brown, Lauren Kirk, Matt Bryan
**/

public class HealthText : MonoBehaviour {

	private GameObject player;

	private Text healthText;
	public Text scoreText;
    private float playerHealth;
    private float playerMaxHealth;
    private int playerExp;
    private int playerLevel;
    private int playerNextLevelExp;
    private int playerGold;
	private int playerScore;

	void Start () {
		healthText = GetComponent<Text>();
	}
	
	void Update () {
        // Can't find Player object until it is spawned, and the Start() function is called before that
        if (player == null) {
			player = GameManager.instance.player;
        }

        if (player.gameObject.name == "Peasant_Pete") {
			PlayerPrefs.SetInt("Pete Score", player.GetComponent<PlayerController>().getPlayerScore());
        }
        else if (this.gameObject.name == "Sorceress_Sera") {
			PlayerPrefs.SetInt("Sera Score", player.GetComponent<PlayerController>().getPlayerScore());
        }

        playerHealth = player.GetComponent<PlayerController>().healthPoints;
        playerMaxHealth = player.GetComponent<PlayerController>().maxHealthPoints;
        playerExp = player.GetComponent<PlayerController>().getCurrentExp();
        playerLevel = player.GetComponent<PlayerController>().level;
        playerNextLevelExp = player.GetComponent<PlayerController>().getRequiredXP();
        playerGold = player.GetComponent<PlayerController>().getPlayerGold();
		playerScore = player.GetComponent<PlayerController> ().getPlayerScore ();

		scoreText.text = "SCORE: " + playerScore.ToString();
		healthText.text = "Level: " + playerLevel + " XP: " + playerExp + " / " + playerNextLevelExp;
    }
}