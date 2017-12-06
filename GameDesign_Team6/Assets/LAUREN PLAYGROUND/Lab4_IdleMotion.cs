using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * THROW THIS AWAY
 */
public class Lab4_IdleMotion : MonoBehaviour {

    public float moveSpeed;
    private Vector2 minWalkPoint;
    private Vector2 maxWalkPoint;

    private Rigidbody2D myRigidBody;

    public bool isWalking;

    public float walkTime;
    private float walkCounter;
    public float waitTime;
    private float waitCounter;

    private int WalkDirection; //UP = 0, RIGHT = 1, DOWN = 2, LEFT = 3

    public Collider2D walkZone;
    private bool hasWalkZone; // by default, is false.

    public bool canMove;
    private DialogueManager theDM;

    // Use this for initialization
    void Start () {
        myRigidBody = GetComponent<Rigidbody2D>();
        theDM = FindObjectOfType<DialogueManager>();

        waitCounter = waitTime;
        walkCounter = walkTime;

        chooseDirection();
        
        if (walkZone != null) {
            minWalkPoint = walkZone.bounds.min;
            maxWalkPoint = walkZone.bounds.max;
            hasWalkZone = true;
        }

        canMove = true;
    }

    // Update is called once per frame
    void Update () {

        if (!theDM.dialogActive)
        {
            canMove = true;
        }

        if (!canMove) {
            myRigidBody.velocity = Vector2.zero;
            return;
        }

        if (isWalking) {
            walkCounter -= Time.deltaTime;

            switch(WalkDirection) {
                case 0:
                    myRigidBody.velocity = new Vector2(0, moveSpeed);
                    if (hasWalkZone && transform.position.y > maxWalkPoint.y) {
                        isWalking = false;
                        waitCounter = waitTime;
                    }
                    break;

                case 1:
                    myRigidBody.velocity = new Vector2(moveSpeed, 0);
                    if (hasWalkZone && transform.position.x > maxWalkPoint.x) {
                        isWalking = false;
                        waitCounter = waitTime;
                    }
                    break;

                case 2:
                    myRigidBody.velocity = new Vector2(0, -moveSpeed);
                    if (hasWalkZone && transform.position.y < minWalkPoint.y) {
                        isWalking = false;
                        waitCounter = waitTime;
                    }
                    break;

                case 3:
                    myRigidBody.velocity = new Vector2(-moveSpeed, 0);
                    if (hasWalkZone && transform.position.x < minWalkPoint.x)
                    {
                        isWalking = false;
                        waitCounter = waitTime;
                    }
                    break;

                default:
                    Debug.Log("Lab4_IdleMotion: This statement should never happen.");
                    break;
            }

            if (walkCounter < 0)
            {
                isWalking = false;
                waitCounter = waitTime;
            }

        }
        else {
            waitCounter -= Time.deltaTime;

            myRigidBody.velocity = Vector2.zero;

            if (waitCounter < 0)
            {
                chooseDirection();
            }
        }
    }

    public void chooseDirection() {
        WalkDirection = Random.Range(0, 4);
        isWalking = true;
        walkCounter = walkTime;
    }
}
