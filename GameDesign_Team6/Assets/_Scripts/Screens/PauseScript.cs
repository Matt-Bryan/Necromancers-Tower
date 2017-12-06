using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * People that touched this script:
 *    Lauren Kirk
 *
 *   This class handles pauseMenu / pauseButton functionality:
 *   1. Pause Game
 *   2. Resume Game
 *   3. Access Instructions (done from Pause Menu)
 *   4. Return to Main Menu (done from Pause Menu)
 *   5. Exit Game (done from Pause Menu)
 *
 *   Further Improvements:
 *      - Add a "Save Game" button, or make such behaviour implicit to "Exit Game" or return to "Main Menu" button selections.
 *      - Restrict the player from mashing the attack keys while is a "paused" state.
 *      - When clicking the exit button:
 *          1. Consider adding implicit save-game state functionality.
 *          2. Show main character dying before exitting game ? LAY ON THE GUILT.
 */ 

public class PauseScript : MonoBehaviour {

    public GameObject pauseButton;
    public GameObject pauseMenu;
    public GameObject instructionsMenu;

    // NOTE: This might be a useful value for the gameManager
    //       to access based on how we want to handle pausing
    //       functionality in general
    public bool paused;

    public void PauseButton() {
		GameManager.instance.save (-1);

        // Hide Button Object
        pauseButton.SetActive(false);

        // Pause Game
        paused = true;
        Time.timeScale = 0;

        // Launch Pause Screen
        pauseMenu.SetActive(true);
    }

    public void ResumeButton() {
        // Hide Pause Screen
        pauseMenu.SetActive(false);

        // UnPause Game
        paused = false;
        Time.timeScale = 1;

        // Show Pause Button
        pauseButton.SetActive(true);
    }

    public void InstructionsButton() {
        // Launch Instructions Screen
        instructionsMenu.SetActive(true);
    }

    public void LeaveInstructionsButton() {
        // Launch Instructions Screen
        instructionsMenu.SetActive(false);
    }

    public void MainMenuButton() {
        // TODO: Change string to real main menu
		GameManager.instance.changeScene("MainMenu");
    }

    public void ExitGameButton() {
		Application.Quit();
    }

	public void BossBattleButton() {
		//Object.DontDestroyOnLoad (GetComponent<GameManager>());
		GameObject.Find ("Main Camera").GetComponent<CameraController>().resetCamera ();
		PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController> ();
		player.healthPoints = 500;
		player.maxHealthPoints = 500;
		player.moveSpeed = 6;
		player.attackDamage = 20;
		player.level = 20;
		player.attackSpeed = 250;
		GameManager.instance.changeScene ("BossBattle");
	}

    public void SoundsButton() {
        // TODO: Look up tutorial for this
        Debug.Log("Write Me");
    }

}
