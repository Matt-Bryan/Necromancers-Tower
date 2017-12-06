using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * People that touched this script: Allison Lee
**/

public class Game_ButtonManager : MonoBehaviour {

	public GameObject PopupCanvas;

	void Start()
	{
		//Make sure the popup is off by default.
		PopupCanvas.SetActive(false);
	}

	public void OnClickButton(string choice) {
		if( choice == "continue") {
			PopupCanvas.SetActive(false);
			Time.timeScale = 1.0f;
		}
		if (choice == "open_popup")
		{
			PopupCanvas.SetActive(true);
			Time.timeScale = 0f;
		}
	}
}
