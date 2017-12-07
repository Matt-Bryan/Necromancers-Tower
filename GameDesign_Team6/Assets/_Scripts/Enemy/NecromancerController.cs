using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* People who touched this script:
 * Allison Lee
   Matt Bryan (i stole lots of matt's code for his skeleton because i'm a terrible person)
   Lauren Kirk (Many functions copied from FlyingMonster)
*/

public class NecromancerController : EnemyController {

	public float chaseRange = 2f;
	private bool colliding = false;
    private bool facingRight = true;
	private float specialTimer = 0.0f;

	// Use this for initialization
	public new void Start () {
		base.Start();

        this.attackRange = player.GetComponent<Collider2D>().bounds.size.x + 0.68f;
	}
	
	// Update is called once per frame
	public new void Update () {
		if (isPlayerInChaseRange()) {
            updateIsMoving(true);
        }
        else {
            updateIsMoving(false);
        }

        if (isPlayerInAttackRange()) {
            updateIsAttacking(true);
			PlayerController playerObject = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>();
			playerObject.loseHealth (attackDamage);
        } else {
            updateIsAttacking(false);
        }

		if(isPlayerInSpecialRange())
		{
			colliding = true;
		}
		else{
			colliding = false;
		}

		if (colliding) {
			specialTimer += Time.deltaTime;
			if (specialTimer >= .3) {
				fireSpecial ();
			}
		} else {
			specialTimer = 0.0f;
		}

        if (this.healthPoints <= 0) {
            GameManager.instance.SendMessage("playerWon");
        }
        // This needs to occur after updateIsMoving
        base.Update();
	}

	private void fireSpecial()
	{
		//If the necromancer has been touching the player for longer than .6 seconds, fire this move
		Animator animator = GetComponent<Animator>();
		animator.SetTrigger ("SpecialAttack");
		PlayerController playerObject = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>();
		playerObject.loseHealth (10);
	}

	public override void WalkingStateHandler() {
        base.WalkingStateHandler();

        if (Mathf.Abs(this.transform.position.x - player.transform.position.x) < 0.1) {
            faceRight();
        }
        else if (transform.position.x > player.transform.position.x) {
            // player is to the right of this.GameObject
            faceLeft();
        }
        else {
        	faceRight();
        }

        chasePlayer();
    }

    private bool isPlayerInChaseRange() {
        float distance = Vector2.Distance(player.transform.position, this.transform.position);

        return (distance - chaseRange) < 0;
    }

    private void faceRight() {
        if (!facingRight) {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            facingRight = true;
        }
    }

    private void faceLeft() {
        if (facingRight) {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            facingRight = false;
        }
    }

    private bool isPlayerInAttackRange() {
        float distance = Vector2.Distance(player.transform.position, this.transform.position);

        return (distance - attackRange) < 1.2;
    }
	private bool isPlayerInSpecialRange() {
		float distance = Vector2.Distance(player.transform.position, this.transform.position);

		return (distance - attackRange) < .5;
	}

     private void chasePlayer() {

        float transformX = 0f;
        float transformY = 0f;

        // Left or Right
        if (Mathf.Abs(this.transform.position.x - player.transform.position.x) < 0.1) {
        	transformX = transform.position.x;
        }
        else if (transform.position.x < player.transform.position.x) {
            transformX = transform.position.x + this.moveSpeed * Time.deltaTime;
        }
        else {
            transformX = transform.position.x - this.moveSpeed * Time.deltaTime;
        }

        // Up or Down
        if (Mathf.Abs(this.transform.position.y - player.transform.position.y) < 0.1) {
        	transformY = transform.position.y;
        }
        else if (this.transform.position.y < player.transform.position.y) {
            // player above this.GameObject
            transformY = transform.position.y + this.moveSpeed * Time.deltaTime;
        }
        else {
            // player below this.GameObject
            transformY = transform.position.y - this.moveSpeed * Time.deltaTime;
        }

        transform.position = new Vector3(transformX, transformY, transform.position.z);
    }
}
