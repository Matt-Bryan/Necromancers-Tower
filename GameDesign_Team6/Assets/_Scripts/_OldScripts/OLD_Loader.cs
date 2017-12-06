using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OLD_Loader : MonoBehaviour {

	public GameObject gameManager;

	// Use this for initialization
	void Awake () {
		if (OLD_GameManager.instance == null) {
			Instantiate (gameManager);
		}
	}
}
