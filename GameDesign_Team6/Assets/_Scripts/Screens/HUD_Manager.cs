using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* People who touched this script:
	Lauren Kirk
	Matt Bryan
*/

/*
 * Currently, this script is attached to the HUD UI. 
 * 
 * Future Implementations:
 *    (1) Might consider attaching it to an empty object which is strictly the GameManager. Then attach HUD to it.
 *    (2) After proposing 7-9 inventory spaces, add locking and unlocking capabilities to the UI manager
 *    (3) After proposing 7-9 inventory spaces, add little text bubbles labeling each box 1-9 (for player to use).
 *    (4) Consider restricting player screen size
 *    
 *  HUD LAYOUT PLAN:
 *    Upper Left - Health
 *    Upper Middle - Inventory
 *    Upper Right- Main Menu Select
 *    Lower Left - Mini Map
 *    Lower Middle - Current Level
 *    Lower Right - Selected Weapons
 */

public class HUD_Manager : MonoBehaviour {

    public Text level;
    //public Text gameOverText;
    private int currentLevel;
    private GameObject player;
    private GameObject winText;

	// Use this for initialization
	void Start () {
		// Start at Level 1
		GameManager.instance.SendMessage("setLevelDisplay");
		setLevelDisplay();
        player = GameObject.FindWithTag("Player");
        if (GameManager.instance.curScene == "BossBattle") {
            winText = GameObject.Find("WinText").gameObject;
            winText.SetActive(false);
        }
	}

    // Update is called once per frame
    void Update () {
        setLevelDisplay();
        setGoldDisplay();
	}

    private void setGoldDisplay() {
        GameObject lowerHud = this.gameObject.transform.Find("LowerHUD").gameObject;
        GameObject LevelGoldDisplayCanvas = lowerHud.transform.Find("LevelGoldDisplayCanvas").gameObject;
        GameObject GoldDisplayObject = LevelGoldDisplayCanvas.transform.Find("GoldDisplayText").gameObject;
        Text goldText = GoldDisplayObject.GetComponent<Text>();

        int playerGold = player.GetComponent<PlayerController>().getPlayerGold();

        goldText.text = "" + playerGold;
    }

	private void setLevelDisplay() {
		level.text = "FLOOR " + currentLevel;
	}

	public void setLevel(int level) {
		currentLevel = level;
	}

    public void displayGameOver() {
        GameObject pauseMenu = this.gameObject.transform.Find("LowerHUD").gameObject;
        pauseMenu.SetActive(false);

        GameObject gameOverDisplay = this.gameObject.transform.Find("MiddleHUD").gameObject;
        gameOverDisplay.SetActive(true);
    }

    public void displayWinMessage() {
        GameObject pauseMenu = this.gameObject.transform.Find("LowerHUD").gameObject;
        pauseMenu.SetActive(false);

        
        winText.GetComponent<Text>().text = "You Win!";
        winText.SetActive(true);
    }
}
