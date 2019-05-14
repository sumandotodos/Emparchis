using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VotationController_mono : Task {

	const int REDBOTTLE = 0;
	const int BLUEBOTTLE = 1;
	const int YELLOWBOTTLE = 2;

	public Text[] label;
	public Text[] labelShadow;

	public RosettaWrapper rosetta;
	public string textPrefix;

	public GameController_mono gameController;

	public GameObject bingoActivity;
	public BingoController_mono bingoController;

	public Texture[] playerIcons;
	public RawImage playerIconRawImage;

	public CircleDeploy[] judges;
	public CircleDeploy okButton;

	public UIFaderScript fader;

	public UIScaleFader helpPanelScaler;
	public UIScaleFader interrogationButton;

	public int whichPlayer;

	int state = 0;

	public int vote = -1;
	public int voteType = -1;
	public int blueVote = -1;
	public int greenVote = -1;
	public int yellowVote = -1;

	bool voted;

	//bool[] typeVoted;

	public const int NumberOfTypes = 3;

	public const int YELLOW = 0;
	public const int BLUE = 1;
	public const int GREEN = 2;
	public const int OTHER = 3;

	int nbottles;

	public void startVotationActivity(Task w, int player) {

		w.isWaitingForTaskToComplete = true;
		waiter = w;
		voted = false;
		whichPlayer = player;
		vote = -1;

		helpPanelScaler.scaleOutImmediately ();
		interrogationButton.scaleOutImmediately ();


		label [REDBOTTLE].text = rosetta.rosetta.retrieveString (textPrefix, REDBOTTLE);
		label [BLUEBOTTLE].text = rosetta.rosetta.retrieveString (textPrefix, BLUEBOTTLE);
		label [YELLOWBOTTLE].text = rosetta.rosetta.retrieveString (textPrefix, YELLOWBOTTLE);
//		labelShadow [REDBOTTLE].text = rosetta.rosetta.retrieveString (textPrefix, REDBOTTLE);
//		labelShadow [BLUEBOTTLE].text = rosetta.rosetta.retrieveString (textPrefix, BLUEBOTTLE);
//		labelShadow [YELLOWBOTTLE].text = rosetta.rosetta.retrieveString (textPrefix, YELLOWBOTTLE);


		playerIconRawImage.texture = playerIcons [player];
		for (int i = 0; i < judges.Length; ++i) {
			judges [i].reset ();
			okButton.reset ();
		}

		/*typeVoted = new bool[NumberOfTypes];
		for (int i = 0; i < NumberOfTypes; ++i)
			typeVoted [i] = false;
			*/

		fader.fadeIn ();
		interrogationButton.scaleIn ();

		state = 1;

	}

	// Use this for initialization
	void Start () {
		
	}

	public void receiveVote(int j, int type) { // 0: amarillo  1: azul   2: verde

		//typeVoted [type] = true;
		//voted = true;
		voteType = type;

		if (type == YELLOW)
			vote = j;
		if (type == BLUE)
			vote = j;
		if (type == GREEN)
			vote = j;
		// no impossible case treatment, sorry lads!

		bool raiseOKButton = true;
		//for (int i = 0; i < typeVoted.Length; ++i) {
		//	if (typeVoted [i] == false) {
		//		raiseOKButton = false;
		//		break;
		//	}
		//}

		if (raiseOKButton)
			extendBotonaco ();

	}
	
	// Update is called once per frame
	void Update () {
		if (state == 0) {
			return;
		}

		if (state == 1) {
			if (gameController.localPlayerN != whichPlayer) { // if is voting player...
				// can't vote for myself
				// extend judges
				for (int i = 0; i < judges.Length; ++i) {
					judges [i].extend ();
					judges [i].gameObject.GetComponent<UIScaleFader> ().scaleOutImmediately ();
				}
			}
			state = 2;
		}

		if (state == 2) { // waiting for all other players to vote
			if (gameController.currentTurnVotes >= gameController.nPlayers - 1) {
				gameController.updateVoteVariables (); // <- here it is....
				gameController.currentTurnVotes = 0;
				interrogationButton.scaleOut ();
				fader.fadeOutTask (this);
				state = 3;
			}
		}
		if (state == 3) {
			if (!isWaitingForTaskToComplete) { // when the fadeout has finished....

				// check if es un bingo!
				bool esunbingo = false;
				//if (gameController.localPlayerN != whichPlayer) {

					esunbingo = true;
					if ((gameController.currentVote.Count > 1)) {
						Vote basevote = gameController.currentVote [0];
						nbottles = basevote.amount;
						for (int i = 1; i < gameController.currentVote.Count; ++i) {
							if (!basevote.Equals (gameController.currentVote [i])) {
								esunbingo = false;
								break;
							}
						}
					} else
						esunbingo = false;

				//} else
				//	esunbingo = false;

				if (esunbingo)
					state = 4;
				else
					state = 6;

			}

		}

		if (state == 4) {
			bingoActivity.SetActive (true);
			bingoController.startBingo (this, nbottles, gameController.localPlayerN != whichPlayer);
			for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
				if ((i != whichPlayer) && (gameController.playerPresent[i])) {
					gameController.playerList [i].bottlesReceived [TicketScreenController_mono.ClearBottle] += nbottles;
				}
			}
			state = 5;
		}

		if (state == 5) {
			if(!isWaitingForTaskToComplete) {
				bingoActivity.SetActive (false);
				state = 6;
			}
		}

		if(state == 6) {

				// clear votelist
				gameController.currentVote = new List<Vote> ();
				state = 0;
				notifyFinishTask (); // end task

		}
	}

	public void extendBotonaco() {
		okButton.extend ();
	}

	// UI callbacks


	public void closeHelp() {

		helpPanelScaler.scaleOut ();
		interrogationButton.scaleIn ();

	}

	public void botonacoTouch() {
		if (voted)
			return;
		voted = true;
		gameController.currentTurnVotes++;

		//gameController.playerList [gameController.playerTurn].scoreReceived [gameController.localPlayerN] += vote;
		//gameController.playerList [gameController.localPlayerN].scoreGiven += vote;
		if (voteType == YELLOW) {
			//gameController.playerList [whichPlayer].yellowScoreReceived [gameController.localPlayerN] += vote;
			//gameController.playerList [gameController.localPlayerN].yellowScoreGiven += vote;
			//gameController.playerList [whichPlayer].yellowVotesReceived[gameController.localPlayerN]++;
			//gameController.playerList [gameController.localPlayerN].yellowVotesGiven++;

			gameController.playerList [gameController.localPlayerN].bottlesGiven [TicketScreenController_mono.YellowBottle] += vote;
			gameController.playerList [whichPlayer].bottlesReceived [TicketScreenController_mono.YellowBottle] += vote;

			gameController.yellowTurnMyValue = vote;
		}
		if (voteType == BLUE) {
			//gameController.playerList [whichPlayer].blueScoreReceived [gameController.localPlayerN] += vote;
			//gameController.playerList [gameController.localPlayerN].blueScoreGiven += vote;
			//gameController.playerList [whichPlayer].blueVotesReceived[gameController.localPlayerN]++;
			//gameController.playerList [gameController.localPlayerN].blueVotesGiven++;

			gameController.playerList [gameController.localPlayerN].bottlesGiven [TicketScreenController_mono.BlueBottle] += vote;
			gameController.playerList [whichPlayer].bottlesReceived [TicketScreenController_mono.BlueBottle] += vote;

			gameController.blueTurnMyValue = vote;
		}
		if (voteType == GREEN) {

			gameController.playerList [gameController.localPlayerN].bottlesGiven [TicketScreenController_mono.RedBottle] += vote;
			gameController.playerList [whichPlayer].bottlesReceived [TicketScreenController_mono.RedBottle] += vote;

			gameController.greenTurnMyValue = vote;
		}

		//gameController.turnMyValue = vote;

		gameController.currentVote.Add (new Vote (voteType, vote));
		for (int i = 0; i < judges.Length; ++i) {
			judges [i].gameObject.GetComponent<UIScaleFader> ().scaleOut ();
			judges [i].retract ();
		}

		okButton.retract ();
	}

}
