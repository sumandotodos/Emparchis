using UnityEngine;
using System.Collections;

public class RouletteController_mono : Task, ButtonPressListener {

	public GameController_mono gameController;
	public MasterController_mono masterController;
	public MainGameController_mono mainGameController;

	public AudioClip rouletteTick;

	public GameObject wheel;
	public GameObject arrow;
	public UIOpacityWiggle wheelSelection;
	public UIFaderScript fader;

	public float maxAngSpeed = -20.0f;

	public float angSpeed;
	float angle;
	public float angAccel = 2.0f;
	const float SpeedThreshold = 0.1f;
	public float clampedAngle;
	public float clampledAngle_5;

	public int selectedItem = -1;

	float initialSpeedSign;

	const float delay = 3.0f;

	int state = 0;
	float timer;

	float timeToStartBraking;
	const float minBrakingTime = 0.2f;
	const float maxBrakingTime = 5.0f;

	float finishAngle;
	const float minFinishAngle = 360.0f * 3.0f;
	const float maxFinishAngle = 360.0f * 5.0f;

	float T;

	bool hasTicked = false;

	bool rouletteCanSpin = true; // prevent a second button push

	public void buttonPress() {

		if (rouletteCanSpin == false)
			return;

		rouletteCanSpin = false;



		timer = 0.0f;
		state = 1;
	}



	public void startRouletteActivity(Task w) {
		w.isWaitingForTaskToComplete = true;
		waiter = w;
		angSpeed = 0.0f;
		angle = 0.0f;
		state = 0;
		fader.fadeIn ();
		// choose a finish angle that does not conflict with arrow zone
		finishAngle = Random.Range (minFinishAngle, maxFinishAngle);
		float cAngle = finishAngle - Mathf.Floor (finishAngle / (360.0f / 5.0f)) * (360.0f / 5.0f);
		while ((cAngle + 5.0f) < 16.0f) {
			finishAngle = Random.Range (minFinishAngle, maxFinishAngle);
			cAngle = finishAngle - Mathf.Floor (finishAngle / (360.0f / 5.0f)) * (360.0f / 5.0f);
		}
		T = Mathf.Sqrt ((2 * finishAngle) / angAccel);
		angle = finishAngle - 0.5f * angAccel * (T) * (T);
		wheel.transform.localRotation = Quaternion.Euler (0, 0, -angle);
		rouletteCanSpin = true;
		wheelSelection.reset ();
		wheelSelection.transform.rotation = Quaternion.Euler (Vector3.zero);

	}
	
	// Update is called once per frame
	void Update () {

		if (state == 0) { // idling

		}


		if (state == 1) { // spinning the wheel
			// do a very simple wheel dynamics update 
			//  (using a 2d rigid body would be overkill, I'm afraid)

			if (timer < T) {
				angle = finishAngle - 0.5f * angAccel * (T - timer) * (T - timer);
				timer += Time.deltaTime;
			} else {
				angle = finishAngle;
				timer = 0.0f;
				state = 2;
				selectedItem = 4-(int)Mathf.Floor ((angle - Mathf.Floor (angle / 360.0f) * 360.0f) / 72.0f);
				if(MasterController_mono.ForceTest != -1) selectedItem = MasterController_mono.ForceTest;

				mainGameController.tType = selectedItem;

				gameController.selectedItem = selectedItem;

				wheelSelection.transform.Rotate (0, 0, -72.0f * selectedItem);
				wheelSelection.go ();
			}



			clampedAngle = angle - Mathf.Floor (angle / (360.0f / 5.0f)) * (360.0f / 5.0f);


			if ((clampedAngle + 5.0f) < 16.0f) {
				if (!hasTicked) {
					masterController.playSound (rouletteTick);
					hasTicked = true;
				}
				arrow.transform.localRotation = Quaternion.Euler (0, 0, 6.0f * (clampedAngle + 5.0f));
			} else {
				hasTicked = false;
				arrow.transform.localRotation = Quaternion.Euler (0, 0, 0);
			}

			wheel.transform.localRotation = Quaternion.Euler (0, 0, -angle);

		}


		if (state == 2) { // small delay
			timer += Time.deltaTime;
			if (timer > delay) {
				fader.fadeOutTask (this);
				state = 3;
			}
		}

		if (state == 3) { // waiting for fadeout
			if (!isWaitingForTaskToComplete) {
				wheelSelection.transform.Rotate (0, 0, 72.0f * selectedItem);
				wheelSelection.reset ();
				state = 0;
				mainGameController.rouletteResult = selectedItem;
				notifyFinishTask ();
			}
		}
	}
}
