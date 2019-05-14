using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDelayFader : MonoBehaviour {

	public UIFaderScript fader;
	public UITextFader textFader;
	public float delay;
	float remainingTime;

	public bool going = true;

	public bool fadeIn = true;

	public void resetTimer() {
		remainingTime = delay;
	}

	// Use this for initialization
	void Start () {
		remainingTime = 0.0f;
		if (going)
			resetTimer ();
	}
	
	// Update is called once per frame
	void Update () {
		if (going) {
			if (remainingTime > 0.0f) {
				remainingTime -= Time.deltaTime;
				if (remainingTime <= 0.0f) {
					if (fader != null) {
						if (fadeIn)
							fader.fadeIn ();
						else
							fader.fadeOut ();
					}
					if (textFader != null) {
						if (fadeIn)
							textFader.fadeIn ();
						else
							textFader.fadeOut ();
					}
				}
			}
		}
	}
}
