using UnityEngine;
using System.Collections;

public class RouletteController : Task, ButtonPressListener {

	public GameController gameController;
	public MasterController masterController;
	public MainGameController mainGameController;

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

		gameController.networkAgent.broadcast ("rouletteangle:" + finishAngle + ":" + gameController.turn + ":");

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
				if(MasterController.ForceTest != -1) selectedItem = MasterController.ForceTest;
				//if (selectedItem == 0)
				//	selectedItem = 3;
				//if (selectedItem == 2)
				//selectedItem = 4;
				//selectedItem = masterController.testIncremento;
				//if(masterController.testIncremento != 3) masterController.testIncremento = (masterController.testIncremento + 1) % 5;
				mainGameController.tType = selectedItem;

				//if(selectedItem > 1)
				//					selectedItem = 1;
				gameController.selectedItem = selectedItem;
				gameController.networkAgent.broadcast ("wheelitem:" + selectedItem + ":");
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
