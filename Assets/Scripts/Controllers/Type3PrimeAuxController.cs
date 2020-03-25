using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Type3PrimeAuxController : Task {

	public UIFaderScript fader;
	public GameController gameController;

	public UITextAndImageFader agresiva;
	public UITextAndImageFader pasiva;
	public UITextAndImageFader pasivaAgresiva;
	public UITextAndImageFader asertiva;
	public UIScaleFader botonOK;

	public UIFaderScript flechaFader;
	public UITextFader textFader;

	public UIBlinker tickBlink;
	public UIBlinker batsuBlink;

	public UITextAndImageFader[] resultMasks;

	public GameObject syncCanvas;

	int chosenMask; // 0: agresiva   1: pasiva    2: pasivaAgresiva 	3: asertiva
	int turnPlayerChosenMask;

	public int state = 0;
	float timer = 0.0f;

	bool somethingTouched = false;

	bool myTurn;

	public void startType3Aux(Task w) {
		w.isWaitingForTaskToComplete = true;
		waiter = w;
		state = 0;
		timer = 0.0f;
		flechaFader.Start ();
		flechaFader.setFadeValue (0.0f);
		textFader.Start ();
		textFader.setOpacity (0.0f);
		agresiva.Start ();
		pasiva.Start ();
		pasivaAgresiva.Start ();
		asertiva.Start ();
		agresiva.reset ();
		pasiva.reset ();
		pasivaAgresiva.reset ();
		asertiva.reset ();
		somethingTouched = false;
		fader.fadeIn ();
		myTurn = gameController.playerTurn == gameController.localPlayerN;
		for (int i = 0; i < resultMasks.Length; ++i) {
			resultMasks [i].reset ();
		}
		agresiva.fadeIn ();
		pasiva.fadeIn ();
		pasivaAgresiva.fadeIn ();
		asertiva.fadeIn ();

		botonOK.reset ();
		botonOK.scaleOutImmediately ();
	}

	void Start () 
	{
		
	}
	
	void Update () 
	{
		if (state == 0) {

		}

		if (state == 1) {

		}

		if (state == 2) { // wait a little bit and fadeout
			timer += Time.deltaTime;
			if (timer > 1.0f) 
			{
				timer = 0.0f;
				state = 3;

				if (myTurn) 
				{
					//gameController.networkAgent.broadcast ("nschosenmask:" + chosenMask + ":");
				}

				gameController.synchNumber++; // sync other players
				//gameController.networkAgent.broadcast ("synch:");
				fader.fadeOut ();
			}
		}

		if (state == 3) { // wait for all players to be ready
			if (gameController.synchNumber >= gameController.nPlayers) {
				syncCanvas.SetActive (false);
				state = 4;
				fader.fadeIn ();
				gameController.synchNumber = 0; // reset syncs
			}

		}

		if (state == 4) {
			if (myTurn) {
				resultMasks [chosenMask].fadeIn ();
			} else {
				resultMasks [turnPlayerChosenMask].fadeIn ();
				if (chosenMask == turnPlayerChosenMask) {
					tickBlink.startBlinking ();
					flechaFader.fadeOut ();
					textFader.fadeIn ();
				} else
					batsuBlink.startBlinking ();
			}
			state = 5;
		}

		if (state == 5) 
		{
			if (Input.GetMouseButtonDown (0)) 
			{
				fader.fadeOutTask (this);
				state = 6;
			}
		}
		if (state == 6) 
		{
			if (!isWaitingForTaskToComplete) 
			{
				tickBlink.stopBlinking (false);
				batsuBlink.stopBlinking (false);
				notifyFinishTask ();
			}
		}
	}

	public void touchOnPasiva() {
		if (somethingTouched)
			return;

		chosenMask = 1;
		pasivaAgresiva.fadeOut ();
		agresiva.fadeOut ();
		pasiva.fadeOut ();
		asertiva.fadeOut ();
		somethingTouched = true;
		//syncCanvas.SetActive (true);
		if (myTurn) {
			state = 1;
			botonOK.scaleIn ();
		} else {
			syncCanvas.SetActive (true);
			state = 2;
		}
	}

	public void touchOnPasivaAgresiva() {
		if (somethingTouched)
			return;

		chosenMask = 2;

		pasivaAgresiva.fadeOut ();
		agresiva.fadeOut ();
		pasiva.fadeOut ();
		asertiva.fadeOut ();
		somethingTouched = true;

		if (myTurn) {
			state = 1;
			botonOK.scaleIn ();
		} else {
			syncCanvas.SetActive (true);
			state = 2;
		}
	}

	public void touchOnAgresiva() {
		if (somethingTouched)
			return;

		chosenMask = 0;

		pasivaAgresiva.fadeOut ();
		agresiva.fadeOut ();
		pasiva.fadeOut ();
		asertiva.fadeOut ();
		somethingTouched = true;

		if (myTurn) {
			state = 1;

			botonOK.scaleIn ();
		} else {
			state = 2;
			syncCanvas.SetActive (true);
		}
	}

	public void touchOnAsertiva() {
		if (somethingTouched)
			return;

		chosenMask = 3;

		pasivaAgresiva.fadeOut ();
		agresiva.fadeOut ();
		pasiva.fadeOut ();
		asertiva.fadeOut ();
		somethingTouched = true;

		if (myTurn) {
			state = 1;

			botonOK.scaleIn ();
		} else {
			state = 2;
			syncCanvas.SetActive (true);
		}
	}

	public void touchOnOKButton() {
		if (state == 1) {
			state = 2;
			botonOK.scaleOut ();
			syncCanvas.SetActive (true);
		}
	}

	// network callback
	public void setChosenMask(int m) {
		turnPlayerChosenMask = m;
	}
}
