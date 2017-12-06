using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OLD_PlayerController : MonoBehaviour {

	public Rigidbody2D playerRb;
	public float maxSpeed;
	public GameObject projectile;
	public int projectileSpeed;
	public float health = 3.0f;
	public Text playerHealthText;
    public GenerateLevel genLevel; // Is this needed?

	private Animator PlayerAnimator;

	void Start()
	{
		PlayerAnimator = GetComponent<Animator> ();
		//reference the animator to allow running animation
	}

	void Update() {
		if (health <= 0) {
			Destroy (this.gameObject);
		}

		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		if (Input.GetMouseButtonDown(0)) {
			PlayerAnimator.SetFloat ("attack", 1);
			Shoot (mousePos);
		} else if (Input.GetMouseButtonDown(1)) {
			//Defense / Special / Etc
		}
		//playerHealthText.text = "Player Health: " + health;

		//Face the sprite towards the mouse position
		transform.rotation = Quaternion.Euler(0, (mousePos.x - transform.position.x) < 0 ? 180 : 0,0);
		PlayerAnimator.SetFloat ("speed", Mathf.Abs (playerRb.velocity.x)+Mathf.Abs(playerRb.velocity.y)); 
		//Sets "speed" to velocity to allow the animator to change states.
	}

	// Update is called once per frame
	void FixedUpdate () {
		PlayerAnimator.SetFloat ("attack", 0);
		float horz = Input.GetAxis ("Horizontal");
		float vert = Input.GetAxis ("Vertical");

		float velocityMagnitudeH = maxSpeed * horz;
		float velocityMagnitudeV = maxSpeed * vert;
		Vector2 newVelocity = new Vector2 (velocityMagnitudeH, velocityMagnitudeV);

		playerRb.velocity = newVelocity;
	}

	private void Shoot(Vector2 mousePos) {
		//Changes the animator to attacking animations.
		GameObject shot = Instantiate (projectile, transform.position, Quaternion.identity);
		Rigidbody2D shotRb = shot.GetComponent<Rigidbody2D>();

		//Velocity vector from player to mouse position
		Vector2 shotVelocityVector = ((Vector2)mousePos - (Vector2)transform.position);

		//Normalize velocity vector, multiply times projectile speed, add force to shot
		shotRb.AddForce (shotVelocityVector.normalized * projectileSpeed);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Enemy")) {
			health -= 0.5f;
		}

        if (other.CompareTag("Exit"))
           restart();
	}

    
    public void restart() {
        SceneManager.LoadScene("TestProceduralPlacement");
    }
}
