using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Type3PrimeTestController_mono : Task {


	public GameObject[] icons;


	public string textPrefix;
	public UITextFader titleFader;
	public UITextFader descrFader;


	public UIFaderScript fader;

	float angle;
	public float angleSpeed = 6.0f;

	const float delay = 5.0f;

	float timer;

	int state = 0;

	bool isNegativeSituation;

	public StringBank subtypeText;
	public RosettaWrapper rosetta;


	int subType;// 0: solución creativa
				// 1: Equilibrio
				// 2: Tipo de respuesta


	public void startType3Test(Task w) {
		titleFader.Start ();
		titleFader.reset ();
		descrFader.Start ();
		descrFader.reset ();
		w.isWaitingForTaskToComplete = true;
	
		waiter = w;
		timer = 0;
		state = 1;
		timer = 0.0f;
		subType = Random.Range (0, 3);  // WARNING  must be random
		if (MasterController_mono.ForceTest3Subtest != -1) {
			subType = MasterController_mono.ForceTest3Subtest;
		}
		Debug.Log("<color=blue>Source subtype</color> : " + subType);
		subtypeText.rosetta = rosetta.rosetta;
		subtypeText.reset ();
		fader.fadeIn ();
	}

	void Start () {
		//startType3Test (this);

	}

	public void showTextEvent() {
		
	}

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
