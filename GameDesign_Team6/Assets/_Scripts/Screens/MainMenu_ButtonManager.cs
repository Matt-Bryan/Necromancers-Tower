using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/**
 * People that touched this script:
 * Lauren
 * Matthew Stewart
**/

public class MainMenu_ButtonManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject instructionsMenu;
	public GameObject loadMenu;
	public Text OverrideText;
	public Text LoadGame1;
	public Text LoadGame2;
	public Text LoadGame3;

	public void Start() {
		for (int i = 0; i < 3; i++) {
			GameManagerData gmd = SaveLoad.Load (i);
			if (gmd != null) {
				int level = gmd.levelNum;
				string str = "";
				if (gmd.characterName != "") {
					str = gmd.characterName.Replace("_", " ") + ": ";
				} else {
					str = "No character selected: ";
				}
				str = str + "Floor " + level.ToString ();
				if (i == 0) {
					LoadGame1.text = str;
				} else if (i == 1) {
					LoadGame2.text = str;
				} else if (i == 2) {
					LoadGame3.text = str;
				}
			}
		}
	}

    public void NewGame_Button(string NewGameLevel)
	{
		if (!GameManager.instance.save (-1)) {
			ContinueGame_Button (false);
			OverrideText.text = "Choose a save to override";
		} else {
			GameManager.instance.changeScene (NewGameLevel);
		}
	}

	public void ContinueGame_Button(bool loadMain)
	{
		mainMenu.SetActive (loadMain);
		loadMenu.SetActive (!loadMain);
		if (loadMain) {
			OverrideText.text = "Choose a save to load";
		}
	}

	public void LoadSave_Button(int index) {
		if (OverrideText.text == "Choose a save to override") {
			SaveLoad.Save (index);
		}
		GameManager.instance.save (index);
		if (GameManager.instance.player != null) {
			GameManager.instance.changeScene ("MattTestScene");
		} else {
			GameManager.instance.changeScene ("CharacterSelect");
		}
	}

    public void ExitGame_Button()
    {
	    Application.Quit ();
    }

    public void InstructionsButton()
    {
        // Launch Instructions Screen
        instructionsMenu.SetActive(true);
    }

    public void LeaveInstructionsButton()
    {
        // Launch Instructions Screen
        instructionsMenu.SetActive(false);
    }
}

