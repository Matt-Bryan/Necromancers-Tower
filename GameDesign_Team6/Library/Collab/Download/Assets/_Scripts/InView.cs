using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * People that touched this script: thank you will sorry
**/

//written by Will Belden Brown
public class InView : MonoBehaviour {
    //mode[0] is sprite when in view, mode[1] is when sprite has been seen but not in view
    public Sprite[] modes;

	private bool seen = false;
    private SpriteRenderer sr;
    private GameObject player;

    private void Start() {
        player = GameObject.FindWithTag("Player");
    }

    void inView() {
        float distanceX = player.transform.position.x - transform.position.x;
        float distanceY = player.transform.position.y - transform.position.y;
        sr = GetComponent<SpriteRenderer>();

        if (Mathf.Abs(distanceX) <= 3 && Mathf.Abs(distanceY) <= 3 && wallInWay(distanceX, distanceY) == false) {
            seen = true;
            sr.enabled = true;
            sr.sprite = modes[0];
        }

        else if (seen == true && modes.Length > 1) {
            sr.enabled = true;
            sr.sprite = modes[1];
        }

        else
            sr.enabled = false;
    }

    bool wallInWay(float x, float y) {
        // Value never used. Commenting out to remove warning in logs.
        // Vector2 dir = new Vector2(transform.position.x -player.transform.position.x, transform.position.y - player.transform.position.y); 

        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(x, y), 3.0f);
        if (hit && transform.position != hit.transform.position) {
            if (hit.transform.tag == "Wall")
                return true;
        }

        return false;
    }
	
	// Update is called once per frame
	void Update () {
		inView();
	}
}