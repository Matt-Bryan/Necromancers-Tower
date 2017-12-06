using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/**
	People who touched this script:
		Matt Bryan
        Will Belden Brown
        Matthew Stewart
        Lauren Kirk (Barely)

	This script has control over a majority of the game, as the title shows.

	IMPORTANT: Any scene changes in the game must go through the Game Manager.
	Use GameManager.instance.changeScene(sceneName) for any transitions.
**/


public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public GameObject selectedCharPrefab;
	public GameObject hudPrefab;
	
	public int levelNum = 1;
	private int bossLevelNum = 15;
	public string curScene;
	public GameObject levelGen;
    public GameObject dropLoot;
	public GameObject player;

	private GameObject hudInstance;
	private int saveIndex = -1;

	void Awake () {
		if (instance == null) {
			instance = this;
		}
		else {
			Destroy(gameObject);
		}
		DontDestroyOnLoad (gameObject);

		curScene = SceneManager.GetActiveScene().name;

        dropLoot = Instantiate(dropLoot, Vector2.zero, Quaternion.identity);
	}

	// Not entirely sure what these two functions do other than
	// allow the Game Manager to use OnSceneLoaded.
	// Advice: Probably leave these alone until we better understand them
	void OnEnable() {
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnDisable() {
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	// This function is run once every time a scene is loaded
	void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		Time.timeScale = 1;
		if (scene.name == "MainMenu") {
			// Main Menu Stuff Here
			resetStats();
			Destroy (player);
			saveIndex = -1;
		}
		else if (scene.name == "CharacterSelect") {
			// Character Selection Screen
		}
		else if (scene.name == "BossBattle") {
			GameObject.Find ("Main Camera").GetComponent<CameraController>().resetCamera ();
			player.transform.position = Vector3.zero;
			createHudInstance();
		}
		else if (scene.name == "MattTestScene") {
			// Main Scene
			if (player == null) {
				createPlayerInstance ();
			}
			player.SetActive(true);
			generateLevel();
			createHudInstance();
			GameManager.instance.save (-1);
		}

        else if (scene.name == "Shop") {
            player.SetActive(false);
        }
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

        if (Input.GetKeyDown("h")) {
            enableHaungs();
        }
	}

    private void enableHaungs() {
        player.GetComponent<PlayerController>().maxHealthPoints += 10000;
        player.GetComponent<PlayerController>().healthPoints += 10000;
        player.GetComponent<PlayerController>().attackDamage += 100;
        int gp = player.GetComponent<PlayerController>().getPlayerGold();
        player.GetComponent<PlayerController>().setPlayerGold(gp + 100000);
    }

	private void createHudInstance() {
		hudInstance = (GameObject)Instantiate(hudPrefab, Vector2.zero, Quaternion.identity);
		hudInstance.GetComponent<HUD_Manager>().setLevel(levelNum);
	}

	private void nextLevel() {
		player.SetActive(true);
		if (++levelNum == bossLevelNum) {
			changeScene("BossBattle");
		}
		else {
			changeScene("MattTestScene");
		}
	}

    private void toShop() {
		changeScene("Shop");
    }

	private void playerDied() {
		player.SetActive(false);
		hudInstance.GetComponent<HUD_Manager>().displayGameOver();
	}

	private void playerWon() {
		hudInstance.GetComponent<HUD_Manager>().displayWinMessage();
	}

	// always use this to create the player
	private void createPlayerInstance() {
		player = (GameObject) Instantiate(selectedCharPrefab, new Vector2(15, 15), Quaternion.identity);
		DontDestroyOnLoad (player);
	}

	private void generateLevel() {
		GameObject levelGeneratorObject = Instantiate(levelGen, Vector3.zero, Quaternion.identity);
		GenerateLevel levelGenerator = levelGeneratorObject.GetComponent<GenerateLevel> ();
		// 11 enemies at floor 1  with grid size of 30x30
		// 27 enemies at floor 5  with grid size of 42x42
		// 66 enemies at floor 10 with grid size of 57x57
		levelGenerator.numEnemies = (int)((20 + ((5 + 2.5 * levelNum) * (4 + 2.5 * levelNum)) / 5) / 3);
		levelGenerator.width = 12 + (levelNum + 5) * 3;
		levelGenerator.height = 12 + (levelNum + 5) * 3;
	}

	private void pause() {
		if (Time.timeScale == 1) {
			Time.timeScale = 0;
		}
		else {
			Time.timeScale = 1;
		}
	}

	// Used by the HUD_Manager script to set its level
	private void setLevelDisplay() {
		hudInstance.GetComponent<HUD_Manager>().setLevel(levelNum);
	}

	private void resetStats() {
		levelNum = 1;
	}

	public void changeScene(string sceneName) {
		// This debug statement is useful for debugging just about anything:
		// Debug.Log (sceneName);
		curScene = sceneName;
		SceneManager.LoadScene("LoadingScene");
	}

	public bool save(int index) {
		saveScore ();

		if (saveIndex < 0) {
			// new save
			if (index < 0) {
				saveIndex = SaveLoad.Save (-1);
				if (saveIndex < 0) {
					return false;
				}
			// load save
			} else {
				saveIndex = index;
				GameManagerData gmd = SaveLoad.Load (saveIndex);
				if (gmd != null) {
					loadData (gmd);
				} else {
					return false;
				}
			}
		} else {
			// overwrite save
			if (SaveLoad.Save (saveIndex) < 0) {
				return false;
			}
		}
		return true;
	}

	public void saveScore() {
		if (player) {
			PlayerController p = player.GetComponent<PlayerController> ();
			int score = p.getPlayerScore ();

            //Debug.Log("GAME OBJECT NAME: " + p.gameObject.name);
            if (p.gameObject.name == "Peasant_Pete(Clone)")
            {
                if (!PlayerPrefs.HasKey("Pete Score"))
                {
                    PlayerPrefs.SetInt("Pete Score", 0);
                }
                else
                {
                    if (PlayerPrefs.GetInt("Pete Score") < score)
                    {
                        PlayerPrefs.SetInt("Pete Score", score);
                    }
                }
            }

            if (p.gameObject.name == "Sorceress_Sera(Clone)")
            {
                if (!PlayerPrefs.HasKey("Sera Score"))
                {
                    PlayerPrefs.SetInt("Sera Score", 0);
                }
                else
                {
                    if (PlayerPrefs.GetInt("Sera Score") < score)
                    {
                        PlayerPrefs.SetInt("Sera Score", score);
                    }
                }
            }
		}
	}

	public GameManagerData createData () {
		if (player != null) {
			PlayerController p = player.GetComponent<PlayerController> ();
			return new GameManagerData (levelNum, curScene, selectedCharPrefab.name, p);
		} else {
			return new GameManagerData (levelNum, curScene, "", null);
		}
	}

	public void loadData (GameManagerData data) {
		levelNum = data.levelNum;
		curScene = data.curScene;
		if (data.characterName.Length > 0) {
			selectedCharPrefab = (GameObject)Resources.Load (data.characterName);
			if (player) {
				Destroy (player);
			}
			createPlayerInstance ();
			PlayerController p = player.GetComponent<PlayerController> ();
			p.healthPoints = data.curHP;
			p.maxHealthPoints = data.maxHP;
			p.xpValue = data.xpValue;
			p.setPlayerGold(data.playerGold);
			p.level = data.level;
		}
	}
}

[System.Serializable]
public class GameManagerData {
	
	public int levelNum;
	public string curScene;
	public string characterName;
	public float curHP;
	public float maxHP;
	public int xpValue;
	public int level;
    public int playerGold;
	public float attackDamage;

	public GameManagerData(int ln, string cs, string cn, PlayerController pc) {// float chp, float mhp, int xp, int l) {
		levelNum = ln;
		curScene = cs;
		characterName = cn;
		if (pc) {
			curHP = pc.healthPoints;
			maxHP = pc.maxHealthPoints;
			xpValue = pc.xpValue; 
			level = pc.level;
			attackDamage = pc.attackDamage;
            playerGold = pc.getPlayerGold();
		}
	}

}