using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;



public class StatisticsController_mono : Task {

	public bool isWinner;

	public Score[] score;
	public int nOfPlayersWithMaxScore = 0;
	public string route = "";

	bool enhorabuenaDeployed = false;

	public FGTable msgTable;
	public Text enhorabuenaText;

	public const int HalfStarPoints = 4;

	public GameController_mono gameController;

	public Text totalScore; // circle

	public Text redScore, yellowScore, blueScore;

	public Text totalBottlesText;
	public Text totalClearBottlesText;
	public UIScaleFader enhorabuena;



	public RawImage scoreCirculito;


	public CircleDeploy laurel;

	public Color[] categoryColor;

	public Texture[] halfStarsTex;

	public Text mineScore;

	public UIFaderScript fader;

	int state = 0;
	float timer = 0.0f;
	const float delay = 0.5f;

	int state2 = 0; // slot that waits for all scores to be received
	float timer2 = 0.0f;

	int setScores = 0;

	void Start() {
		score = new Score[GameController_mono.MaxPlayers];
		for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			score [i] = new Score ();
		}
	}

	public void startStatisticsActivity(Task w) {

		isWinner = false;

		w.isWaitingForTaskToComplete = true;
		waiter = w;

		gameController.resetQuickSaveInfo (); // game finished!!!!

		enhorabuena.scaleOutImmediately ();



		laurel.reset ();

		calculateScores ();
		fader.fadeIn ();
		state = 1;
		state2 = 1;
	}

	private int uniqueFirst(int[] values) {
		
		int maxValue = 0;
		int maxIndex = 0;
		int nIndices = 0;


		for (int i = 0; i < values.Length; ++i) {
			if (values [i] > maxValue) {
				maxValue = values [i];
				maxIndex = i;
				nIndices = 1;
			} else if (values [i] == maxValue) {
				nIndices++;
			}
		}

		if (nIndices == 1)
			return maxIndex;
		else
			return -1;
			

	}

	private void secondValue(int[] values, out int val, out int nplayr) {

		int maxValue = 0;
		int maxIndex = 0;
		int nIndices = 0;

		for (int i = 0; i < values.Length; ++i) {
			if (values [i] > maxValue) {
				maxValue = values [i];
			} 
		}

		int absoluteMax = maxValue;
		maxValue = 0;

		for (int i = 0; i < values.Length; ++i) {
			if ((values[i] < absoluteMax) && (values [i] > maxValue)) {
				maxValue = values [i];
				maxIndex = i;
				nIndices = 1;
			} else if ((values[i] < absoluteMax) && (values [i] == maxValue)) {
				nIndices++;
			}
		}

		val = maxValue;
		nplayr = nIndices;

	}

	private int uniqueSecond(int[] values) {

		int maxValue = 0;
		int maxIndex = 0;
		int nIndices = 0;

		for (int i = 0; i < values.Length; ++i) {
			if (values [i] > maxValue) {
				maxValue = values [i];
			} 
		}

		int absoluteMax = maxValue;
		maxValue = 0;

		for (int i = 0; i < values.Length; ++i) {
			if ((values[i] < absoluteMax) && (values [i] > maxValue)) {
				maxValue = values [i];
				maxIndex = i;
				nIndices = 1;
			} else if ((values[i] < absoluteMax) && (values [i] == maxValue)) {
				nIndices++;
			}
		}

		if (nIndices == 1)
			return maxIndex;
		else
			return -1;

	}

	private void order(int[] values, int [] indices) {

		int maxValue = 50000;
		int candidateMaxValue = 0;
		int candidateMaxIndex = 0;
		int sorted = 0;
		for (int j = 0; j < indices.Length; ++j) {
			for (int i = 0; i < values.Length; ++i) {
				if ((values [i] > candidateMaxValue) && (values [i] <= maxValue)) {
					candidateMaxValue = values [i];
					indices [j] = i;
				}
			}
			maxValue = candidateMaxValue;
		}

	}

	public void hideEnhorabuena() {
		enhorabuena.scaleOut();
		enhorabuenaDeployed = false;
	}


	public void calculateScores() {



		int tS = 0;




		for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			if (!gameController.playerPresent [i]) {
				for (int j = 0; j < TicketScreenController_mono.MaxColors; ++j) {
				
					gameController.playerList [i].bottlesGiven [j] = -1;
				
				}
			}
		}
	

	


//		int totalBottles = 0;
//		for(int i = 0; i < TicketScreenController.MaxColors-1; ++i) {
//			int nOfBottles = gameController.playerList [gameController.localPlayerN].bottlesReceived [i];
//			colorBottlesText [i].text = "x " + nOfBottles;
//			totalBottles += nOfBottles;
//		}
//
//		totalBottlesText.text = "" + totalBottles;



		int[] receivedBottles = new int[GameController_mono.MaxPlayers];
		int[] receivedBottlesOrder = new int[GameController_mono.MaxPlayers];
		for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			receivedBottles [i] = gameController.playerList [gameController.localPlayerN].bottlesReceived [i];
		}

		int[] givenBottles = new int[GameController_mono.MaxPlayers];
		int[] givenBottlesOrder = new int[GameController_mono.MaxPlayers];
		for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			givenBottles [i] = 0;
			for (int j = 0; j < TicketScreenController_mono.MaxColors; ++j) {
				givenBottles [i] += gameController.playerList [i].bottlesGiven [j];
			}
		}
			
		int maxBottlesColor = uniqueFirst (receivedBottles);
//		if (maxBottlesColor != -1) {
//			scoreCirculito.color = categoryColor [maxBottlesColor];
//		}

//		int secondPlayer = uniqueSecond (givenBottles);
//		if (secondPlayer != -1) {
//			gameController.playerList [secondPlayer].bottlesReceived [TicketScreenController.ClearBottle] += 7;
//			if (secondPlayer == gameController.localPlayerN) {
//				enhorabuena.scaleIn ();
//				enhorabuenaDeployed = true;
//			}
//		}
		int secondGiveScore;
		int secondNPlayers;
		secondValue(givenBottles, out secondGiveScore, out secondNPlayers);
		if (secondNPlayers > 0) {
			if (secondNPlayers == 1) {
				enhorabuenaText.text = (string)msgTable.getElement (0, 0);
			} else if (secondNPlayers == 2) {
				enhorabuenaText.text = (string)msgTable.getElement (0, 1);
			} else if (secondNPlayers == 3) {
				enhorabuenaText.text = (string)msgTable.getElement (0, 2);
			}
			for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
				if (gameController.playerPresent [i] && givenBottles [i] == secondGiveScore) {
					gameController.playerList [i].bottlesReceived [TicketScreenController_mono.ClearBottle] += (6/secondNPlayers);
					if (i == gameController.localPlayerN) {
						enhorabuena.scaleIn ();
						enhorabuenaDeployed = true;
					}
				}
			}
		}

		int totalRedBottles = gameController.playerList [gameController.localPlayerN].bottlesReceived [TicketScreenController_mono.RedBottle];
		int totalYellowBottles = gameController.playerList [gameController.localPlayerN].bottlesReceived [TicketScreenController_mono.YellowBottle];
		int totalBlueBottles = gameController.playerList [gameController.localPlayerN].bottlesReceived [TicketScreenController_mono.BlueBottle];
		int totalClearBottles = gameController.playerList [gameController.localPlayerN].bottlesReceived [TicketScreenController_mono.ClearBottle];

		redScore.text = "" + totalRedBottles;
		yellowScore.text = "" + totalYellowBottles;
		blueScore.text = "" + totalBlueBottles;

		int totalBottles = totalRedBottles + totalYellowBottles + totalBlueBottles;

		totalClearBottlesText.text = "" + totalClearBottles;


		int totalTotalBottles = totalClearBottles + totalBottles;

		tS += totalTotalBottles;


		// tell myself
		setScore(gameController.localPlayerN, totalTotalBottles);
		

		this.totalScore.text = "" + tS;



	}

	// network callback
	public void setScore(int player, int points) {
		++setScores;

		score [player].pointsScore = points;


		score [player].total = points;
	}
		

	public void decideWinner(bool secondPass) {

		int maxScoreIndex = 0;
		int maxScore = 0;


		if (secondPass) {
			maxScore = score [0].pointsScore;
		} else {
			maxScore = score [0].total;
		}

		// pass 1: find out max score
		for (int i = 1; i < GameController_mono.MaxPlayers; ++i) {
			if (secondPass) {
				if (score [i].pointsScore > maxScore) {
					maxScore = score [i].pointsScore;
					maxScoreIndex = i;
				}
			} else {
				if (score [i].total > maxScore) {
					maxScore = score [i].total;
					maxScoreIndex = i;
				}
			}

		}

		// pass 2: find out how many players have this max score
		nOfPlayersWithMaxScore = 0;
		for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			if (secondPass) {
				if (score [i].pointsScore == maxScore) {
					++nOfPlayersWithMaxScore;
				}
			} else {
				if (score [i].total == maxScore) {
					++nOfPlayersWithMaxScore;
				}
			}
		}

		if (nOfPlayersWithMaxScore == 1) { // phew! usual case
			if (secondPass) {
				if (score [gameController.localPlayerN].pointsScore == maxScore) {
					laurel.extend (); // show our pride!!!
					isWinner = true;
					Handheld.Vibrate();
					route = "single player second pass";
				}
			} else {
				if (score [gameController.localPlayerN].total == maxScore) {
					laurel.extend (); // show our pride!!!
					isWinner = true;
					route = "single player first pass";
					Handheld.Vibrate();
				}
			}
		} else {
			// prioritize score components over others

			if (!secondPass) {
				decideWinner (true);
			} else { // give up, tied players
				if (score[gameController.localPlayerN].pointsScore == maxScore) {
					laurel.extend (); // show our pride!!!
					isWinner = true;
					Handheld.Vibrate();
					route = "several players, second pass";
				}
			}

		}


	}
	
	// Update is called once per frame
	void Update () {


		// slot 2
		if (state2 == 0) {
			// idling
		}
		if (state2 == 1) { // waiting for nPlayers score reports
			if (setScores >= gameController.nPlayers) {
				state2 = 2;
			}
		}
		if (state2 == 2) {
			decideWinner (false);
			setScores = 0;
			state2 = 0; // stop this slot
		}

	
		if (state == 0) {	} // idling

		if (state == 1) {
			timer += Time.deltaTime;
			if (timer > delay) {
				//own.go ();
				timer = 0.0f;
				state = 2;
			}
		}

		if (state == 2) {
			timer += Time.deltaTime;
			if (timer > delay) {
				//others1.go ();
				timer = 0.0f;
				state = 10;
				if (gameController.nPlayers > 2)
					state = 3;
			}
		}

		if (state == 3) {
			
			timer += Time.deltaTime;
			if (timer > delay) {
				//others2.go ();
				timer = 0.0f;
				state = 10;
				if (gameController.nPlayers > 3)
					state = 4;
			}
		}

		if (state == 4) {
			timer += Time.deltaTime;
			if (timer > delay) {
				//others3.go ();
				timer = 0.0f;
				state = 10;
			}
		}

		if (state == 10) {
			/*if (Input.GetMouseButtonDown (0) && (enhorabuenaDeployed == false)) {
				fader.fadeOutTask (this);
				state = 11;
			}*/
		}

		if (state == 11) {
			if (!isWaitingForTaskToComplete) {
				notifyFinishTask ();
			}
		}

	}

	public void finishButtonPress() {
		if (enhorabuenaDeployed == false) {
			fader.fadeOutTask (this);
			state = 11;
		}
	}
}
