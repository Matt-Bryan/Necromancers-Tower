using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OLD_HeartsSpriteController : MonoBehaviour {

    public float floatRange = 1f;
    public float speed = 1f;

    public Sprite[] heartSprites = new Sprite[3];
    private int currentSprite = 0;

    private Vector2 startPos;
    private float maxUp;
    private float maxDown;
    private bool floatingUp;

    // Use this for initialization
    void Start () {
        startPos = transform.position;

        maxUp = startPos.y + floatRange;
        maxDown = startPos.y - floatRange;
    }

    // Update is called once per frame
    void Update () {
        float yPosCurrently = transform.position.y;

        if (yPosCurrently > maxUp) {
            // above maxUp, go down
            floatingUp = false;

            // Note: Remove This. Place to test if updateSprite() works
            updateSprite();
        }
        else if (yPosCurrently < maxDown) {
            // below maxDown, go up
            floatingUp = true;
        }

        if (floatingUp) {
            //Debug.Log("Here...");
            transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime, transform.position.z);
        } else {
            //Debug.Log("Here Too...");
            transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);
        }
    }

    private void updateSprite() {
        Debug.Log("Here As Well...");
        currentSprite = (currentSprite + 1 == heartSprites.Length) ? 0 : currentSprite + 1;
        gameObject.GetComponent<SpriteRenderer>().sprite = heartSprites[currentSprite];
    }
}
