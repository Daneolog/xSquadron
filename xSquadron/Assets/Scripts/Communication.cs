using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Communication : MonoBehaviour {
	public GameObject panel;
	public string text;
	private bool passed;

	void closeNotification() {
		passed = false;
	}

	void moveDown() {
		if (panel.GetComponent<RectTransform> ().anchoredPosition.y > -40)
			panel.GetComponent<RectTransform> ().anchoredPosition.y--;
	}

	void moveUp() {
		if (panel.GetComponent<RectTransform> ().anchoredPosition.y > -40)
			panel.GetComponent<RectTransform> ().anchoredPosition.y--;
	}

	// Use this for initialization
	void Start() {
		passed = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag ("Player"))
			passed = true;
	}
}
