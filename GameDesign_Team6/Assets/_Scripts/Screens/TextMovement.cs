using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * People that touched this script: Matt Bryan
**/

public class TextMovement : MonoBehaviour {

	private float num = 1f;
	private bool isDecreasing = true;
	private float xPos;
	private float zPos;

	// Use this for initialization
	void Start () {
		xPos = transform.position.x;
		zPos = transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {
		hover();
	}

	private void hover() {
		transform.position = new Vector3(xPos, Mathf.Sin(num) + transform.position.y, zPos);
		if (isDecreasing) {
			num -= 0.05f;
		}
		else {
			num += 0.05f;
		}
		if (num <= -1f) {
			isDecreasing = false;
		}
		else if (num >= 1f) {
			isDecreasing = true;
		}
	}
}
