using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Axis { x, y };

public class UIAxisScaleBoing : MonoBehaviour {

	//public Axis axis;

	public float value;
	public float timer;

	public float freq;
	public float speed;

	public float maxScale;
	public float minScale;

	bool going = false;

	public void reset() {
		timer = 0.0f;
		value = 0.0f;
		this.transform.localScale = Vector3.zero;
		going = false;
	}

	// Use this for initialization
	void Start () {
		reset ();
	}

	public void go() {
		going = true;
	}
	
	// Update is called once per frame
	void Update () {

		if (!going)
			return;

		if (timer * speed > 1000) { 
			value = maxScale - (maxScale - minScale) * Mathf.Cos (timer) / (timer * speed);
		} else
			value = maxScale;
		timer += Time.deltaTime;
		Vector3 sc = this.transform.localScale;
			sc.x = value;
			sc.y = value;
		this.transform.localScale = sc;

	}
}
