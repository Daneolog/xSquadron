using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public GameObject player;
	public float damping;
	private Vector3 offset;

	// Use this for initialization
	void Start () {
		offset = transform.position - player.transform.position;
	}

	void LateUpdate () {
		float angle = player.transform.eulerAngles.y;
		Quaternion rotation = Quaternion.Euler (180, angle, 0);

		transform.position = player.transform.position - (rotation * offset);
		transform.LookAt (player.transform);
	}
}
