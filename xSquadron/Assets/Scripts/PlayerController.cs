using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
	public ParticleSystem thrusters;
	public ParticleSystem explosion;
	public GameObject bullet;
	public float speed;
	public int spread;
	private int health;
	private int gunState;

	#region gameplay functions
	void fire() {
		Vector3 position = GameObject.Find("gun").transform.position;
		Quaternion rotation = transform.rotation;

		if (gunState == 1) { // normal bullet
			var bulletObject = (GameObject)Instantiate (bullet, position, rotation);
			bulletObject.GetComponent<Rigidbody> ().velocity = bulletObject.transform.forward * 100;

			Destroy (bulletObject, 2.0f);
		} else if (gunState == 2) { // shotgun-style bullets
			for (int i = 0; i < spread; i++) {
				Quaternion modifiedRotation = rotation *
				                              Quaternion.Euler (Vector3.up * (Random.value * 20 - 10)) *
				                              Quaternion.Euler (Vector3.right * (Random.value * 20 - 10));

				var bulletObject = (GameObject)Instantiate (bullet, position, modifiedRotation);
				bulletObject.GetComponent<Rigidbody> ().velocity = bulletObject.transform.forward * 100;

				Destroy (bulletObject, 2.0f);
			}
		} else if (gunState == 3) { // homing rockets

		}
	}
	#endregion

	// Use this for initialization
	void Start () {
		health = 100;
		speed /= 10;
		gunState = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha1))
			gunState = 1;
		else if (Input.GetKeyDown (KeyCode.Alpha2))
			gunState = 2;

		if (Input.GetKeyDown (KeyCode.Space))
			fire ();

		if (health <= 0) {
			Destroy (gameObject);
			SceneManager.LoadScene ("Failure");
		}
	}

	void FixedUpdate () {
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");

		// create thruster particles
		if (vertical > 0) {
			if (!thrusters.isPlaying)
				thrusters.Play ();
		} else if (thrusters.isPlaying)
				thrusters.Stop ();

		// change rotation
		transform.Rotate(new Vector3(0, horizontal, 0));

		// tilt spaceship
		Vector3 euler = transform.localEulerAngles;
		euler.z = Mathf.LerpAngle(euler.z, -horizontal * 15, 3 * Time.deltaTime);
		euler.x = Mathf.LerpAngle(euler.x, vertical * 10, 3 * Time.deltaTime);
		transform.localEulerAngles = euler;

		// translate forwards
		transform.Translate (new Vector3 (0, 0, speed));
		transform.Translate (new Vector3 (0, 0, speed * vertical));

		// reset transform position
		Vector3 temp = transform.position;
		temp.y = 1.6f;
		transform.position = temp;
	}

	void OnTriggerEnter(Collider other) {
		switch (other.tag) {
		case "Asteroid":
			health -= 20;
			break;
		case "Enemy":
			health -= 50;
			if (health <= 0)
				SceneManager.LoadScene ("Failure");
			break;
		case "Station":
			health -= 90;
			break;
		}

		if (!other.CompareTag ("Bullet") && !other.CompareTag("Squadron Member")) {
			Instantiate (explosion, other.transform.position, other.transform.rotation);
			Destroy (other.gameObject);
		}
	}
}
