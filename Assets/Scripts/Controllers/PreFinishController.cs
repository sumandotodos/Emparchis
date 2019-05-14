using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreFinishController : Task, ButtonPressListener {

	// the GameHost will have centralized control over all this shit

	public GameController gameController;

	const int MaxShoesPerPlayer = 4;

	public Text nShoesText;
	public Text nHitosText;

	int finishPlayer = -1;

	const int MaxHitos = 28;

	public CircleDeploy okButtonDeploy;

	int nShoes = 0;
	bool OKButtonEnabled = false;

	public UIFaderScript fader;

	int nHitos = 0;

	int state = 0;

	public int nReports = 0;

	// Use this for initialization
	void Start () {
		nShoes = 0;
		finishPlayer = -1;
		OKButtonEnabled = false;
		nShoesText.text = "" + nShoes;
		nHitosText.text = "" + nHitos;
		state = 0;
	}

	public void startPreFinishActivity(Task w) {
		nReports = 0;
		w.isWaitingForTaskToComplete = true;
		waiter = w;
		nShoes = 0;
		finishPlayer = -1;
		OKButtonEnabled = false;
		okButtonDeploy.reset ();
		okButtonDeploy.retract ();
		nHitos = 0;
		fader.Start ();
		fader.fadeIn ();
		state = 0;
		if (gameController.isMaster) {
			checkOK ();
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (state == 0) {
			return;
		}

		if (state == 1) { // waiting for fadeout
			if (!isWaitingForTaskToComplete) {
				state = 0;
				notifyFinishTask ();
			}
		}

	}

	private void checkOK() { // only the master checks OK
		// the OK button is enabled and shown when all
		//  players agree on who reached the goal first, 
		//  and the total number of shoes is conserved

		bool okOK = true;

		int totalShoes = 0;

		int finishPlayerVotes = 0;
		int finishPlayer = -1;

		bool firstToGoalSet = false;
		for(int i = 0; i < GameController.MaxPlayers; ++i) {
			if (gameController.playerList [i].firstToGoal != -1) {
				firstToGoalSet = true;
			}
		}

		for (int i = 0; i < GameController.MaxPlayers; ++i) {

			if (gameController.playerPresent [i]) {
				totalShoes += gameController.playerList [i].shoes;

				if (firstToGoalSet) {

					if (gameController.playerList [i].firstToGoal == -1) {
						okOK = false;
						break;
					} else {
						if (finishPlayer == -1) {
							finishPlayer = gameController.playerList [i].firstToGoal;
						} else {
							if (finishPlayer != gameController.playerList [i].firstToGoal) {
								okOK = false;
								break;
							}
						}
					}
				}

			}
			
		}

		if ((totalShoes != (gameController.nPlayers * MaxShoesPerPlayer)) && (totalShoes != 0)) {
			okOK = false;
		}


		if (okOK) {
			OKButtonEnabled = true;
			okButtonDeploy.extend ();
		} else {
			OKButtonEnabled = false;
			okButtonDeploy.retract ();
		}

	}

	private void updatePlayers() {
		if (gameController.isMaster) {
			setFinishPlayer (gameController.localPlayerN, finishPlayer);

		} else {
			gameController.networkAgent.sendCommand (0, "setfinishplayer:" +
				gameController.localPlayerN + ":" + finishPlayer + ":");	
		}

	}

	private void updateShoes() {
		if (gameController.isMaster) {
			// update shoe amount
			setShoes(gameController.localPlayerN, nShoes);

		} else {
			// tell master about your shoe amount
			gameController.networkAgent.sendCommand (0, "setshoes:" +
				gameController.localPlayerN + ":" + nShoes + ":");
		}
	}

	private void updateHitos() {
		if (gameController.isMaster) {
			// update shoe amount
			setHitos(gameController.localPlayerN, nHitos);

		} else {
			// tell master about your shoe amount
			gameController.networkAgent.sendCommand (0, "sethitos:" +
				gameController.localPlayerN + ":" + nHitos + ":");
		}
	}


	public void plusButton() {
		if (nShoes < gameController.nPlayers * MaxShoesPerPlayer) {
			++nShoes;
			nShoesText.text = "" + nShoes;
		}

		updateShoes ();

	}

	public void minusButton() {
		if (nShoes > 0) {
			--nShoes;
			nShoesText.text = "" + nShoes;
		}

		updateShoes ();

	}

	public void hitoPlusButton() {
		if (nHitos < MaxHitos) {
			++nHitos;
			nHitosText.text = "" + nHitos;
		}

		updateHitos ();
	}

	public void hitoMinusButton() {
		if (nHitos > 0) {
			--nHitos;
			nHitosText.text = "" + nHitos;
		}

		updateHitos ();
	}





	// ui callbacks
	public void touchPlayer1() {
		finishPlayer = 0;
		updatePlayers();
	}

	public void touchPlayer2() {
		finishPlayer = 1;
		updatePlayers();
	}

	public void touchPlayer3() {
		finishPlayer = 2;
		updatePlayers();
	}

	public void touchPlayer4() {
		finishPlayer = 3;
		updatePlayers();
	}

	public void buttonPress() {
		
		if (OKButtonEnabled == false)
			return;

		if (gameController.isMaster) {
			fader.fadeOutTask (this);
			state = 1;
			OKButtonEnabled = false;
			okButtonDeploy.retract ();
			for (int k = 0; k < GameController.MaxPlayers ; ++k) {
				if (gameController.playerPresent[k] && k != gameController.localPlayerN) {
					gameController.networkAgent.sendCommand (gameController.playerList [k].id, "prefinishdone:" +
					k + ":" +
					gameController.playerList [k].shoes + ":" +
					gameController.playerList [k].firstToGoal + ":" +
					gameController.playerList [k].hitos + ":");
				}
			}
			gameController.playerList [gameController.localPlayerN].hitos = nHitos;
		}

	}


	// network callbacks

	public void setShoes(int player, int shoes) {
		gameController.playerList [player].shoes = shoes;
		checkOK ();
	}

	public void setHitos(int player, int hitos) {
		gameController.playerList [player].hitos = hitos;
		checkOK ();
	}

	public void setFinishPlayer(int player, int fPlayer) {
		gameController.playerList [player].firstToGoal = fPlayer;
		checkOK ();
	}

	public void preFinishDone(int whichPlayer, int nshoes, int fplayer, int nhitos) {
		Debug.Log ("preFinishDone actually executed!!!");
		gameController.playerList [whichPlayer].shoes = nshoes;
		gameController.playerList [whichPlayer].firstToGoal = fplayer;
		gameController.playerList [whichPlayer].hitos = nhitos;
		//++nReports;
		//if (nReports == (gameController.nPlayers - 1)) {
			fader.fadeOutTask (this);
			state = 1;
		//}
	}
}
