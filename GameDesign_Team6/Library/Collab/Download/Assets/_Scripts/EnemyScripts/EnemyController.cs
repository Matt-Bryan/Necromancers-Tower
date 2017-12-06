using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
	This script extends ActorController and represents any enemy we want it to.

	CURRENT ISSUES: Just like the player, the enemy's dying animation continues to play over and over, and
					the enemy doesn't destroy itself yet. Can be easily fixed later.

					I didn't put in any of the previous enemyController's advanced player-following stuff.
					It just straight up follows the player no matter how far.

					Following up on the previous point, the enemy continues to move towards the player
					until it's literally right on top of the player, where it will spaz back and forth
					until the player moves.

					As mentioned in the newPlayerScript, we need to look at how damage is applied to actors in our game 
					and make changes accordingly.
**/

public class EnemyController : ActorController {

	private GameObject player;
	private Rigidbody2D enemyRb;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		enemyRb = GetComponent<Rigidbody2D>();
	}

	// base.Update() must be called somewhere in the function (best to just leave it in as the first line)
    void Update () {
        base.Update();

        if (currentState != ObjectState.DIE) {
            moveEnemy(player.transform.position);
            setLookDirection();
        }
        else {
            enemyRb.velocity = Vector2.zero;
        }
    }

    private void setLookDirection() {
        // Value never used. Commenting out to remove warning in logs.
        // Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.rotation = Quaternion.Euler(0, (player.transform.position.x - transform.position.x) < 0 ? 180 : 0,0);
    }

	private void moveEnemy (Vector3 target) {
		Vector2 velocityVector = target - transform.position;
		enemyRb.velocity = velocityVector.normalized * moveSpeed;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Projectile")) {
			wasHit = true;
			Destroy (other.gameObject);

            // Value never used. Commenting out to remove warning in logs.
            // PlayerController playerScript = player.GetComponent<PlayerController>();

            //changeHealth(-playerScript.attackDamage);
            //We can change the way damage is applied, but right now the attacked actor changes their health based on
            //the other actor's attackDamage (a public variable)
        }

        if (other.CompareTag ("Player")) {
            isAttacking = true;
            //Player's health is modified by the player (Will have to be reworked for enemy projectiles)
            }
    }

	private void OnTriggerExit2D(Collider2D other) {
		if (other.CompareTag("Player")) {
			isAttacking = false;
		}
	}
}
