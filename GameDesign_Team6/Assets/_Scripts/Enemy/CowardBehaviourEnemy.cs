using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * People that touched this script:
 *    Lauren Kirk
 *
 *  This script shows sample behaviour of a sprite running away when a playerObject
 *  comes too close.
 *
 *  Future implementation:
 *     (1) add some sort of idle animation or activity... see TODO below
 *     (2) Move sprite direction handling to the enemy controller (M3 goal)
 *     (3) Figure out isMoving vs transition between idle and walking states (M2 Goal)
 *     (4) Coward behaviour only executes when "isMoving" need a way to change the states
 */

public class CowardBehaviourEnemy : EnemyController {

    public float playerTooCloseRange = 1.5f;
    public float playerFarEnoughRange = 3.0f;
    public int speed = 2;

    private GameObject playerObject;

    private bool isScared;
    private bool goingRight;
    private bool facingRight;
    private bool resetCalm;


    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
     *                                                                   *
     *                          UNITY API                                *
     *                                                                   *
     * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

    // Use this for initialization
    public new void Start() {
        base.Start();

        // Redundant line of code, but ancestory not correctly inheriting player?
        playerObject = GameObject.FindGameObjectWithTag("Player");
        //Debug.Log("PLAYER OBJECT: " + playerObject);

        isScared = false;
        resetCalm = false;

        goingRight = !generateRandomBoolean();
        facingRight = goingRight;

        // Flip right-facing sprite, left
        if (!goingRight && !facingRight)
            transform.localRotation = Quaternion.Euler(0, 180, 0);
    }

    // Update is called once per frame
    public new void Update() {
        if (isPlayerTooClose()) {
            updateIsMoving(true);
        }

        // This needs to occur after updateIsMoving
        base.Update();
    }


    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    *                                                                    *
    *                         INHERTIABLE METHODS                        *
    *                                                                    *
    * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *  */

    public override void WalkingStateHandler() {
        if (isPlayerTooClose()) {
            isScared = true;
            resetCalm = true;
        }

        if (!isPlayerFarEnough()) {
            isScared = false;
        }

        if (isScared) {
            //Debug.Log("RUN AWAY, SCARED");
            lookAwayFromPlayer();
            fleePlayerObject();
        } else {
            //Debug.Log("Not Scared");

            if (resetCalm) {
                resetCalm = false;
            }

            // How to go back to idle state?
            // wanderAroundCalmly();
            updateIsMoving(false);
        }
    }


    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
     *                                                                   *
     *                   PRIVATE TO COWARD BEHAVIOR                      *
     *                                                                   *
     * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

    private bool isPlayerTooClose() {
        float distance = Vector2.Distance(
            playerObject.transform.position,
            this.transform.position);

        return (distance - playerTooCloseRange) < 0;
    }

    private bool isPlayerFarEnough() {
        float distance = Vector2.Distance(
            playerObject.transform.position,
            this.transform.position);

        return (distance - playerFarEnoughRange) < 0;
    }

    private void fleePlayerObject() {
        float transformX = 0f;
        float transformY = 0f;

        if (facingRight) {
            transformX = transform.position.x + speed * Time.deltaTime;
        }
        else {
            transformX = transform.position.x - speed * Time.deltaTime;
        }

        // Up or Down
        if (this.transform.position.y <= playerObject.transform.position.y) {
            // playerObject above this.GameObject, go down
            transformY = transform.position.y - speed * Time.deltaTime; ;
        }
        else if (this.transform.position.y > playerObject.transform.position.y) {
            // playerObject below this.GameObject, go up
            transformY = transform.position.y + speed * Time.deltaTime; ;
        }

        transform.position = new Vector3(transformX, transformY, transform.position.z);
    }

    private void lookAwayFromPlayer() {
        if (this.transform.position.x <= playerObject.transform.position.x) {
            // playerObject is to the left of (or on top of) this.GameObject
            faceLeft();
        }
        else if (this.transform.position.x > playerObject.transform.position.x) {
            // playerObject is to the right of this.GameObject
            faceRight();
        }
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

    private bool generateRandomBoolean() {
        // Note: Random.value returns a number within the range 0.0 (inclusive) to 1.0 (non-inclusive).
        //       Value is multiplied by 100 to generate a range from 0 (inclusive) to 100 (non-inclusive)
        //       in this way, one can use probabiltyTrue to generate a boolean with a probability of being
        //       true the assigned percentage of time (give or take a percentage point).

        int randomInt = (int) (Random.value * 100);
        int probabilityTrue = 50;

        return randomInt < probabilityTrue;
    }
}
