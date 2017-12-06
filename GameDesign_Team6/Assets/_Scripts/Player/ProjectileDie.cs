using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** Touched by Allison Lee, Matt Bryan, Matthew Stewart **/

public class ProjectileDie : MonoBehaviour {

	public AudioSource efxSource;

	public Vector3 originalPosition;
	public float attackRange;
	public float attackDamage;
	public GameObject particle;
	public bool rotateProjectile;
	//If this is set to true, the projectile will rotate to face the mouse when fired.
	public float lifetime;
	//If this is set to 0, it will remain alive until it hits something.
	//Otherwise, it'll stay alive until its lifetime expires.
	private float Timer = 0.0f;

	// Update is called once per frame
	void Update () {
		Timer += Time.deltaTime;
		if (lifetime != 0.0f && Timer >= lifetime) {
			Destroy (this.gameObject);
		}

		float distance = Vector3.Distance (originalPosition, this.gameObject.transform.position);
		if (Mathf.Abs(distance) > attackRange) {
			Destroy (this.gameObject);
		}
	}
		
	void setAttackRange(float range)
	{
		attackRange = range;
	}

	void setAttackDamage(float damage)
	{
		attackDamage = damage;
	}

	private void OnTriggerEnter2D (Collider2D other) {
		if (lifetime > 0) {
			//If it has a lifetime, it's a passive move and shouldn't have any
			//trigger properties!
			return;
		}
		if (other.CompareTag("Wall")) {
			if (particle != null) {
				GameObject explosion = Instantiate (particle, transform.position, Quaternion.identity);
				Destroy (explosion, 2.0f);
			}
			Destroy(this.gameObject, 0.1f);
			//There's a slight delay on destroy

			efxSource.Play();
		}
		if (other.CompareTag ("Enemy")) {
			if (particle != null) {
				GameObject explosion = Instantiate (particle, transform.position, Quaternion.identity);
				Destroy (explosion, 2.0f);
			}
			Destroy(this.gameObject, 0.1f);

			efxSource.Play();
		}
	}
}
