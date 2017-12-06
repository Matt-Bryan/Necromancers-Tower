using UnityEngine;

/*
 * People that touched this script:
 *    Lauren Kirk
 *
 *  This script shows sample behaviour of a sprite chasing an enemy object and transitioning 
 *  between various animation states. Main state of attack is meelee / physical contact
 *
 *  Future implementation:
 *     (1) Fix the object's walking animation vs. actual speed (looks like its floating)
 */
public class LittleMonsterController : EnemyController {

    public float chaseRange = 2f;

    private bool facingRight = true;

    // Use this for initialization
    public new void Start() {
        base.Start();

        // Attacks when on top of character
        this.attackRange = player.GetComponent<Collider2D>().bounds.size.x * 1.2f;
    }

    // Update is called once per frame
    public new void Update() {
        if (isPlayerInChaseRange()) {
            updateIsMoving(true);
        }
        else {
            updateIsMoving(false);
        }

        if (isPlayerInAttackRange()) {
            updateIsAttacking(true);
        } else {
            updateIsAttacking(false);
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
        base.WalkingStateHandler();

        if (this.transform.position.x < player.transform.position.x) {
            // player is to the left of this.GameObject
            faceRight();
        }
        else if (this.transform.position.x > player.transform.position.x) {
            // player is to the right of this.GameObject
            faceLeft();
        }
        else {
            // player ontop of this.GameObject X
			faceRight();
        }

        chasePlayer();
    }

    public override void AttackStateHandler() {
        base.AttackStateHandler();

        // Fixes the bug where the game object gets stuck in "attack mode" but 
        // never actually touches the player and thus never causes damage.
        chasePlayer();
    }

    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
     *                                                                   *
     *                   PRIVATE TO CHASE AND HIT                        *
     *                                                                   *
     * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

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

    private bool isPlayerInChaseRange() {
        float distance = Vector2.Distance(player.transform.position, this.transform.position);

        return (distance - chaseRange) < 0;
    }

    private bool isPlayerInAttackRange() {
        float distance = Vector2.Distance(player.transform.position, this.transform.position);

        return (distance - attackRange) < 0;
    }

    private void chasePlayer() {
        // Monster is "damaged", therefore it shouldn't be moving.
        if (this.isFlickering)
        {
            return;
        }

        float transformX = 0f;
        float transformY = 0f;

        // Left or Right
        if (facingRight) {
            transformX = transform.position.x + this.moveSpeed * Time.deltaTime;
        }
        else {
            transformX = transform.position.x - this.moveSpeed * Time.deltaTime;
        }

        // Up or Down
        if (this.transform.position.y < player.transform.position.y) {
            // player above this.GameObject
            transformY = transform.position.y + this.moveSpeed * Time.deltaTime;
        }
        else if (this.transform.position.y > player.transform.position.y) {
            // player below this.GameObject
            transformY = transform.position.y - this.moveSpeed * Time.deltaTime;
        }
        else {
            // player ontop of this.GameObject Y
        }

        transform.position = new Vector3(transformX, transformY, transform.position.z);
    }
}
