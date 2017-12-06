using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Touched by: Allison Lee, Matthew Stewart **/
/**
    This class represents any "special" attack. All special attack classes will inherit this one and expand
    upon it. All helper functions for attacks can go in here.
**/

public class SpecialController : MonoBehaviour {
    // used when instantiating a new projectile for basic attack
    public int attackSpeed;
	public GameObject attackPrefab;
    public float attackRange;

	private float attackDamage = 0;

	public void ShootFixed(Vector2 shotVelocityVector, Vector3 rotation, Vector3 position) {
		//Shoots a projectile in a fixed direction given a velocity vector and a sprite rotation.
		GameObject shot = Instantiate (attackPrefab, position, Quaternion.identity);
		shot.transform.Rotate (rotation);
		Rigidbody2D shotRb = shot.GetComponent<Rigidbody2D>();

		//Normalize velocity vector, multiply times projectile speed, add force to shot
		shotRb.AddForce (shotVelocityVector.normalized * attackSpeed);

		shot.gameObject.GetComponent<ProjectileDie>().attackRange = attackRange;
		shot.gameObject.GetComponent<ProjectileDie>().attackDamage = attackDamage;
		shot.gameObject.GetComponent<ProjectileDie>().originalPosition = position;
	}

	public void setDamage (float damage) {
		attackDamage = damage;
	}

	public void Shoot(Vector3 position) {
		//Velocity vector from player to mouse position
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 shotVelocityVector = ((Vector2)mousePos - (Vector2)position);

		//Uses the ShootFixed method to send a projectile in that direction.
		ShootFixed (shotVelocityVector, new Vector3 (0, 0, 0), position);
	}

	virtual public void Special (Vector3 position)
	{
		//This method is overridden in the Special classes for each attack.
	}
}
