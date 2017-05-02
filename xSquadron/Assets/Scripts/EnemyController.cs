using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
	public ParticleSystem explosion;
	public float speed;
	private Transform player;
	private int health;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player").transform;

		health = 100;
		speed /= 10;
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
		if (other.CompareTag ("Bullet")) {
			Instantiate (explosion, other.transform.position, other.transform.rotation);
			health -= 50;
			Destroy (other.gameObject);
		}
	}
}
