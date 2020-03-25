using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BingoController : Task {

	const int SINGULAR = 0;
	const int PLURAL = 1;

	public RosettaWrapper rosetta;
	public string textPrefix;
	public UIScaleFader[] faces;
	public UIAxisBoingScaleHost bingoBoing;
	public GameController gameController;
	public GameObject synchScreen;

	public Text bottlesText;

	public UIFaderScript fader;

	int state;
	float timer;

	// Use this for initialization
	void Start () {

		fader.Start ();

	}

	public void startBingo(Task w, int nbottles, bool isVotingPlayer) {

		w.isWaitingForTaskToComplete = true;
		waiter = w;

		fader.Start ();
		fader.setFadeValue (1.0f);

		//bottlesText.text = bottlesText.text.Replace ("<1>", "" + nbottles);
		if (nbottles == 1) {
			bottlesText.text = rosetta.rosetta.retrieveString (textPrefix, SINGULAR);
		} else {
			string txt = rosetta.rosetta.retrieveString (textPrefix, PLURAL);
			bottlesText.text = txt.Replace ("<1>", "" + nbottles).Replace("<2>", "" + (nbottles*2));
		}

		bingoBoing.reset ();
	
		for (int i = 0; i < faces.Length; ++i) {
			faces [i].scaleOutImmediately ();
		}

		if (isVotingPlayer) {
			fader.fadeIn ();
			bottlesText.enabled = true;
			state = 1;
		} else {
			fader.setFadeValue (1.0f);
			bottlesText.enabled = false;
			//synchScreen.SetActive (true);
			state = 10; // wait for others
		}


	}

	// Update is called once per frame
	void Update () {

		if (state == 0) {

		}

		if (state == 1) {
			timer += Time.deltaTime;
			if (timer > 1.0f) {
				state = 2;

				bingoBoing.go ();

				timer = 0.0f;
			}
		}

		if (state == 2) {
			timer += Time.deltaTime;
			if (timer > 0.5f) {
				state = 3;
				timer = 0.0f;
				faces [0].scaleIn ();
			}
		}

		if (state == 3) {
			timer += Time.deltaTime;
			if (timer > 0.5f) {
				state = 4;
				timer = 0.0f;
				faces [1].scaleIn ();
			}
		}

		if (state == 4) {
			timer += Time.deltaTime;
			if (timer > 0.5f) {
				state = 5;
				timer = 0.0f;
				faces [2].scaleIn ();
			}
		}

		if (state == 5) {
			timer += Time.deltaTime;
			if (timer > 0.75f) {
				timer = 0.0f;
				bottlesText.enabled = true;
				state = 6;
			}
		}

		if (state == 6) {
			if (Input.GetMouseButtonDown (0)) {
				fader.fadeOutTask (this);
				state = 7;
			}
		}

		if (state == 7) { // wait for fadeout to finish
			if (!isWaitingForTaskToComplete) {
				state = 10;
				//gameController.synchNumber++; // tell myself
				////gameController.networkAgent.broadcast ("synch:"); // tell others
			}
		}


		if (state == 10) {
			//if (gameController.synchNumber >= (gameController.nPlayers - 1)) {
			//	gameController.synchNumber = 0;
			//	synchScreen.SetActive (false);
				notifyFinishTask ();
				state = 0;
			//}
		}



	}
}
