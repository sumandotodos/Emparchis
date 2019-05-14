using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.Networking;

[System.Serializable]
public class Player  {


	/* data that characterizes the player */

	public int color; // 0 to 3
	public int score;
	int sessId; // id for current session
	public int endGameVote, resetGameVote, dismissPlayerVote;
	public string login;
	public int id;
	public int nVotesGiven;
	public int blueVotesGiven;
	public int greenVotesGiven;
	public int yellowVotesGiven;

	public List<int> nVotesReceived;
	public List<int> blueVotesReceived;
	public List<int> greenVotesReceived;
	public List<int> yellowVotesReceived;

	public List<float> scoreReceived; // accumulation of score received by others;
	public List<float> blueScoreReceived;
	public List<float> greenScoreReceived;
	public List<float> yellowScoreReceived;

	public List<int> bottlesReceived; // list of colors
	public List<int> bottlesGiven;
	//public int totalBottlesGiven;

	public float scoreGiven; // accumulation of score given to others
	public float blueScoreGiven;
	public float greenScoreGiven;
	public float yellowScoreGiven;

	public float disperssion; // accumulation of disperssion
	public float blueDisperssion;
	public float greenDisperssion;
	public float yellowDisperssion;

	public int firstToGoal;

	public int shoes;
	public int hitos;

	public void reset() {
		
		endGameVote = resetGameVote = dismissPlayerVote = 0;
		score = 0;
		hitos = 0;
		disperssion = 0.0f;
		blueDisperssion = 0.0f;
		greenDisperssion = 0.0f;
		yellowDisperssion = 0.0f;

		scoreGiven = 0.0f;

		blueScoreGiven = 0.0f;
		greenScoreGiven = 0.0f;
		yellowScoreGiven = 0.0f;

		scoreReceived = new List<float> ();
		blueScoreReceived = new List<float> ();
		greenScoreReceived = new List<float> ();
		yellowScoreReceived = new List<float> ();

		bottlesReceived = new List<int> ();
		bottlesGiven = new List<int> ();
		for (int i = 0; i < TicketScreenController.MaxColors; ++i) {
			bottlesReceived.Add (0);
			bottlesGiven.Add (0);
		}

		nVotesReceived = new List<int> ();
		blueVotesReceived = new List<int> ();
		greenVotesReceived = new List<int> ();
		yellowVotesReceived = new List<int> ();

		for (int i = 0; i < GameController.MaxPlayers; ++i) {

			scoreReceived.Add (0.0f);
			nVotesReceived.Add (0);

			blueScoreReceived.Add (0.0f);
			blueVotesReceived.Add (0);
			greenScoreReceived.Add (0.0f);
			greenVotesReceived.Add (0);
			yellowScoreReceived.Add (0.0f);
			yellowVotesReceived.Add (0);

		}

		firstToGoal = -1;
		shoes = 0;

	}

	public int totalScore() {
		int res = 0;
		for (int i = 0; i < bottlesReceived.Count; ++i) {
			res += bottlesReceived [i];
		}
		return res;
	}

	public Player() {

		reset ();
		
	}
	

}
