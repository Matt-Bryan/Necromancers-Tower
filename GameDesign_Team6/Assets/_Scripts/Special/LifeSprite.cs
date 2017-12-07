using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * People that worked on this script: 
 * Matt Bryan
 *
 * This script is deprecated, as we have a different way we show health
**/

public class LifeSprite : MonoBehaviour {

	public Sprite[] spriteList = new Sprite[3];

	private PlayerController player;
	private SpriteRenderer rend;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		rend = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		float hp = player.healthPoints;

		if (hp <= 0) {
			rend.sprite = spriteList[2];
		}
		else if (hp <= 10) {
			rend.sprite = spriteList[1];
		}
		else {
			rend.sprite = spriteList[0];
		}
	}
}
