using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Allison Lee fuddled with this thing or whatever

public class ProjectileDie : MonoBehaviour {

	private Vector3 position;
	public float attackRange;

	// Use this for initialization
	void Start () {
		position = gameObject.transform.position;
	}

	// Update is called once per frame
	void Update () {
		float distance = Vector3.Distance (position, gameObject.transform.position);
		if (Mathf.Abs(distance) > attackRange) {
			Destroy (gameObject);
		}

	}

	void setAttackRange(float range)
	{
		attackRange = range;
	}

	private void OnTriggerEnter2D (Collider2D other) {
		Debug.Log("Hit Wall");
		if (other.CompareTag("Wall")) {
			Destroy(this.gameObject);
		}
		Debug.Log(other.tag);
	}
}
