using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAutoDelayDisable : MonoBehaviour {

	public float delay = 2.5f;
	float remaining;

	void Start () 
	{
		remaining = delay;
	}
	
	void Update () 
	{
		if (remaining > 0.0f) {
			remaining -= Time.deltaTime;
			if (remaining <= 0.0f) {
				remaining = delay;
				this.gameObject.SetActive (false);
			}
		}
	}
}
