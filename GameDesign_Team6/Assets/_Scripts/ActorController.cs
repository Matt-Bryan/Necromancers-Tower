using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * People that touched this script:
 *    Matt Bryan
 *    Lauren Kirk
 *    Matt Stewart
 * 	  Allison Lee
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
	
    // Dev Mode, Set as True in Unity Console
    public bool isDevModeOn = false;

    public float healthPoints;
	[HideInInspector] //Hides maxHealthPoints from the inspector.
	public float maxHealthPoints;
	//The maximum number of health points. Automatically set to the value of healthPoints on start.
    public float moveSpeed;
    public int xpValue;
    public float attackDamage;
	public int level;
    protected AudioSource footstepSource;
    protected AudioSource damagedSource;
    protected Animator animator;

    // Used when Instantiating a New Projectile for Basic Attack
    public int attackSpeed;
    public GameObject attackPrefab;
    public float attackRange;

    // Animation State Triggers
    protected ObjectState currentState;
    protected bool wasHit = false;
    protected bool isAttacking = false;
    protected bool isMoving = false;

    public enum ObjectState {
        IDLE,
        WALKING,
        ATTACK,
        DAMAGED,
        DIE,
    };

    private Dictionary<ObjectState, string> animationStates = new Dictionary<ObjectState, string> {
        { ObjectState.IDLE,    "Idle"},
        { ObjectState.WALKING, "Walking"},
        { ObjectState.ATTACK,  "Attack"},
        { ObjectState.DAMAGED, "Damaged"},
        { ObjectState.DIE,     "Die"}
    };

    private Dictionary<ObjectState, Action> animationStateHandlers;

    // For Sprite Flickering in Damaged State
    protected bool isFlickering = false; // use this to restrict movement when enemy is being damaged (can't walk and be hurt at the same time)?
    private int timesFlickering;
    private float alphaLevel;
    private bool isAlphaDecrease = false;
    private const int NUM_FLICKERS = 3;
    private float totalTime;


    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
     *                                                                   *
     *                          UNITY API                                *
     *                                                                   *
     * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
 
    // Use this for initialization
    public void Awake () {
		currentState = ObjectState.IDLE;

		setAnimStateHandlers ();

        animator = GetComponent<Animator>();
        if (animator == null) {
            Debug.Log("No Animator attached!");
        }

        AudioSource[] temp = GetComponents<AudioSource>();
        if (temp.Length != 0) {
            footstepSource = temp[0];
            damagedSource = temp[1];
        }

		this.maxHealthPoints = this.healthPoints;
		//We set max health to whatever we set our health points to as a starting point.
    }
    
    // Update is called once per frame
    public void Update () {
        currentState = updateObjectState();

        launchDevMode();

        resetTriggers(animator);
        
		if (isFlickering)
            flickerSprite();
    }

    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    *                                                                    *
    *                         INHERTIABLE METHODS                        *
    *                                                                    *
    * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *  */

    public virtual void IdleStateHandler() {
    
    }

    public virtual void WalkingStateHandler() {
    
    }

    public virtual void AttackStateHandler() {
    
    }

    public virtual void DamagedStateHandler() {
        launchFlickering ();
    }

    public virtual void DieStateHandler() {
    
        // TODO:
        //  * Remove sprite?
        //  * Or maybe have it fade away and then set inactive?
        //  * Ooooh. Death noise!

        Destroy (gameObject, animator.GetCurrentAnimatorStateInfo (0).length + 0.25f);
    }

    public virtual void gainHealth(float amount) {
        healthPoints += amount;
    }

    public virtual void loseHealth(float amount) {
		healthPoints = Mathf.Max(0, healthPoints - amount);
        wasHit = true;
        if (damagedSource != null) {
            damagedSource.Play();
        }
    }

	public virtual float calculateHealthPercentage()
	{
		return this.healthPoints / this.maxHealthPoints;
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

	//TODO: Make inheritable? But don't ever use it.
	public virtual void resetTriggers(Animator animator) {
		if (animator == null)
			return;
		if (animationStateHandlers == null)
			setAnimStateHandlers ();

		if (tag != "Player") {
			foreach (var state in animationStates) {
				if (currentState.Equals (state.Key)) {
					animator.SetTrigger (state.Value);
					animationStateHandlers [state.Key] ();
				}
			}

			foreach (var state in animationStates) {
				if (!currentState.Equals (state.Key)) {
					animator.ResetTrigger (state.Value);
				}
			}
		}
	}

    private void launchFlickering() {
        totalTime = 0;
        alphaLevel = 1f;
        isFlickering = true;
        isAlphaDecrease = true;
        timesFlickering = NUM_FLICKERS;
    }

    private void flickerSprite()
    {
        const float alphaLevelChangeSpeed = .25f;

        totalTime += Time.deltaTime;

        if (totalTime >= 0.015f)
        {
            if (isAlphaDecrease == true)
            {
                alphaLevel -= alphaLevelChangeSpeed;
            }
            else
            {
                alphaLevel += alphaLevelChangeSpeed;
            }

            // 100% Transparent - Fade Back Up
            if (alphaLevel < 0f)
            {
                isAlphaDecrease = false;
            }

            // 100% Opague - Fade Back Down
            if (alphaLevel > 1f)
            {
                isAlphaDecrease = true;
                timesFlickering--;
            }

            totalTime = 0;
        }

        //Tint Dark(?) Red
        GetComponent<SpriteRenderer>().color = new Color(.5f + .5f * alphaLevel, alphaLevel, alphaLevel, alphaLevel);

        //Tint back to normal
        if (timesFlickering == 0)
        {
            //wasHit = false;
            isFlickering = false;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alphaLevel);
        }
    }

	private void setAnimStateHandlers () {
		animationStateHandlers = new Dictionary<ObjectState, Action> {
			{ObjectState.IDLE, IdleStateHandler},
			{ObjectState.WALKING, WalkingStateHandler},
			{ObjectState.ATTACK, AttackStateHandler},
			{ObjectState.DAMAGED, DamagedStateHandler},
			{ObjectState.DIE, DieStateHandler}
		};
	}

    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
     *                                                                   *
     *                   DEV MOVE CONTROLS                                *
     *                                                                   *
     * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

    public void launchDevMode() {
        if (isDevModeOn == false)
            return;
        else {
            // Check if space bar is pressed and set to Damaged State
            if (Input.GetKeyDown("space")) {
                Debug.Log("Spacebar is pressed");
                currentState = ObjectState.DAMAGED;
            }
        }
    }
}