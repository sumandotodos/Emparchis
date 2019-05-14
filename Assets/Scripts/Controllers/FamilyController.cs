using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FamilyController : Task {

	public MasterController masterController;

	public GameController gameController;

	public UIFaderScript fatherFader;
	public UIFaderScript daughterFader;
	public UIFaderScript motherFader;
	public UIFaderScript sonFader;

	public Texture motherHilight;
	public Texture brotherHilight;
	public Texture sisterHilight;
	public Texture fatherHilight;

	public RawImage wholeFamily;

	public AudioClip selectSound_N;

	const float delay = 0.75f;

	public UIFaderScript fader;

	int state;
	float timer;

	public void stop() {
		state = 0;
		timer = 0;
	}

	bool decided = false;

	public void startFamilyActivity(Task w) {

		w.isWaitingForTaskToComplete = true;
		waiter = w;

		timer = 0.0f;

		state = 1;

		fatherFader.Start ();
		motherFader.Start ();
		daughterFader.Start ();
		sonFader.Start ();

		fatherFader.setFadeValue (1.0f);
		motherFader.setFadeValue (1.0f);
		daughterFader.setFadeValue (1.0f);
		sonFader.setFadeValue (1.0f);

		fader.fadeIn ();

	}

	// Use this for initialization
	void Start () {
		state = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (state == 0) { // idling

		} 

		else if (state == 1) { // waiting for touch event

			// do nothing!!

		} 

		else if (state == 2) { // small delay

			timer += Time.deltaTime;
			if (timer > delay) {
				timer = 0.0f;
				fader.fadeOutTask (this);
				state = 3;
			}

		} 

		else if (state == 3) { // wait for fadeout to finish

			if (!isWaitingForTaskToComplete) {
				gameController.networkAgent.broadcast ("synch:");
				gameController.synchCanvas.SetActive(true);
				state = 4; // synch players

			}

		} 

		else if (state == 4) { // synch players
			if (gameController.synchNumber >= gameController.nPlayers - 1) {
				gameController.synchCanvas.SetActive(false);
				gameController.synchNumber = 0;
				state = 0;
				masterController.startActivity = "ChoosePlayer";
				notifyFinishTask (); // return to parent task
			}
		}

	}

	// event callbacks
	public void touchOnSister() {

		if (decided)
			return;
		decided = true;
		//wholeFamily.texture = sisterHilight;
		if (selectSound_N != null) {
			masterController.playSound (selectSound_N);
		}
		gameController.currentPlayerRole = Family.daughter;
		state = 2;

		sonFader.fadeIn ();
		motherFader.fadeIn ();
		fatherFader.fadeIn ();

	}

	public void touchOnFather() {
		if (decided)
			return;
		decided = true;
		//wholeFamily.texture = fatherHilight;
		if (selectSound_N != null) {
			masterController.playSound (selectSound_N);
		}
		gameController.currentPlayerRole = Family.father;
		state = 2;

		sonFader.fadeIn ();
		motherFader.fadeIn ();
		daughterFader.fadeIn ();
	}

	public void touchOnMother() {
		if (decided)
			return;
		decided = true;
		//wholeFamily.texture = motherHilight;
		if (selectSound_N != null) {
			masterController.playSound (selectSound_N);
		}
		gameController.currentPlayerRole = Family.mother;
		state = 2;

		sonFader.fadeIn ();
		daughterFader.fadeIn ();
		fatherFader.fadeIn ();
	}

	public void touchOnBrother() {
		if (decided)
			return;
		decided = true;
		//wholeFamily.texture = brotherHilight;
		if (selectSound_N != null) {
			masterController.playSound (selectSound_N);
		}
		gameController.currentPlayerRole = Family.son;	
		state = 2;

		daughterFader.fadeIn ();
		motherFader.fadeIn ();
		fatherFader.fadeIn ();
	}
}
