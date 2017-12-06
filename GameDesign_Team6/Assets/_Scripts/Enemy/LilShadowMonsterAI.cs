using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * People that touched this script:
 * Lauren Kirk
 * Matthew Stewart
**/

/*
 *  This script shows sample behaviour of a sprite wandering back and forth. This script
 *  assumes the initial animation of the sprite is facing right.
 *  
 *  Future implementation:
 *     (1) Wandering sprite following a path more detailed than +/- x-direction
 *     (2) Modify script to handle sprite facing either left or right
 */
public class LilShadowMonsterAI : MonoBehaviour {

    public GameObject playerObject; 
	public Rigidbody2D enemyRb;

    public int maxWanderLeft = -1; //Note: this value should always be less than 0
    public int maxWanderRight = 1; //Note: this value should always be greater than 0
    public int detectRange = 20;
	public float goBackDistance = 0.5f;
	public float enemySpeed = 10.0f;
	public int health = 10;
	public Text enemyHealthText;

	private bool attacking = false;

    private bool goingRight;
    private bool facingRight;

    // Use this for initialization
    void Start () {
        goingRight = !generateRandomBoolean();
        facingRight = goingRight;

        // Flip right-facing sprite, left
        if (!goingRight && !facingRight)
            transform.localRotation = Quaternion.Euler(0, 180, 0);
    }

    // Update is called once per frame
    void Update () {
        hurtByTouch();
        walkForward();
		if (!attacking) {
			moveTowardsPosition (playerObject.transform.position);
		} else {
			moveTowardsPosition (transform.position + (transform.position - playerObject.transform.position));
		}
		if (health <= 0) {
			Destroy (this.gameObject);
		}
		enemyHealthText.text = "Enemy Health: " + health;

		//Face the sprite towards the player position
		transform.rotation = Quaternion.Euler(0, (playerObject.transform.position.x - transform.position.x) < 0 ? 180 : 0,0);
    }

    private bool generateRandomBoolean()
    {
        // Note: Random.value returns a number within the range 0.0 (inclusive) to 1.0 (non-inclusive).
        //       Value is multiplied by 100 to generate a range from 0 (inclusive) to 100 (non-inclusive)
        //       in this way, one can use probabiltyTrue to generate a boolean with a probability of being
        //       true the assigned percentage of time (give or take a percentage point).

        int randomInt = (int) (Random.value * 100);
        int probabilityTrue = 50;

        //Debug.Log("RandomInt: " + randomInt + " ? " + probabilityTrue);

        return randomInt < probabilityTrue;
    }

    private void verticallyFlipSprite()
    {
        if (facingRight && !goingRight)
        {
            // FlipAnimation to left
            transform.localRotation = Quaternion.Euler(0, 180, 0);

            // Set facingRight as false, because now facing left
            facingRight = false;
        }
        else if (!facingRight && goingRight)
        {
            // FlipAnimation to right
            transform.localRotation = Quaternion.Euler(0, 0, 0);

            // Set facingRight as true, because now facing right
            facingRight = true;
        }
    }

    private void walkForward() {
        verticallyFlipSprite();

        // Move Sprite
        if (goingRight)
        {
            transform.position = new Vector3(transform.position.x + enemySpeed * Time.deltaTime, transform.position.y, transform.position.z);
            //Debug.Log("goingRight: " + transform.position.x);
        }
        else //if (!goingRight)
        {
            transform.position = new Vector3(transform.position.x - enemySpeed * Time.deltaTime, transform.position.y, transform.position.z);
            //Debug.Log("goingLeft: " + transform.position.x);
        }

        // Modify direction of Sprite if exceeding walking range
        if (maxWanderLeft > transform.position.x)
        {
            goingRight = true;
        }
        else if (maxWanderRight < transform.position.x)
        {
            goingRight = false;
        }
    }

    // Note: uses for this function can facilitate causing the player object harm or causing 
    // this object harm (indirectly from contact rather than directly through an attack).
    private void hurtByTouch() {
        float distance = Vector2.Distance(
            playerObject.transform.position,
            this.transform.position);
        float ouchDistance = 0.5f;

        if ((distance - ouchDistance) < 0) {
            Debug.Log("Ouch! He's touching me!");
        }
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
