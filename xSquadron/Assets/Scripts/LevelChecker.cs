using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChecker : MonoBehaviour {
	public int level;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		switch (level) {
		case 1:
			if (GameObject.FindGameObjectsWithTag ("Station").Length == 0)
				SceneManager.LoadScene ("Level 1 Success");
			break;
		}
	}
}
