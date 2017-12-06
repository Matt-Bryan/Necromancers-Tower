using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * People that touched this script: Allison Lee
**/


// I think this is all controlled by game manager now? - matt s

public class ActiveCharacter : MonoBehaviour {

	public GameObject selectedChar;

	void Start() {
		setPlayer();
	}

	void setPlayer(){
		//Instantiates the prefab of the selected character and parents it to the object Current_Player.
		//The prefab "selectedChar" can be changed on the start screen, and then setPlayer(); can be called
		// in order to change the player character being used.
		GameObject player;
		player = (GameObject) Instantiate(selectedChar, gameObject.transform.position, gameObject.transform.rotation);
		gameObject.transform.parent = player.transform;
	}
}
