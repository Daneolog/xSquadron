using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationController : MonoBehaviour {
	public ParticleSystem explosion;
	public Rigidbody enemy;
	public Transform spawner;
	public int spawnRate;
	private int health;

	void CreateEnemy() {
		Instantiate (enemy, spawner.position, spawner.rotation);
	}

	// Use this for initialization
	void Start () {
		InvokeRepeating ("CreateEnemy", 0, spawnRate);
		health = 500;
	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0)
			Destroy (gameObject);
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag ("Bullet")) {
			Instantiate (explosion, other.transform.position, other.transform.rotation);
			health -= 50;
			Destroy (other.gameObject);
		}
	}
}
