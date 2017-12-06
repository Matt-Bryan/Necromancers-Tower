using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * People that touched this script:
 *    Matt Bryan
 *    Lauren Kirk
 *
 *  This script extends ActorController and represents any enemy we want it to.
 *
 *  CURRENT ISSUES: Just like the player, the enemy's dying animation continues to play over and over, and
 *                  the enemy doesn't destroy itself yet. Can be easily fixed later.
 *
 *                  I didn't put in any of the previous enemyController's advanced player-following stuff.
 *                  It just straight up follows the player no matter how far.
 *
 *                  Following up on the previous point, the enemy continues to move towards the player
 *                  until it's literally right on top of the player, where it will spaz back and forth
 *                  until the player moves.
 *
 *                  As mentioned in the newPlayerScript, we need to look at how damage is applied to actors in our game 
 *                  and make changes accordingly.
  *
  *  Future implementation considerations:
 *     (1) Lots of enemy behaviour scripts handle the direction-facing behaviour on their own. It might be worth-while
 *         to pull that out and put it here. (M3 Goal)
 *     (2) Remove redundancy of isWalking and isMoving. (M2 Goal)
 *     (3) Figure out isMoving vs transition between idle and walking states. (M2 Goal)
 *     (4) Playern not consistently inherited in subclasses and not sure why. Right now
 *         just redoing the findObjectByTag call in start every time.
  */

public class EnemyController : ActorController {

    private GameObject player;
    private Rigidbody2D enemyRb;

    public bool isWalking;     // FIC: this might be redundant to ActorController's isMoving
    public float walkTime = 1;
    public float waitTime = 1;
    public Collider2D walkZone;

    private Vector2 minWalkPoint;
    private Vector2 maxWalkPoint;
    private float walkCounter;
    private float waitCounter;
    private int WalkDirection; //UP = 0, RIGHT = 1, DOWN = 2, LEFT = 3
    private bool hasWalkZone; // by default, is false.


    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
     *                                                                   *
     *                          UNITY API                                *
     *                                                                   *
     * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

    // Use this for initialization
    public void Start () {
        Debug.Log("Enemy Start");

        player = GameObject.FindGameObjectWithTag("Player");
        enemyRb = GetComponent<Rigidbody2D>();

        if (enemyRb == null) {
            Debug.Log("ERROR: No enemyRb found. Is necessary for idle behaviour");
        }

        // Init Idle Behaviour Variables
        waitCounter = waitTime;
        walkCounter = walkTime;

        chooseDirection();

        if (walkZone != null) {
            minWalkPoint = walkZone.bounds.min;
            maxWalkPoint = walkZone.bounds.max;
            hasWalkZone = true;
        }
    }

    // base.Update() must be called somewhere in the function (best to just leave it in as the first line)
    public new void Update () {
        base.Update();

        /*
        if (currentState != ObjectState.DIE) {
            moveEnemy(player.transform.position);
            setLookDirection();
        }
        else {
            enemyRb.velocity = Vector2.zero;
        }
        */
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Projectile")) {
			
			float damage = other.gameObject.GetComponent<ProjectileDie>().attackDamage;

            wasHit = true;
            this.loseHealth(damage);

            Destroy(other.gameObject);

            Debug.Log("ENEMY HIT. Damaged. Current Health: " + this.healthPoints);
        }
    }

    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
     *                                                                    *
     *                         INHERTIABLE METHODS                        *
     *                                                                    *
     * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *  */

    public override void IdleStateHandler() { 
        if (isWalking) {
            base.WalkingStateHandler();
            walkCounter -= Time.deltaTime;

            switch (WalkDirection)
            {
                case 0:
                    enemyRb.velocity = new Vector2(0, moveSpeed);

                    if (hasWalkZone && transform.position.y > maxWalkPoint.y) {
                        isWalking = false;
                        waitCounter = waitTime;
                    }
                    break;

                case 1:
                    enemyRb.velocity = new Vector2(moveSpeed, 0);

                    if (hasWalkZone && transform.position.x > maxWalkPoint.x) {
                        isWalking = false;
                        waitCounter = waitTime;
                    }
                    break;

                case 2:
                    enemyRb.velocity = new Vector2(0, -moveSpeed);

                    if (hasWalkZone && transform.position.y < minWalkPoint.y) {
                        isWalking = false;
                        waitCounter = waitTime;
                    }
                    break;

                case 3:
                    enemyRb.velocity = new Vector2(-moveSpeed, 0);

                    if (hasWalkZone && transform.position.x < minWalkPoint.x) {
                        isWalking = false;
                        waitCounter = waitTime;
                    }
                    break;

                default:
                    Debug.Log("Lab4_IdleMotion: This statement should never happen.");
                    break;
            }

            if (walkCounter < 0) {
                isWalking = false;
                waitCounter = waitTime;
            }

        }
        else {
            base.IdleStateHandler();
            waitCounter -= Time.deltaTime;

            enemyRb.velocity = Vector2.zero;

            if (waitCounter < 0) {
                chooseDirection();
            }
        }
    }

    public override void WalkingStateHandler() {
        base.WalkingStateHandler();
    }

    public override void AttackStateHandler() {
        base.AttackStateHandler();
    }

    public override void DamagedStateHandler() {
        base.DamagedStateHandler();
    }

    public override void DieStateHandler() {
        base.DieStateHandler();
    }

    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
     *                                                                   *
     *                   PRIVATE TO ENEMY                                *
     *                                                                   *
     * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            isAttacking = false;
        }
    }

    private void setLookDirection() {
        transform.rotation = Quaternion.Euler(0, (player.transform.position.x - transform.position.x) < 0 ? 180 : 0,0);
    }

    private void moveEnemy (Vector3 target) {
        Vector2 velocityVector = target - transform.position;
        enemyRb.velocity = velocityVector.normalized * moveSpeed;
    }


    private void chooseDirection() {
        // Used by Idle Behaviour

        WalkDirection = Random.Range(0, 4);
        isWalking = true;
        walkCounter = walkTime;
    }
}
