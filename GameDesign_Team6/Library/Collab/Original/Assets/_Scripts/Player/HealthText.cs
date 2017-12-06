using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthText : MonoBehaviour {

	private GameObject player;

	private Text healthText;
    private float playerHealth;
    private float playerMaxHealth;
    private int playerExp;
    private int playerLevel;
    private int playerNextLevelExp;
    private int playerGold;

	void Start () {
		healthText = GetComponent<Text>();
	}
	
	void Update () {
		// Can't find Player object until it is spawned, and the Start() function is called before that
		if (player == null) {
			player = GameObject.FindGameObjectWithTag("Player");
		}

        playerHealth = player.GetComponent<PlayerController>().healthPoints;
        playerMaxHealth = player.GetComponent<PlayerController>().maxHealthPoints;
        playerExp = player.GetComponent<PlayerController>().getCurrentExp();
        playerLevel = player.GetComponent<PlayerController>().level;
        playerNextLevelExp = player.GetComponent<PlayerController>().getRequiredXP();
        playerGold = player.GetComponent<PlayerController>().getPlayerGold();

		healthText.text = "Health: " + playerHealth + " / " + playerMaxHealth + " Level: " + playerLevel + " XP: " + playerExp + " / " + playerNextLevelExp + " Gold: " + playerGold;
	}
}