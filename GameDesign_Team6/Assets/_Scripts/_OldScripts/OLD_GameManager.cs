using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OLD_GameManager : MonoBehaviour {

	public GameObject[] enemiesToSpawn;
	public GameObject player;
	public static OLD_GameManager instance = null;

	// Use this for initialization
	void Awake () {
		//Creates player
		Instantiate (player, randomVector2(), Quaternion.identity);

		//Creates all enemies for this level
		foreach (GameObject curEnemy in enemiesToSpawn) {
			Instantiate(curEnemy, randomVector2(), Quaternion.identity);
		}
	}

	Vector2 randomVector2() {
		return new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
