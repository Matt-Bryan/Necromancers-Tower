using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

	public Rigidbody2D enemyRb;
	public GameObject player;
	public int detectRange = 20;
	public float goBackDistance = 0.5f;
	public float enemySpeed = 10.0f;
	public int health = 10;
	public Text enemyHealthText;

	private bool attacking = false;

	void Update () {
		if (!attacking) {
			moveTowardsPosition (player.transform.position);
		} else {
			moveTowardsPosition (transform.position + (transform.position - player.transform.position));
		}
		if (health <= 0) {
			Destroy (this.gameObject);
		}
		enemyHealthText.text = "Enemy Health: " + health;

		//Face the sprite towards the player position
		transform.rotation = Quaternion.Euler(0, (player.transform.position.x - transform.position.x) < 0 ? 180 : 0,0);
	}

	private void moveTowardsPosition(Vector3 target) {
		Vector2 curTargetPos = new Vector2 (target.x, target.y);
		Vector2 curPos = new Vector2 (transform.position.x, transform.position.y);

		float distToTarget = Vector2.Distance (curPos, curTargetPos);
		if (!attacking) {
			if (distToTarget <= detectRange) {
				moveEnemy (target);
			} else {
				enemyRb.velocity = Vector2.zero;
			}
		} else {
			if (distToTarget <= goBackDistance * enemySpeed) {
				moveEnemy (target);
			} else {
				attacking = false;
			}
		}
	}

	private void moveEnemy (Vector3 target) {
		Vector2 velocityVector = target - transform.position;
		enemyRb.velocity = velocityVector.normalized * enemySpeed;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Projectile")) {
			int damage = 1;
			Destroy (other.gameObject);
			health -= damage;
		}
		if (other.CompareTag ("Player")) {
			attacking = true;
		}
	}
}
