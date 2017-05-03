using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomingShips : MonoBehaviour {
	public GameObject warningText;
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag ("Player")) {
			
		}
	}
}
