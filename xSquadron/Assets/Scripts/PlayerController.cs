﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	// level number
	public int level;

	// objects
	public ParticleSystem thruster;
	public ParticleSystem explosion;
	public GameObject bullet;
	public GameObject squadron1, squadron2, squadron3;
	public AudioSource shots;
	public AudioSource thrusters;
	public Image healthBar;

	// constants
	public float speed;
	public int reload;
	public int spread;
	public int health;
	private int gunState;

	// other variables
	private int currentReload;

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

		shots.Play ();
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

		// change squadron 3 position
		temp = squadron3.transform.position;
		temp.y = transform.position.y;
		squadron3.transform.position = temp;

		// change gun
		if (Input.GetKeyDown (KeyCode.Alpha1))
			gunState = 1;
		else if (Input.GetKeyDown (KeyCode.Alpha2))
			gunState = 2;

		// shoot gun
		if (Input.GetKey (KeyCode.Space) && currentReload >= reload) {
			currentReload = 0;
			fire (transform.Find ("gun").transform.position, gunState);
		}

		// display health
		healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, (float)health / 100, Time.deltaTime);

		// failure
		if (health <= 0) {
			Destroy (gameObject);
			SceneManager.LoadScene ("Level " + level + " Failure");
		}

		if (currentReload < reload)
			currentReload++;
	}

	void FixedUpdate () {
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");

		// create thruster particles
		if (vertical > 0) {
			if (!thruster.isPlaying)
				thruster.Play ();

			thrusters.Play ();
		} else if (thruster.isPlaying)
				thruster.Stop ();

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
		temp.y = 0;
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
