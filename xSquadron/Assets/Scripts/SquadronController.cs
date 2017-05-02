using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadronController : MonoBehaviour {
	public Transform target;
	public ParticleSystem explosion;
	public ParticleSystem thrusters;
	public float speed;
	private float health;

	// Use this for initialization
	void Start () {
		health = 100;
		speed /= 10;
	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0)
			Destroy (gameObject);
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

		transform.position = Vector3.Lerp (transform.position, target.position, 100 * speed * Time.deltaTime);
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
