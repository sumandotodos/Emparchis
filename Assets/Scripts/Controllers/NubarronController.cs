using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NubarronController : Task {

	public int state;

	public GameObject centerNube;
	public GameObject rightNube;
	public GameObject topNube;
	public GameObject leftNube;
	GameObject objectToDisable;
	public GameObject luna;

	public GameController gameController;

	public AudioClip flashEffectSound_N;

	public UIFaderScript fader;

	public UIFlashEffect flashEffect;

	int receivedVotes;
	float receivedScore;

	float timer;
	const float MaxTime = 6.0f;

	//int touchedClouds;
	int cloudsNeeded;

	// type = 0   ->   single cloud
	// type = 1   ->   triple cloud

	bool nube0Touched = false;
	bool nube1Touched = false;
	bool nube2Touched = false;
	bool nube3Touched = false;

	int tType;

	public void startNubeActivity(Task w, int type) {
		
		w.isWaitingForTaskToComplete = true;
		waiter = w;
		//touchedClouds = 0;
		if (type == 0 || type == 1) {
			cloudsNeeded = 1;
			rightNube.SetActive (false);
			topNube.SetActive (false);
			leftNube.SetActive (false);
			centerNube.SetActive (true);
		}
		else if (type == 2) {
//			cloudsNeeded = 3;
//			rightNube.SetActive (true);
//			topNube.SetActive (true);
//			leftNube.SetActive (true);
//			centerNube.SetActive (false);
			cloudsNeeded = 1;
			rightNube.SetActive (false);
			topNube.SetActive (false);
			leftNube.SetActive (false);
			centerNube.SetActive (true);
		}
		luna.SetActive (false);
		nube0Touched = false;
		nube1Touched = false;
		nube2Touched = false;
		nube3Touched = false;
		receivedScore = 0.0f;
		fader.fadeIn ();
		tType = type;
	}

	// Use this for initialization
	void Start () {
		state = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (state == 0) {
			return;
		}

		if (state == 1) { // waiting for flash to expand... (center nube)
			if (!isWaitingForTaskToComplete) {
				objectToDisable.SetActive (false);
				flashEffect.contract ();


				--cloudsNeeded;
				if (cloudsNeeded == 0) {
					luna.SetActive (true);
					if ((1 > 2) && (tType == 0)) {
						// if we have to vote
						if (gameController.situationChooser.isNegative) {
							state = 2;
						}
						else {
							state = 6;
							timer = 0.0f;
						}
					}
					else {
						state = 6;
						timer = 0.0f;
					}						
				}
				else state = 0;
			}
		}

		if (state == 2) { // showing moon, waiting for other players to vote............ (test 0)
			if (gameController.currentTurnVotes == gameController.nPlayers - 1) {
				waiter.fReturnValue = (gameController.test0TurnValue / ((float)gameController.currentTurnVotes));
				gameController.updateVoteVariables ();
				fader.fadeOutTask (this);
				state = 3;
			}
		}

		if (state == 3) { // wait for fadeout
			if (!isWaitingForTaskToComplete) {
				notifyFinishTask ();
				state = 0;
			}
		}

		if (state == 6) {
			timer += Time.deltaTime;
			if (Input.GetMouseButtonDown (0))
				timer += MaxTime;
			if (timer > MaxTime) {
				fader.fadeOutTask (this);
				//gameController.networkAgent.broadcast ("finishnotmyroulette:"); // immediately finish notmyroulette
				state = 3;
			}
		}


	}

	// ui callbacks

	public void touchNube(int id) {

		flashEffect.expandTask (this);
		state = 1;

	}

	public void touchNube0() {
		if (nube0Touched)
			return;
		objectToDisable = centerNube;
		touchNube (0);
		nube0Touched = true;
	}

	public void touchNube1() {
		if (nube1Touched)
			return;
		objectToDisable = topNube;
		touchNube (1);
		nube1Touched = true;
	}

	public void touchNube2() {
		if (nube2Touched)
			return;
		objectToDisable = leftNube;
		touchNube (2);
		nube2Touched = true;
	}

	public void touchNube3() {
		if (nube3Touched)
			return;
		objectToDisable = rightNube;
		touchNube (3);
		nube3Touched = true;
	}
}
