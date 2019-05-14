using UnityEngine;
using System.Collections;

public class LogoController_mono : Task {

	public UIFaderScript fader;

	int state;
	float timer;

	void init() {

		fader.fadeIn ();
		state = 1;
		timer = 0.0f;
	}

	public void startLogoActivity(Task w) {

		w.isWaitingForTaskToComplete = true;
		waiter = w;

		init ();

	}

	void Start() {
		state = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (state == 0) { // idle

		}

		if (state == 1) { // wait for user input touch
			timer += Time.deltaTime;
			if (timer > 3.0f) {
				fader.fadeOutTask (this);
				state = 2;
			}
			if (Input.GetMouseButtonDown (0))
				timer = 3.0f;
		}

		if (state == 2) { // wait for fadeout to complete
			if (!isWaitingForTaskToComplete) {
				notifyFinishTask (); // return to parent task
				state = 0;
			}
		}

	}
}
