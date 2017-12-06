using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * People that touched this script:
 *    Lauren Kirk
 *
 *  This script shows sample behaviour of a sprite pacing back and forth. This script
 *  assumes the initial animation of the sprite is facing right.
 *
 *  Future implementation considerations:
 *     (1) Wandering sprite following a path more detailed than +/- x-direction
 *     (2) Move sprite direction handling to the enemy controller (M3 goal)
 */
public class PacingBehaviourEnemy : EnemyController {

    public int maxWanderLeft = -2; //Note: this value should always be less than 0
    public int maxWanderRight = 2; //Note: this value should always be greater than 0
    public int speed = 1;

    private bool goingRight;
    private bool facingRight;

    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
     *                                                                   *
     *                          UNITY API                                *
     *                                                                   *
     * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

    public new void Start() {
        base.Start();

        // Initialize Little Shadow Monster Behaviour Variables
        goingRight = !generateRandomBoolean();
        facingRight = goingRight;

        // Flip right-facing sprite, left
        if (!goingRight && !facingRight)
            transform.localRotation = Quaternion.Euler(0, 180, 0);
    }

    public new void Awake() {
        base.Awake();
    }

    public new void Update() {
        base.Update();
    }

    public override void WalkingStateHandler() {
        walkForward();
    }

    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
     *                                                                   *
     *                   PRIVATE TO LITTLE SHADOW MONSTER                *
     *                                                                   *
     * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

    private bool generateRandomBoolean() {
        // Note: Random.value returns a number within the range 0.0 (inclusive) to 1.0 (non-inclusive).
        //       Value is multiplied by 100 to generate a range from 0 (inclusive) to 100 (non-inclusive)
        //       in this way, one can use probabiltyTrue to generate a boolean with a probability of being
        //       true the assigned percentage of time (give or take a percentage point).

        int randomInt = (int)(Random.value * 100);
        int probabilityTrue = 50;

        return randomInt < probabilityTrue;
    }

    // This might be repetitive
    private void verticallyFlipSprite() {
        if (facingRight && !goingRight) {
            // FlipAnimation to left
            transform.localRotation = Quaternion.Euler(0, 180, 0);

            // Set facingRight as false, because now facing left
            facingRight = false;
        }
        else if (!facingRight && goingRight) {
            // FlipAnimation to right
            transform.localRotation = Quaternion.Euler(0, 0, 0);

            // Set facingRight as true, because now facing right
            facingRight = true;
        }
    }

    private void walkForward() {
        verticallyFlipSprite();

        // Move Sprite
        if (goingRight) {
            transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            //Debug.Log("goingRight: " + transform.position.x);
        }
        else { //if (!goingRight)
            transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            //Debug.Log("goingLeft: " + transform.position.x);
        }

        // Modify direction of Sprite if exceeding walking range
        if (maxWanderLeft > transform.position.x) {
            goingRight = true;
        }
        else if (maxWanderRight < transform.position.x) {
            goingRight = false;
        }
    }
}
