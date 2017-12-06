using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * People that touched this script:
 * Will
 * Matthew Stewart
**/

//written by Will Belden Brown
public class InView : MonoBehaviour {
    //mode[0] is sprite when in view, mode[1] is when sprite has been seen but not in view
    public Sprite[] modes;

	private bool seen = false;
    private SpriteRenderer sr;
	private GameObject player;
	private float range;

	public void Start() {
		player = GameManager.instance.player;
		range = player.GetComponent<PlayerController>().visionRange;
	}

    void inView() {
		float distanceX = player.transform.position.x - transform.position.x;
		float distanceY = player.transform.position.y - transform.position.y;
        sr = GetComponent<SpriteRenderer>();

		if (Mathf.Abs (distanceX) <= range && Mathf.Abs (distanceY) <= range && !wallInWay (distanceX, distanceY)) {
			seen = true;
			sr.enabled = true;
			if (modes.Length < 1) {
				Debug.Log (transform.name);
			}
			sr.sprite = modes [0];
		} else if (seen == true && modes.Length > 1) {
			sr.enabled = true;
			sr.sprite = modes [1];
		} else {
			sr.enabled = false;
		}
    }

    bool wallInWay(float x, float y) {
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