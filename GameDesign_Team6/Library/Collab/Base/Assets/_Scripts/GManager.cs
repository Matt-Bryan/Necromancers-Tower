using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GManager : MonoBehaviour {

	public static GManager instance = null;
	public GameObject selectedCharPrefab;
	
	public int levelNum = 1;
	public string curScene;
	public GameObject levelGen;

	private GameObject player;

	void Awake () {
		if (instance == null) {
			instance = this;
		}
		else {
			Destroy(gameObject);
		}
		DontDestroyOnLoad (gameObject);

		curScene = SceneManager.GetActiveScene().name;

		generateLevel();
	}
	
	void Update () {
		if (curScene == "MainMenu") {
			// Main Menu Stuff Here
		}
		else if (curScene == "CharacterSelect") {
			// Character Selection Screen
		}
		else if (curScene == "Main") {
			// Main Scene
		}
	}

	private void nextLevel() {
		changeScene("MattTestScene");
	}

	private void playerDied() {
		changeScene("MattTestScene");
	}

	private void generateLevel() {
		player = (GameObject) Instantiate(selectedCharPrefab, new Vector2(15, 15), Quaternion.identity);
		Instantiate(levelGen, Vector3.zero, Quaternion.identity);
		//camera.GetComponent<CameraController>().followedObject = player;
	}

	private void pause() {
		if (Time.timeScale == 1) {
			Time.timeScale = 0;
		}
		else {
			Time.timeScale = 1;
		}
	}

	public void changeScene(string sceneName) {
		curScene = sceneName;
		SceneManager.LoadScene(sceneName);
	}
}
