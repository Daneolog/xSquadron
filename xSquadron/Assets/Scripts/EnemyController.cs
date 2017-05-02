using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
	public ParticleSystem explosion;
	public GameObject bullet;
	public float speed;
	public int shootRate;
	private Transform player;
	private int health;

	void fire() {
		Vector3 position = transform.position;
		Quaternion rotation = transform.rotation;

		var bulletObject = (GameObject)Instantiate (bullet, position, rotation);
		bulletObject.GetComponent<Rigidbody> ().velocity = bulletObject.transform.forward * 100;

		Destroy (bulletObject, 2.0f);
	}

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player").transform;

		health = 100;
		speed /= 10;

		InvokeRepeating ("fire", shootRate, shootRate);
	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0)
			Destroy (gameObject);
	}

	void FixedUpdate () {
		Quaternion initial = transform.rotation;
		transform.LookAt(player);
		Quaternion final = transform.rotation;
		transform.rotation = Quaternion.Lerp(initial, final, .01f);

		transform.Translate (new Vector3 (0, 0, speed));
	}

	void OnTriggerEnter(Collider other) {
		switch (other.tag) {
		case "Player Bullet":
			health -= 50;
			break;
		case "Asteroid": case "Player": case "Squadron Member":
			health -= 100;
			break;
		}

//		switch (other.tag) {
//		case "Player Bullet": case "Asteroid": case "Player": case "Squadron Member":
//			Instantiate (explosion, other.transform.position, other.transform.rotation);
//			break;
//		}

		if (!other.CompareTag ("Enemy Bullet") && !other.CompareTag("Station"))
			Instantiate (explosion, other.transform.position, other.transform.rotation);

		if (other.CompareTag ("Asteroid"))
			Destroy (other.gameObject);
	}
}
