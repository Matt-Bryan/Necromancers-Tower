using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * People that touched this script: Matthew Stewart, Matt Bryan, Allison Lee
**/

/**
	This is going to be a special attack template. It should be attached to the special attack prefab, and it inherits from the
	Special Controller script.
**/

public class Peasant_Pete_TomatoExplosion : SpecialController {

	public override void Special(Vector3 position) {
		//Code for the special attack goes into this function.
		//The Shoot function is inside SpecialController, as well as PlayerController.
		//Overrides Special() in SpecialController, so don't change the name of this method.
		//position should be the current transform.position of the player charater.
		Vector2 shotVelocityVector = (new Vector2(0, 1));
		ShootFixed (shotVelocityVector, new Vector3(0, 0, 90), position);
		shotVelocityVector = (new Vector2(0, -1));
		ShootFixed (shotVelocityVector, new Vector3(0, 0, -90), position);
		shotVelocityVector = (new Vector2(-1, 0));
		ShootFixed (shotVelocityVector, new Vector3(0, 180, 0), position);
		shotVelocityVector = (new Vector2(1, 0));
		ShootFixed (shotVelocityVector, new Vector3(0, 0, 0), position);
	}
}
