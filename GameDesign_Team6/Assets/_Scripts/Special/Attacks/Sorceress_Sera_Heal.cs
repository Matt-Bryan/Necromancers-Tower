using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * People that touched this script: Allison Lee
**/

public class Sorceress_Sera_Heal : SpecialController {
	public override void Special(Vector3 position) {
		//Code for the special attack goes into this function.
		//The Shoot function is inside SpecialController, as well as PlayerController.
		//Overrides Special() in SpecialController, so don't change the name of this method.
		//position should be the current transform.position of the player charater.
		//Sera's special should heal the player by a certain amount.

		GameObject player = GameObject.FindWithTag ("Player");

		float health = player.GetComponent<PlayerController>().healthPoints;
		float maxHealth = player.GetComponent<PlayerController> ().maxHealthPoints;
        int addedHealth = (int)(maxHealth - health) / 5;
		if (health >= maxHealth) {
			//If the player already has max health, we do nothing.
			return;
		}

		if (health + addedHealth >= maxHealth) {
			player.GetComponent<PlayerController> ().healthPoints = maxHealth;
		} else {
			player.GetComponent<PlayerController> ().healthPoints += addedHealth;
		}
		//Heals 5 HP when her special is used. If this would bring her over max health, set health to max.

		GameObject newEffect = Instantiate (attackPrefab, player.transform.position, Quaternion.identity);
		newEffect.transform.parent = player.transform;
	}
}
