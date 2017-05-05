using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public GameObject player;
	private Vector3 offset;

	// Use this for initialization
	void Start () {
		offset = transform.position - player.transform.position;
	}

	void Update () {
		float angle = player.transform.eulerAngles.y;
		Quaternion rotation = Quaternion.Euler (180, angle, 0);

		if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift))
			offset = Vector3.Lerp (offset, new Vector3 (0, 16, -40), 5 * Time.deltaTime);
		else
			offset = Vector3.Lerp (offset, new Vector3 (0, 6, -13), 10 * Time.deltaTime);

		transform.position = player.transform.position - (rotation * offset);
		transform.LookAt (player.transform);
	}
}
