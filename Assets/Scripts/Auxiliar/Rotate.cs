using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

	public float speed;
	float angle = 0;

	// Update is called once per frame
	void Update () {
	
		this.transform.Rotate (new Vector3 (0, 0, speed * Time.deltaTime));

	}
}
