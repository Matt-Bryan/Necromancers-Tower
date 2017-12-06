using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCircle : MonoBehaviour {

	public float circleAlphaChange = 0.05f;

	//Origin of alpha on circle
	public float circleAlphaStart = 0.1f;

	private SpriteRenderer circleRend;
	private bool circleFlashing = false;
	private bool circleAlphaIncreasing;
	private Color circleColor = new Color(1f, 1f, 1f, 0.2f);

	// Use this for initialization
	void Start () {
		circleRend = GetComponent<SpriteRenderer>();
		circleRend.color = circleColor;
	}
	
	// Update is called once per frame
	void Update () {
		if (circleFlashing) {
			flashCircle();
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Projectile")) {
			Destroy(other.gameObject);

            initializeFlashCircle();
		}
	}

	private void flashCircle() {
		if (circleRend.color.a < 0.99f && circleRend.color.a > circleAlphaStart) {
			//Alpha less than 1, check if increasing or decreasing
			if (circleAlphaIncreasing) {
				circleRend.color += new Color(0, 0, 0, circleAlphaChange);
			}
			else {
				circleRend.color -= new Color(0, 0, 0, circleAlphaChange);
			}
		}
		else if (circleRend.color.a > 0.99f) {
			//Alpha at about 1, so start decreasing
			circleAlphaIncreasing = false;
			circleRend.color -= new Color(0, 0, 0, circleAlphaChange);
		}
		else {
			//Alpha at about start, so stop flickering
			circleFlashing = false;
			circleRend.color = circleColor;
		}
	}

	private void initializeFlashCircle() {
		circleFlashing = true;
		circleAlphaIncreasing = true;
		circleRend.color += new Color(0, 0, 0, circleAlphaChange);
	}
}
