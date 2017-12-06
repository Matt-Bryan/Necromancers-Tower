using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * People that touched this script:
 *    Matt Bryan
 *    Lauren Kirk
 *
 *   This class represents any "actor" character, which currently only refers to 
 *   either the Player or Enemy class. Those classes will inherit this one and expand
 *   upon it.

 *   As seen below, an actor has 5 states, and switches between them based on
 *   the inherited class's updateObjectState() function. A main portion of this class
 *   is dealing with animations, so I would try to make sure you have an Animator
 *   attached to the player object that is modeled somewhat similar to the LittleMonster
 *   Animator (the one whose transitions make it look like a pentagram).
 */

public class ActorController : MonoBehaviour {

    public float healthPoints;
    public float moveSpeed;
    public int xpValue;
    public float attackDamage;
    protected int level;
    protected Animator animator;


    // used when instantiating a new projectile for basic attack
    public int attackSpeed;
    public GameObject attackPrefab;
    public float attackRange;

    // Animation State Triggers
    public ObjectState currentState; // PUBLIC SOLELY FOR DEV PURPOSES, TO TEST WALKING STATE
    public bool wasHit = false;  // PUBLIC SOLELY FOR DEV PURPOSES, TO TEST WALKING STATE
    public bool isAttacking = false;  // PUBLIC SOLELY FOR DEV PURPOSES, TO TEST WALKING STATE
    public bool isMoving = false;  // PUBLIC SOLELY FOR DEV PURPOSES, TO TEST WALKING STATE

    public enum ObjectState {
        IDLE,
        WALKING,
        ATTACK,
        DAMAGED,
        DIE,
    };


    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
     *                                                                   *
     *                          UNITY API                                *
     *                                                                   *
     * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
 
    // Use this for initialization
    public void Awake () {
        Debug.Log("Actor Awake");

        currentState = ObjectState.IDLE;

        animator = GetComponent<Animator>();
        if (animator == null) {
            Debug.Log("No Animator attached!");
        }
    }
    
    // Update is called once per frame
    public void Update () {
        currentState = updateObjectState();

		resetTriggers (animator);
        if (currentState == ObjectState.IDLE) {
            IdleStateHandler();
        } else if (currentState == ObjectState.WALKING) {
            WalkingStateHandler();
        } else if (currentState == ObjectState.ATTACK) {
            AttackStateHandler();
        } else if (currentState == ObjectState.DAMAGED) {
            DamagedStateHandler();
        } else if (currentState == ObjectState.DIE) {
            DieStateHandler();
        }
    }


    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    *                                                                    *
    *                         INHERTIABLE METHODS                        *
    *                                                                    *
    * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *  */

    public virtual void IdleStateHandler() {
        animateIdle();
    }

    public virtual void WalkingStateHandler() {
        animateWalking();
    }

    public virtual void AttackStateHandler() {
        animateAttack();
    }

    public virtual void DamagedStateHandler() {
        animateDamaged();

        Debug.Log("Health now at: " + healthPoints);
    }

    public virtual void DieStateHandler() {
        animateDie();

        //if (animator.GetCurrentAnimatorStateInfo(0).IsName("")) {
        //
        //}
        // TODO:
        //  * Remove sprite?
        //  * Or maybe have it fade away and then set inactive?
        //  * Ooooh. Death noise!
    }

    // DEPRECATED: See Debug Log
    public virtual void changeHealth(float amount) {
        Debug.Log("THIS FUNCTION IS DEPRECATED. Please use gainHealth instead.");
        gainHealth(amount);
    }

    public virtual void gainHealth(float amount) {
        healthPoints += amount;
    }

    public virtual void loseHealth(float amount) {
        healthPoints -= amount;
        wasHit = true;
    }

    public virtual void BasicAttack() {
        // Instantiate new projectile prefab with projectile parameters
    }

    // Manual set activity between idle and walking states via script
    public void updateIsMoving(bool isItMoving) {
        isMoving = isItMoving;
    }

    // Manual set activity between idle and walking states via script
    public void updateIsAttacking(bool isItAttacking) {
        isAttacking = isItAttacking;
    }

    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
     *                                                                   *
     *                   PRIVATE TO ACTOR                                *
     *                                                                   *
     * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

    private ObjectState updateObjectState()
    {
        if (healthPoints <= 0)
        {
            return ObjectState.DIE;
        }

        if (isMoving && !isAttacking && !wasHit)
        {
            return ObjectState.WALKING;
        }

        if (isAttacking)
        {
            return ObjectState.ATTACK;
        }

        if (wasHit)
        {
            wasHit = false;
            return ObjectState.DAMAGED;
        }

        return ObjectState.IDLE;
    }

	private void resetTriggers(Animator animator) {
		animator.ResetTrigger("Idle");
		animator.ResetTrigger("Walking");
		animator.ResetTrigger("Attack");
		animator.ResetTrigger("Damaged");
		animator.ResetTrigger("Die");
	}

    private void animateIdle() {
		animator.SetTrigger("Idle");
    }

    private void animateWalking() {
        animator.SetTrigger("Walking");
    }

    private void animateAttack() {
        animator.SetTrigger("Attack");
    }

    private void animateDamaged() {
        animator.SetTrigger("Damaged");
    }

    private void animateDie() {
        animator.SetTrigger("Die");
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length + 0.25f);
    }
}
