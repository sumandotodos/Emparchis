using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainGameController_mono : Task {

	[HideInInspector]
	public int tType;

	public AudioClip ticketSound_N;

	float syncCanvasDelayTimer = 0.0f;




	public MasterController_mono masterController;

	public GameController_mono gameController;
	public GameObject rouletteActivity;
	public GameObject notMyRouletteActivity;
	public GameObject turnScreen;
	public GameObject type1Test;
	public GameObject type2Test;
	public GameObject type3Test;
	public GameObject type4Test;
	public GameObject type5Test;

	public GameObject statisticsActivity;

	public GameObject turnScreenCanvas;

	public GameObject preFinishActivity;

	public GameObject testActivity;
	public GameObject maskActivity;
	public GameObject votationActivity;

	public GameObject endVotationPanel;

	public UIHighlight ticketHighlight;

	public RouletteController_mono rouletteController;
	public NotMyRouletteController_mono notMyRouletteController;
	public StatisticsController_mono statisticsController;
	public PreFinishController_mono preFinishController;
	public VotationController_mono votationController;

	public TicketScreenController_mono ticketScreenController;

	public UIHighlight tickButton;
	public UIHighlight batsuButton;

	public UIFaderScript turnFader;

	public CommonTestController_mono commonTestController;

	public EmocionarioController_mono emocionarioController;

	int testType = -1;

	int state = 0;

	bool voted = false;

	public int voteForEndCounter = 0;

	int temp;

	//[HideInInspector]
	public int rouletteResult = -1;

	bool canChooseIcon = true; // to prevent selecting something twice

	public void startMainGameActivity(Task w) {
		w.isWaitingForTaskToComplete = true;
		waiter = w;
		endVotationPanel.SetActive (false);
		voteForEndCounter = 0;
		gameController.saveQuickSaveData ();
		voted = false;
		state = 1;
	}

	// Use this for initialization
	void Start () {
	
	}


	// Update is called once per frame
	void Update () {
	
		if (state == 0) {   }  // idling

		if (state == 1) { // initializing turn screen
			turnScreen.SetActive (true);
			ticketScreenController.updateBottles ();

			turnFader.fadeIn ();
			canChooseIcon = true;
			state = 2;
			rouletteResult = -1; // reset rouletteResult
		}

		if (state == 2) { // waiting for user input (click on ticket or click on the end)

		}

		if (state == 3) { // waiting for fadeout to finish, proceed to turn
			if (!isWaitingForTaskToComplete) {
				state = 5;
				ticketHighlight.unpress ();
				turnScreen.SetActive (false);
			}
		}


		if (state == 4) { // waiting for fadeout to finish, proceed to game end
			if (!isWaitingForTaskToComplete) {
				//state = 30; // wait for statistics activity to finish
				state = 80;
				turnScreen.SetActive (false);
		

				statisticsActivity.SetActive (true);
				statisticsController.startStatisticsActivity (this);
			}
		}

		if (state == 5) {
			if (gameController.localPlayerN == gameController.playerTurn) {

				// my turn...
				rouletteActivity.SetActive (true);
				notMyRouletteActivity.SetActive (false);
				rouletteController.startRouletteActivity (this);

				state = 10; // wait for roulette result

			} else {

				// not my turn, doh!
				rouletteActivity.SetActive (false);
				notMyRouletteActivity.SetActive (true);
				notMyRouletteController.startNotMyRoulette (this);

				state = 20; // wait for notmyroulette results

			}
		} else if (state == 10) { // waiting for roulette result
			if (rouletteResult != -1) {
				rouletteActivity.SetActive (false);
				commonTestController.startCommonTestActivity (this, rouletteResult);
				state = 11;
			}
		} else if (state == 11) { // waiting for test to finish
			if (!isWaitingForTaskToComplete) {
				gameController.synchCanvas.SetActive (true);

				state = 12;
			}
		} else if (state == 12) {


				gameController.synchCanvas.SetActive (false);
				
				state = 200;
				

		} else if (state == 20) { // waitinf for notMyRoulette result
			if (!isWaitingForTaskToComplete) { 
				// proceed to next turn

				notMyRouletteActivity.SetActive (false);

				// we have to sync here!!!

				gameController.synchCanvas.SetActive (true);
				state = 21;
			}
		} else if (state == 21) {


				gameController.synchCanvas.SetActive (false);
				state = 200;
				


		} else if (state == 30) { // waiting for statistics to end. Proceed to reset game
			if (!isWaitingForTaskToComplete) {
				state = 0; // stop this object
				statisticsActivity.SetActive (false);
				masterController.startActivity = "ResetGame";

				// unlock a gift
				if (gameController.obtainedGifts.Count < GalleryController_mono.maxGifts) {
					int r = Random.Range (0, GalleryController_mono.maxGifts);
					while (gameController.obtainedGifts.Contains (r)) {
						r = (r + 1) % GalleryController_mono.maxGifts;
					}
					gameController.obtainedGifts.Add (r);
					gameController.saveMoarData ();
				}

				if (statisticsController.isWinner) {
					// unlock another gift
					if (gameController.obtainedGifts.Count < GalleryController_mono.maxGifts) {
						int r = Random.Range (0, GalleryController_mono.maxGifts);
						while (gameController.obtainedGifts.Contains (r)) {
							r = (r + 1) % GalleryController_mono.maxGifts;
						}
						gameController.obtainedGifts.Add (r);
						gameController.saveMoarData ();
					}
				}

				notifyFinishTask (); // return to mastercontroller and reset the game
			}
		} else if (state == 80) { // waiting for prefinish to finish
			state = 30;

		} 

		// start votation activity on all present players
		else if (state == 200) { 

			temp = 0;
			state = 201;
			votationActivity.SetActive (true);

		} else if (state == 201) {
			

			if (gameController.situationChooser.isNegative) {
				votationController.startVotationActivity (this, gameController.playerTurn);
				state = 202;
			} else {
				votationActivity.SetActive (false);
				gameController.nextTurn ();
				state = 1; // loop back
			}


		} else if (state == 202) {
			if (!isWaitingForTaskToComplete) {
				
				state = 203;
			}
		} else if (state == 203) {

			votationActivity.SetActive (false);
			gameController.resetTimesPlayed ();
			gameController.numberOfVotations++;
			gameController.nextTurn ();
			state = 1; // loop back

		} else if (state == 800) { // end votation

			if (voteForEndCounter == gameController.nPlayers) {
				turnFader.fadeOutTask (this);
				state = 4;
			}


		} 

		//


		else if (state == 5000) { // waiting for sync before restarting turn

			if (syncCanvasDelayTimer > 0.0f) {
				syncCanvasDelayTimer -= Time.deltaTime;
				if (syncCanvasDelayTimer <= 0.0f) {
					commonTestController.syncCanvas.SetActive (true);
				}
			}
			if (gameController.synchNumber >= gameController.nPlayers - 1) {
				gameController.synchNumber = 0;
				commonTestController.syncCanvas.SetActive (false);
				state = 1;
			}
		}

		else if (state == 6000) { // waiting for sync after hitting ticket

			if (syncCanvasDelayTimer > 0.0f) {
				syncCanvasDelayTimer -= Time.deltaTime;
				if (syncCanvasDelayTimer <= 0.0f) {
					commonTestController.syncCanvas.SetActive (true);
				}
			}
			if (gameController.synchNumber >= gameController.nPlayers - 1) {
				gameController.synchNumber = 0;
				commonTestController.syncCanvas.SetActive (false);
				state = 3;
			}
		}

		else if(state == 9000) {
			if(!isWaitingForTaskToComplete) {
				if (gameController.obtainedGifts.Count < GalleryController_mono.maxGifts) {
					int r = Random.Range (0, GalleryController_mono.maxGifts);
					while (gameController.obtainedGifts.Contains (r)) {
						r = (r + 1) % GalleryController_mono.maxGifts;
					}
					gameController.obtainedGifts.Add (r);
					gameController.saveMoarData ();
				}
				masterController.hardReset ();
				state = 0;
			}
		}


	}




	// event callbacks
	public void clickOnTicket() {

		if (canChooseIcon == false)
			return;

		canChooseIcon = false;

		AudioManager.getInstance ().playSound (ticketSound_N);

		setTurn (0);


	}

	public void clickOnEmotions() {

		emocionarioController.startEmocionario();

	}


	public void clickOnEnd() {

		if (canChooseIcon == false)
			return;

		canChooseIcon = false;

		showEndVotationPanel ();

	}


	public void clickOnTick() {

		turnFader.fadeOutTask (this);
		state = 9000;

	}

	public void clickOnBatsu() {

	
		hideEndVotationPanel ();

	}


	// network callbacks

	public void waitForVotation() {
		state = 800;
	}

	public void cancelVotation() {
		voted = false;
		clickOnBatsu ();
	}

	public void supportVotation() {
		++voteForEndCounter;
	}

	public void ticketACK() {

	}

	public void ticketNACK() {
		// disable the ticket
		ticketHighlight.unpress();
		ticketHighlight.disable ();
	}

	public void setTurn(int pl) {
		gameController.playerTurn = pl;
		turnFader.fadeOutTask (this);

		syncCanvasDelayTimer = 2.0f;
		state = 6000; // wait for fadeout
	}

	public void end() {
		turnFader.fadeOutTask (this);
		state = 4; // wait for fadeout
	}

	public void showEndVotationPanel() {
		voted = false;
		endVotationPanel.SetActive (true);
	}

	public void hideEndVotationPanel() {
		batsuButton.setEnable (true);
		tickButton.setEnable (true);
		batsuButton.unpress ();
		tickButton.unpress ();
		gameController.playerRequestedEnd = -1;
		voted = false;
		canChooseIcon = true;
		voteForEndCounter = 0;
		endVotationPanel.SetActive (false);
	}
		
	// called as a response to network command "negativesituation"
	//   disables NotMyRouletteActivity and starts NegativeSituationActivity
	public void startAllPlaySituation(int table, int row) {

		// we cancel notMyRouletteActivity task and start a new task

		notMyRouletteActivity.SetActive(false);
		notMyRouletteController.notifyFinishTask ();
		commonTestController.sitTable = table;
		commonTestController.sitRow = row;
		commonTestController.startCommonTestActivity(this, GameController_mono.AllPlaySituation);


	}

	public void restartTurn() {
		gameController.loadQuickSaveData ();
		notMyRouletteController.cancel ();
		rouletteController.cancel ();
		notMyRouletteActivity.SetActive (false);
		rouletteActivity.SetActive (false);
		turnFader.setFadeValue (1.0f);
		notMyRouletteController.fader.Start ();
		notMyRouletteController.fader.setFadeValue (1.0f);
		rouletteController.fader.Start ();
		rouletteController.fader.setFadeValue (1.0f);
		syncCanvasDelayTimer = 2.0f;
		state = 5000;


	}

	public void broadcastRestartTurn() {
		restartTurn ();
	}

}
