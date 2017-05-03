using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	public ParticleSystem thrusters;
	public ParticleSystem explosion;
	public GameObject bullet;
	public GameObject squadron1;
	public GameObject squadron2;
	public Image healthBar;
	public float speed;
	public int spread;
	public int health;
	private int gunState;

	#region gameplay functions
	void fire(Vector3 position, int gun) {
		Quaternion rotation = transform.rotation;
		Vector3 eulerAngles = rotation.eulerAngles;
		eulerAngles.x = 0;
		rotation = Quaternion.Euler (eulerAngles);

		if (gun == 1) { // normal bullet
			var bulletObject = (GameObject)Instantiate (bullet, position, rotation);
			bulletObject.GetComponent<Rigidbody> ().velocity = bulletObject.transform.forward * 100;

			Destroy (bulletObject, 2.0f);
		} else if (gun == 2) { // shotgun-style bullets
			for (int i = 0; i < spread; i++) {
				Quaternion modifiedRotation = rotation *
				                              Quaternion.Euler (Vector3.up * (Random.value * 20 - 10)) *
				                              Quaternion.Euler (Vector3.right * (Random.value * 20 - 10));

				var bulletObject = (GameObject)Instantiate (bullet, position, modifiedRotation);
				bulletObject.GetComponent<Rigidbody> ().velocity = bulletObject.transform.forward * 100;

				Destroy (bulletObject, 2.0f);
			}
		} else if (gun == 3) { // homing rockets

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
		// change squadron 1 position
		Vector3 temp = squadron1.transform.position;
		temp.y = transform.position.y;
		squadron1.transform.position = temp;

		// change squadron 2 position
		temp = squadron2.transform.position;
		temp.y = transform.position.y;
		squadron2.transform.position = temp;

		// change guns
		if (Input.GetKeyDown (KeyCode.Alpha1))
			gunState = 1;
		else if (Input.GetKeyDown (KeyCode.Alpha2))
			gunState = 2;

		// shoot gun
		if (Input.GetKeyDown (KeyCode.Space)) {
			fire (GameObject.Find("gun").transform.position, gunState);

			if (GameObject.Find("gun1") != null)
				fire (GameObject.Find("gun1").transform.position, 1);

			if (GameObject.Find("gun2") != null)
				fire (GameObject.Find("gun2").transform.position, 1);
		}

		// display health
		healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, (float)health / 100, Time.deltaTime);
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
		case "Enemy Bullet":
			health -= 10;
			break;
		case "Asteroid": case "Enemy": case "Station":
			health -= 100;
			break;
		}

		if (!other.CompareTag ("Player Bullet") && !other.CompareTag("Squadron Member"))
			Instantiate (explosion, other.transform.position, other.transform.rotation);

		if (other.CompareTag ("Asteroid"))
			Destroy (other.gameObject);
	}
}
