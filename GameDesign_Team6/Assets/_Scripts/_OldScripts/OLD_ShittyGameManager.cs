using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

public class ShittyGameManager : MonoBehaviour {

    public Text level;
    private int currentLevel;

	// Use this for initialization
	void Start () {
        // Start at Level 1
        currentLevel = 1;
        setLevelDisplay();

    }
	
	// Update is called once per frame
	void Update () {
        GoToNextLevel();
        setLevelDisplay();
    }

    private void GoToNextLevel() {
        // Fake Level Progression
        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("TODO: Next Level Obtained (Currently Faked)!");
            currentLevel++;
        }
    }

    private void setLevelDisplay() {
        level.text = "LEVEL " + currentLevel;
    }
}
