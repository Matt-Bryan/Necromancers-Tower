using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * People that worked on this script: 
 * Allison Lee
**/

public class CooldownBar : MonoBehaviour {

	private GameObject player;

	private UnityEngine.UI.Slider cooldownBar;

	void Start () {
		cooldownBar = GetComponent<UnityEngine.UI.Slider>();
	}

	void Update () {
		// Can't find Player object until it is spawned, and the Start() function is called before that
		if (player == null) {
			player = GameObject.FindGameObjectWithTag("Player");
		}
		setCooldownBar();
	}

	public virtual void setCooldownBar()
	{
		float timer = player.GetComponent<PlayerController>().Timer;
		float maxTimer = player.GetComponent<PlayerController>().SpecialAttackTimer;
		float percent = timer / maxTimer;

		if (percent > 1) {
			percent = 1;
		}

		if(this.cooldownBar != null)
		{
			cooldownBar.value = percent;
		}
	}
}
