using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * People that touched this script: Matthew Stewart, Matt Bryan, Allison Lee
**/

/**
	This is going to be the new PlayerController script. It should be attached to the player object, and it inherits from the
	Actor Controller script, which does the main handling of changing the object's state and animations. Check Actor Controller
	script for notes on those topics.

	This will probably need to be looked at and changed when we do our "standardization" meeting, but it works for now.

	CURRENT ISSUES: When the player dies, the death animation cycles over and over and the player doesn't actually die.
					Can be easily fixed later once we're all on the same page.

					Not really an issue, but we need to think about how damage is applied to actors in our game and
					make code changes accordingly.
**/

public class PlayerController : ActorController {

	public SpecialController SpecialAttack1;

	public float SpecialAttackTimer = 0.5f;	// Cooldown between special attacks

	private float Timer = 0.0f;					// Used to check time between special attacks

	private Rigidbody2D PlayerRigidBody;

	// Is the player standing on the exit tile? (Used in order to hit E to exit)
	private bool onExit = false;

	private bool soundPlaying = false;

	// just an example for now
	private Dictionary<int, int> xpValuesTable = new Dictionary<int, int>()
	{
		{ 1, 100 },
		{ 2, 175 },
		{ 3, 250 }
	};

	// can also use something like:
	private int getRequiredXP () {
		return 25 + level * 75;
	}

	// Use this for initialization
	void Start () {
 		SpecialAttack1.setDamage (attackDamage);
		PlayerRigidBody = GetComponent<Rigidbody2D>();
	}

	// base.Update() must be called somewhere in the function (best to just leave it in as the first line)
	void Update() {
		base.Update();

		Timer += Time.deltaTime;
		if (currentState != ObjectState.DIE) {
			setLookDirection();
			takeInput();
		}

		if (isMoving && !soundPlaying) {
			footstepSource.Play();
			soundPlaying = true;
		}
		else if (!isMoving && soundPlaying) {
			footstepSource.Stop();
			soundPlaying = false;
		}
	}
	
	void FixedUpdate () {
		if (currentState != ObjectState.DIE) {
			movePlayer();
		}
		else {
			PlayerRigidBody.velocity = Vector2.zero;
		}
	}
		
	void checkIfLevelUp () {
		// one way of doing it:
		if (xpValue > xpValuesTable[level]) {
			level++;
			xpValue -= xpValuesTable [level];
		}
		// other example way of doing it:
		if (xpValue > getRequiredXP ()) {
			level++;
			xpValue -= getRequiredXP ();
		}
	}

	// TODO: Have this call ActorController basic attack method after interpreting mousePos

	private void Shoot(Vector2 mousePos) {
		GameObject shot = Instantiate (attackPrefab, transform.position, Quaternion.identity);
		Rigidbody2D shotRb = shot.GetComponent<Rigidbody2D>();

		//Velocity vector from player to mouse position
		Vector2 shotVelocityVector = ((Vector2)mousePos - (Vector2)transform.position);

		//Normalize velocity vector, multiply times projectile speed, add force to shot
		shotRb.AddForce (shotVelocityVector.normalized * attackSpeed);

		//Destroys the projectile when it moves out of range.
		shot.gameObject.GetComponent<ProjectileDie>().attackRange = attackRange;
		shot.gameObject.GetComponent<ProjectileDie>().attackDamage = attackDamage;
		shot.gameObject.GetComponent<ProjectileDie>().originalPosition = gameObject.transform.position;

	}

	private void takeInput() {
		// Call pause function if ESC is pressed
		// THIS MUST GO ABOVE EVERYTHING ELSE IN ORDER TO BE ABLE TO UNPAUSE
		if (Input.GetKeyDown(KeyCode.Escape)) {
			GameManager.instance.SendMessage("pause");
			isMoving = !isMoving;
		}

		if (Time.timeScale == 0) {
			return;
		}

		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		if (Input.GetMouseButtonDown(0)) {
			Shoot (mousePos);
			isAttacking = true;
			animator.SetTrigger("isAttacking");
		} else if (Input.GetMouseButtonDown(1)) {

			if (Timer > SpecialAttackTimer) {					// If enough time has passed
				SpecialAttack1.Special (transform.position);
				isAttacking = true;
				animator.SetTrigger ("isAttacking");
				Timer = 0.0f;
			}
		}
		else {
			isAttacking = false;
		}

		// Call interact function if E is pressed
		if (Input.GetKeyDown(KeyCode.E)) {
			interact();
			GameManager.instance.SendMessage("nextLevel");
		}
	}

	private void movePlayer() {
		float horz = Input.GetAxis ("Horizontal");
		float vert = Input.GetAxis ("Vertical");

		float velocityMagnitudeH = moveSpeed * horz;
		float velocityMagnitudeV = moveSpeed * vert;
		Vector2 newVelocity = new Vector2 (velocityMagnitudeH, velocityMagnitudeV);

		PlayerRigidBody.velocity = newVelocity;

		if (PlayerRigidBody.velocity != Vector2.zero) {
			isMoving = true;
			animator.SetBool("isMoving", true);
		}
		else {
			isMoving = false;
			animator.SetBool("isMoving", false);
		}
	}

    private void setLookDirection() {
    	Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    	transform.rotation = Quaternion.Euler(0, (mousePos.x - transform.position.x) < 0 ? 180 : 0,0);
    }

    private void OnCollisionStay2D(Collision2D collision) {
        Collider2D other = collision.collider;

        if (other.CompareTag ("Enemy")) {
            wasHit = true;
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
            loseHealth(enemy.attackDamage);
        }

        if (other.CompareTag("Exit"))
           onExit = true;

        if (this.healthPoints <= 0) {
            Debug.Log ("Player died :(");
            GameManager.instance.SendMessage("playerDied");
        }
    }

	private void OnTriggerExit2D(Collider2D other) {
		if (other.CompareTag("Exit")) {
			onExit = false;
		}
	}

	private void interact() {
		if (onExit) {
			GameManager.instance.SendMessage("nextLevel");
		}
	}
}
