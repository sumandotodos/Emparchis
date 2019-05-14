using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Type3TestController_mono : Task {

	public GameObject centerWheel;
	public GameObject[] icons;

	public RosettaWrapper rosetta;
	public string textPrefix;
	public Text titleText;
	public UITextFader titleFader;

	public UIFaderScript fader;

	float angle;
	public float angleSpeed = 6.0f;

	const float delay = 5.0f;

	float timer;

	int state = 0;

	bool isNegativeSituation;


	public void startType3Test(Task w) {
		titleFader.Start ();
		titleFader.reset ();
		titleText.text = rosetta.rosetta.retrieveString (textPrefix, 3);
		w.isWaitingForTaskToComplete = true;
		waiter = w;
		timer = 0;
		state = 1;
		timer = 0.0f;
		fader.fadeIn ();
	}

	// Use this for initialization
	void Start () {
		//startType3Test (this);

	}
	
	// Update is called once per frame
	void Update () {
	
		if (state == 0) {

		}

		if (state == 1 || state == 2 || state == 3) {
			centerWheel.transform.Rotate (0, 0, angleSpeed);
			for (int i = 0; i < icons.Length; ++i) {
				icons [i].transform.Rotate (0, 0, -angleSpeed);
			}
			if (Input.GetMouseButtonDown (0)) {
				timer = delay;
			}
		}

		if (state == 1) {
			timer += Time.deltaTime;
			if (timer > 0.75f) {
				titleFader.fadeIn ();
				state = 2;
			}
		}

		if (state == 2) {
			
			timer += Time.deltaTime;
			if (timer > delay) {
				state = 3;
				fader.fadeOutTask (this);
			}
		}

		if (state == 3) {
			if (!isWaitingForTaskToComplete) {
				//waiter.bReturnValue = //
				notifyFinishTask ();
				state = 0;
			}
		}

	}
}
