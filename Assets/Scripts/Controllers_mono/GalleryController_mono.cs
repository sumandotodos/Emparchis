using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GalleryController_mono : Task {

	public static int maxGifts = 6;


	public RawImage[] buttonBase;
	public Texture lockedButton;
	public Texture playableButton;
	public string[] links;
	public UIFaderScript fader;

	public GameController_mono gameController;

	int state = 0;

	public void stop() {
		state = 0;

	}

	public void startGalleryActivity(Task w) {
		w.isWaitingForTaskToComplete = true;
		waiter = w;

		for (int i = 0; i < buttonBase.Length; ++i) {
			buttonBase [i].texture = lockedButton;

		}

		for (int i = 0; i < gameController.obtainedGifts.Count; ++i) {
			buttonBase [gameController.obtainedGifts [i]].texture = playableButton;

		}

		fader.Start ();
		fader.setFadeValue (1.0f);
		fader.fadeIn ();

	}

	void Update() {

		if (state == 0)
			return;

		if (state == 10) {
			fader.fadeOutTask (this);
			state = 11;
		}
		if (state == 11) {
			if (!isWaitingForTaskToComplete) {
				state = 0;
				notifyFinishTask ();
			}
		}

	}

	// event callbacks

	public void touchButton(int but) {
		if (gameController.obtainedGifts.Contains (but)) {
			Application.OpenURL (links [but]);
		}
	}

	public void returnButton() {
		state = 10; // do a fadeout and finish task
	}

}
