using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//The health bar. - Allison Lee

public class HealthBar : MonoBehaviour {

	private GameObject player;

	private UnityEngine.UI.Slider healthBar;
	private GameObject hpText;

	void Start () {
		healthBar = GetComponent<UnityEngine.UI.Slider>();
		hpText = GameObject.Find("HpText");
	}

	void Update () {
		// Can't find Player object until it is spawned, and the Start() function is called before that
		if (player == null) {
			player = GameObject.FindGameObjectWithTag("Player");
		}
		setHealthBar();
	}

	public virtual void setHealthBar()
	{
		float health = player.GetComponent<PlayerController>().healthPoints;
		float maxHealth = player.GetComponent<PlayerController>().maxHealthPoints;
		if(this.healthBar != null)
		{
			healthBar.value = health / maxHealth;
		}

		hpText.GetComponent<Text>().text = health + " / " + maxHealth;
	}
}
