using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * People that touched this script: Matthew Stewart, Matt Bryan, Allison Lee, Will Belden Brown
**/

/**
	This is going to be the new PlayerController script. It should be attached to the player object, and it inherits from the
	Actor Controller script, which does the main handling of changing the object's state and animations. Check Actor Controller
	script for notes on those topics.
**/

public class PlayerController : ActorController {

	public SpecialController SpecialAttack1;

	public float SpecialAttackTimer = 0.5f;	// Cooldown between special attacks
	[HideInInspector]
	public float Timer = 100.0f;  // Used to check time between special attacks
                                  //Timer should begin with all cooldowns reset!

	public float visionRange;

	public GameObject inv;

	private Rigidbody2D PlayerRigidBody;

    private int toNextLevel;
    private int playerGold = 0;

	// Is the player standing on the exit tile? (Used in order to hit E to exit)
	public bool onExit = false; // For debuggin purposes only

	private bool soundPlaying = false;	
    private bool onItem = false;
    private GameObject touchedItem;


	// can also use something like:
	public int getRequiredXP () {
		return (level-1)*100 + level * 100;
	}
		
	// Use this for initialization
	public void Start () {
 		SpecialAttack1.setDamage (attackDamage);
		PlayerRigidBody = GetComponent<Rigidbody2D>();
		inv.SetActive (false);
	}

	// base.Update() must be called somewhere in the function (best to just leave it in as the first line)
	public new void Update() {
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
	
	public void FixedUpdate () {
		if (currentState != ObjectState.DIE) {
			movePlayer();
		}
		else {
			PlayerRigidBody.velocity = Vector2.zero;
		}
	}

	private Quaternion getProjectileRotation () {
			bool rotateProjectileTrue = attackPrefab.GetComponent<ProjectileDie>().rotateProjectile;
			//This is set in the attack prefab. If it's true, we rotate the projectile to face the mouse.
			//Otherwise, we return Quaternion.identity.
		if (rotateProjectileTrue) {
			//This rotates projectiles to the correct direction.
			Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint (transform.position);
			float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
			return Quaternion.AngleAxis (angle, Vector3.forward);
		} else {
			return Quaternion.identity;
		}
	}

		// TODO: Have this call ActorController basic attack method after interpreting mousePos

	private void Shoot(Vector2 mousePos) {
		//Velocity vector from player to mouse position
		Vector2 shotVelocityVector = ((Vector2)mousePos - (Vector2)transform.position);

		Quaternion projectileRotation = getProjectileRotation();

		GameObject shot = Instantiate (attackPrefab, transform.position, projectileRotation);
		Rigidbody2D shotRb = shot.GetComponent<Rigidbody2D>();

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
		}

		if (Input.GetKeyDown(KeyCode.I)) {
			inv.SetActive (!inv.activeSelf);
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

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Exit")) {
            onExit = true;
        }
		// fix this to work with multiple items (maybe move to collision stay?)
        else if (other.CompareTag("Item")) {
            onItem = true;
            touchedItem = other.gameObject;
        }

    }

    private void OnCollisionStay2D(Collision2D collision) {
        Collider2D other = collision.collider;

        if (other.CompareTag ("Enemy")) {
            wasHit = true;
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
            loseHealth(enemy.attackDamage);
        }

        //if (other.CompareTag("Exit"))
        //   onExit = true;

        if (this.healthPoints <= 0) {
            GameManager.instance.SendMessage("playerDied");
        }
    }

	private void OnTriggerExit2D(Collider2D other) {
		if (other.CompareTag("Exit")) {
			onExit = false;
		}

        else if (other.CompareTag("Item")) {
            onItem = false;
            touchedItem = null;
        }
	}

	private void interact() {
        int chanceOfShop = Random.Range(1,100);
		if (onExit) {
			resetStuff ();
            if(chanceOfShop <= 35)
                GameManager.instance.SendMessage("toShop");
            else
			    GameManager.instance.SendMessage("nextLevel");
		}
			
        else if (onItem)
            pickup(touchedItem);
	}

    public void gainExp(int exp) {
        xpValue += exp;

		if (xpValue >= getRequiredXP()) {
            level++;
            maxHealthPoints += 10;
            healthPoints += 10;
			xpValue = 0;
        }
    }

    void pickup(GameObject pickup) {
        Item item = pickup.GetComponent<Item>();
        if (item.type.CompareTo("Healing") == 0)
        {
            if (healthPoints != maxHealthPoints)
            {
                if (healthPoints + item.getHealing() >= maxHealthPoints)
                {
                    Debug.Log("at max health");
                    healthPoints = maxHealthPoints;
                }
                else
                {
                    Debug.Log("PlayerController: picking up health: " + pickup.GetComponent<Item>().getHealing());
                    healthPoints += (float)pickup.GetComponent<Item>().getHealing();
                }
            }
        }

        else if (item.type.CompareTo("Gold") == 0)
        {
            playerGold += item.getGold();
        }
 
        pickup.gameObject.SetActive(false);
    }

    public int getCurrentExp() {
		return xpValue;
    }

    public void setExp(int exp) {
		xpValue = exp;
    }

    public int getPlayerGold() {
        return playerGold;
    }

    public void setPlayerGold(int gold) {
        playerGold = gold;
    }

	public void resetStuff () {
		onExit = false; // For debuggin purposes only
		soundPlaying = false;	
		onItem = false;
		touchedItem = null;
	}

    public void upgradeStats(float upgrade, string option) {
        //moveSpeed
        //attackDamage
        //maxHealthPoints

        switch (option) {
            case "Speed" :
                moveSpeed += upgrade;
                break;

            case "Damage" :
                attackDamage += upgrade;
                break;

            case "Health" :
                maxHealthPoints += upgrade;
                break;

            case "Heal" :
                if (healthPoints + upgrade > maxHealthPoints) {
                    healthPoints = maxHealthPoints;
                    break;
                }
                healthPoints += upgrade;
                break;

            default :
                break;

        }
    }
}

[System.Serializable]
public class InventorySlots {

}

public class InventorySlot {

}