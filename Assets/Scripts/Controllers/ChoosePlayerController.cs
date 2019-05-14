using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChoosePlayerController : Task, ButtonPressListener {


	public MasterController masterController;

	public GameController gameController;

	public AudioClip okSound_N;

	public UIHighlight[] choosablePlayer;

	public Texture[] enabledTexture;
	public Texture[] disabledTexture;


	public UIFaderScript fader;

	public int chosenPlayer = -1;

	List<int> canIClaimPlayer;

	public bool buttonLock = false;

	public int state = 0;
	float timer = 0.0f;

	public void buttonPress() {

		if (buttonLock)
			return;
		buttonLock = true;

		if (chosenPlayer == -1)
			return;
		gameController.networkAgent.sendCommand (0, "claim:" + gameController.getUserLogin() + ":" + chosenPlayer + ":");
		state = 3; // wait for network response
	}

	public void startChoosePlayerActivity(Task w) {

		w.isWaitingForTaskToComplete = true;
		waiter = w;

		state = 1;
		timer = 0.0f;

		chosenPlayer = -1;

		canIClaimPlayer = new List<int> ();
		for (int i = 0; i < GameController.MaxPlayers; ++i) {
			canIClaimPlayer.Add (-1);
		}

		for (int i = 0; i < choosablePlayer.Length; ++i) {
			choosablePlayer [i].Start ();
			choosablePlayer [i].GetComponent<RawImage> ().texture = enabledTexture [i];
			choosablePlayer [i].setEnable (true);
			choosablePlayer [i].unpress ();
		}

		fader.fadeIn ();

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (state == 0) {

		}

		if (state == 1) { // waiting for user input

		}

		if (state == 2) { // waiting for fadeout to finish
			if (!isWaitingForTaskToComplete) {
				gameController.networkAgent.broadcast ("synch:"); // start a synch
				gameController.synchCanvas.SetActive(true);
				state = 4; // synch players
			}
		}

		if (state == 3) { // waiting for network confirmation

		}

		if(state == 4) { // synch players
			if(gameController.synchNumber >= (gameController.nPlayers - 1)) {
				gameController.synchCanvas.SetActive(false);
				gameController.synchNumber = 0;
				state = 0; // idle
				masterController.startActivity = "MainGame";
				notifyFinishTask(); // return to parent task
			}
		}

	}

	public void touchPlayer1() {
		chosenPlayer = 0;
	}

	public void touchPlayer2() {
		chosenPlayer = 1;
	}

	public void touchPlayer3() {
		chosenPlayer = 2;
	}

	public void touchPlayer4() {
		chosenPlayer = 3;
	}

	public void touchButton() {




	}

	// network callback
	public void disablePlayer(int p, int owner) {

		if (p < 0)
			return;
		if (p >= choosablePlayer.Length)
			return;

		if (gameController.networkAgent.id == owner)
			return;
		if (p == chosenPlayer) {
			choosablePlayer [p].unpress ();
			chosenPlayer = -1;
		}
		choosablePlayer [p].GetComponent<RawImage> ().texture = disabledTexture [p];
		choosablePlayer [p].disable ();

	}

	// claimPlayerACK is only executed by the slave!!
	public void claimPlayerACK() {
		gameController.localPlayerN = chosenPlayer;
		fader.fadeOutTask (this);
		state = 2;
	}

	public void claimPlayerNACK() {
		disablePlayer (chosenPlayer, -1); // can't touch this!!
		chosenPlayer = -1;
		buttonLock = false;
		state = 1;
	}


	// claimPlayer is only executed by the Master!!
	public void claimPlayer(int claimerId, int pl) {
		if (canIClaimPlayer [pl].Equals ("")) {
			canIClaimPlayer [pl] = claimerId;
			// claim OK: send confirmation message
			gameController.playerPresent[pl] = true;
			gameController.playerList [pl].id = claimerId;
			gameController.networkAgent.broadcast ("setplayerpresent:" + pl + ":" + claimerId + ":");
			gameController.networkAgent.broadcast ("disableplayer:"+pl+":"+claimerId+":");
			gameController.networkAgent.sendCommand(claimerId, "claimplayerACK:");
			disablePlayer (pl, claimerId);

		} else {
			// claim denied: send unconfirmation message
			gameController.networkAgent.sendCommand(claimerId, "claimplayerNACK:");
		}
	}
}
