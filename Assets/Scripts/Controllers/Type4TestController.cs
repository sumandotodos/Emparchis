using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Type4TestController : Task {

	public UIFaderScript fader;


	public RosettaWrapper rosetta;
	public string textPrefix;

	public UITextFader titleFader;
	public UITextFader descrFader;


	float timer;
	const float delay = 5.0f;

	int state = 0;

	public void startType4Test(Task w) {
		titleFader.Start ();
		titleFader.reset ();
		descrFader.Start ();
		descrFader.reset ();
		w.isWaitingForTaskToComplete = true;
		waiter = w;
		state = 1;
		timer = 0.0f;
		fader.fadeIn ();
	}

	// Use this for initialization
	void Start () {
		//startType2Test (this);
	}

	// Update is called once per frame
	void Update () {

		if (state == 0) { // idling

		}

		if (state == 1) {
			timer += Time.deltaTime;
			if (timer > 0.75f) {
				titleFader.fadeIn ();
				state = 2;
				timer = 0;
			}
		}

		if (state == 2) {
			timer += Time.deltaTime;
			if (timer > 0.75f) {
				descrFader.fadeIn ();
				state = 3;
			}
		}

		if (state == 3) { // delaying
			//timer += Time.deltaTime;
			if (timer > 30f) {
				fader.fadeOutTask (this);
				state = 4;
			}

			if (Input.GetMouseButtonDown (0)) {
				timer = 31f;
			}
		}

		if (state == 4) { // waiting for fadeout
			if (!isWaitingForTaskToComplete) {
				notifyFinishTask ();
				state = 0;
			}
		}
	}


}

