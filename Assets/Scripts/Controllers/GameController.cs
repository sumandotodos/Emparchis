using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Family { daughter, father, son, mother, parents, siblings };
public enum Mood { happy, sad, neutral, assertive, thoughtful }; 

public class MoodInteger {

	public const int happy = 0;
	public const int sad = 1;
	public const int neutral = 2;
	public const int assertive = 3;
	public const int thoughtful = 4;

	public static int enumToInt(Mood m) {
		if (m == Mood.happy)
			return happy;
		if (m == Mood.sad)
			return sad;
		if (m == Mood.neutral)
			return neutral;
		if (m == Mood.assertive)
			return assertive;
		if (m == Mood.thoughtful)
			return thoughtful;
		return happy;
	}

	public static Mood intToEnum(int m) {
		if (m == happy)
			return Mood.happy;
		if (m == sad)
			return Mood.sad;
		if (m == neutral)
			return Mood.neutral;
		if (m == assertive)
			return Mood.assertive;
		if (m == thoughtful)
			return Mood.thoughtful;
		return Mood.happy;
	}

}

public class FamilyInteger {

	public const int daughter = 0;
	public const int father = 1;
	public const int son = 2;
	public const int mother = 3;
	public const int parents = 4;
	public const int siblings = 5;

	public static int enumToInt(Family m) {
		if (m == Family.daughter)
			return daughter;
		if (m == Family.father)
			return father;
		if (m == Family.son)
			return son;
		if (m == Family.mother)
			return mother;
		if (m == Family.parents)
			return parents;
		if (m == Family.siblings)
			return siblings;
		return -1;
	}

	public static Family intToEnum(int m) {
		if (m == daughter)
			return Family.daughter;
		if (m == father)
			return Family.father;
		if (m == son)
			return Family.son;
		if (m == mother)
			return Family.mother;
		if (m == parents)
			return Family.parents;
		if (m == siblings)
			return Family.siblings;
		return Family.mother;
	}

}

[System.Serializable]
public class Vote {

	public int type;
	public int amount;

	public Vote(int t, int a) {
		type = t;
		amount = a;
	}

	public bool Equals(Vote other) {
		if (other.type == type && other.amount == amount)
			return true;
		return false;
	}

}

public class GameController : MonoBehaviour {

	public QuickSaveInfo quickSaveInfo;
	public QuickSaveData quickSaveData;

	public Text currentTurnVotesText;

	public int currentListCount;

	public Flash flashDot_N;
	public UIEnableImageOnTimeout noConnectionIcon_N;

	public int continuedPlayers = 0;

	// this is needed to decide if we have to vote or not
	public List<int> timesPlayed;
	//[HideInInspector]
	public int numberOfVotations = 0;

	public Text scoreText;
	public Text presentText;

	public int tType;
	public int table;
	public bool isNegative;
	public Family member;
	public int family;
	public Mood mood;

	public string randomChallenge;
	public string datetimeOfGame;

	bool showDebug = false;

	public const int Type0Test = 0;
	public const int Type1Test = 1;
	public const int Type2Test = 2;
	public const int Type3Test = 3;
	public const int Type4Test = 4;
	public const int NegativeSituation = 5;
	public const int AllPlaySituation = 5;

	public List<int> obtainedGifts;

	[HideInInspector]
	public int selectedItem;

	public FGBetterNetworkAgent networkAgent;

	public SituationChooser situationChooser;

	public MasterController masterController;

	public NotMyRouletteController notMyRouletteController;
	public ChoosePlayerController choosePlayerController;

	public CreateNewGameController createNewGameController;
	public JoinNewGameController joinNewGameController;

	public MainGameController mainGameController;
	public CommonTestController commonTestController;

	public PreFinishController preFinishController;

	public StatisticsController statisticsController;

	public Type3PrimeAuxController type3AuxController;

	public ContinueGameController continueGameController;

	public GameObject[] turnDisableObjects; 



	public GameObject synchCanvas;

	//[HideInInspector]
	public int turn; // current turn, from 0 to ...



	public const int voteEveryNTurns = 1;


	[HideInInspector]
	public bool isMaster = false;

	//[HideInInspector]
	public int nPlayers = 1;

	public int reportedPlayers = 0;
	public int awardedPlayers = 0;

	public string localUserLogin = "";
	public string localUserPass = "";
	public string localUserEMail = "";

	public int localPlayerN;
	public int playerTurn; // current player turn, modulus MaxPlayers
	public string masterLogin;

	public const int MaxPlayers = 4;

	// own and other people's votation values for current turn
	// used to calculate disperssion!!
	public List<float> turnOtherValues;
	public float turnMyValue; // set by votationController

	public List<float> blueTurnOtherValues;
	public float blueTurnMyValue; // set by votationController
	public List<float> greenTurnOtherValues;
	public float greenTurnMyValue; // set by votationController
	public List<float> yellowTurnOtherValues;
	public float yellowTurnMyValue; // set by votationController


	public List<Vote> currentVote;


	//[HideInInspector]
	public int synchNumber;

	[HideInInspector]
	public int state;
	[HideInInspector]
	public string gameRoom;
	[HideInInspector]
	public string gameHost;

	[HideInInspector]
	public string qrCodeContents;

	[HideInInspector]
	public List<int> playedTimes;

	//[HideInInspector]
	public List<Player> playerList;

	[HideInInspector]
	public List<bool> playerPresent;

	[HideInInspector]
	public int currentTurnPlayer = -1;

	[HideInInspector]
	public int currentTurnVotes = 0;

	[HideInInspector]
	public float test0TurnValue = 0.0f;

	[HideInInspector]
	public Family currentPlayerRole;


	public int playerRequestedEnd = -1;

	[HideInInspector]
	public int NSPlayerAttitude = -1;

	public void toggleDebugInfo() {
		if (showDebug) {
			scoreText.enabled = false;
			presentText.enabled = false;
			showDebug = false;
		} else {
			scoreText.enabled = true;
			presentText.enabled = true;
			showDebug = true;
		}
	}

	public void updateVoteVariables() { // de esto se puede quitar casi todo

		// update blue disperssion

		float diff = 0.0f;
		if (blueTurnMyValue > -1.0f) {

			// 1.- get average
			float blueAverage = 0.0f;
			float nBlueOthers = 0.0f;
			for (int i = 0; i < MaxPlayers; ++i) {
				if ((i != localPlayerN) && (blueTurnOtherValues[i] > -1.0f)) {
					blueAverage += blueTurnOtherValues [i];
					nBlueOthers += 1.0f;
				}
			}
			blueAverage += blueTurnMyValue;
			blueAverage /= (nBlueOthers + 1.0f);

			diff =  ((blueAverage - blueTurnMyValue) * (blueAverage - blueTurnMyValue)) / 4.0f; 

		}
		playerList [localPlayerN].blueDisperssion += diff;


		// update yellow disperssion
		diff = 0.0f;
		if (yellowTurnMyValue > -1.0f) {

			// 1.- get average
			float yellowAverage = 0.0f;
			float nYellowOthers = 0.0f;
			for (int i = 0; i < MaxPlayers; ++i) {
				if ((i != localPlayerN) && (yellowTurnOtherValues[i] > -1.0f)) {
					yellowAverage += yellowTurnOtherValues [i];
					nYellowOthers += 1.0f;
				}
			}
			yellowAverage += yellowTurnMyValue;
			yellowAverage /= (nYellowOthers + 1.0f);

			diff =  ((yellowAverage - yellowTurnMyValue) * (yellowAverage - yellowTurnMyValue)) / 4.0f; 

		}
		playerList [localPlayerN].yellowDisperssion += diff;

		// update green disperssion
		diff = 0.0f;
		if (greenTurnMyValue > -1.0f) {

			// 1.- get average
			float greenAverage = 0.0f;
			float nGreenOthers = 0.0f;
			for (int i = 0; i < MaxPlayers; ++i) {
				if ((i != localPlayerN) && (greenTurnOtherValues[i] > -1.0f)) {
					greenAverage += greenTurnOtherValues [i];
					nGreenOthers += 1.0f;
				}
			}
			greenAverage += greenTurnMyValue;
			greenAverage /= (nGreenOthers + 1.0f);

			diff =  ((greenAverage - yellowTurnMyValue) * (greenAverage - yellowTurnMyValue)) / 4.0f; 

		}
		playerList [localPlayerN].yellowDisperssion += diff;


		// reset turnOtherValues;
		for (int i = 0; i < MaxPlayers; ++i) {
			turnOtherValues[i] = -1.0f;
			blueTurnOtherValues [i] = -1.0f;
			greenTurnOtherValues [i] = -1.0f;
			yellowTurnOtherValues [i] = -1.0f;
		}
		turnMyValue = -1.0f;
		blueTurnMyValue = -1.0f;
		greenTurnMyValue = -1.0f;
		yellowTurnMyValue = -1.0f;

		currentTurnVotes = 0;


		test0TurnValue = 0.0f;

	}

	// next turn please!
	public void nextTurn() { 

		currentVote = new List<Vote> ();

		// just in case....
		// reset turnOtherValues;
		for (int i = 0; i < MaxPlayers; ++i) {
			turnOtherValues[i] = -1.0f;
			blueTurnOtherValues [i] = -1.0f;
			greenTurnOtherValues [i] = -1.0f;
			yellowTurnOtherValues [i] = -1.0f;
		}
		turnMyValue = -1.0f;
		blueTurnMyValue = -1.0f;
		yellowTurnMyValue = -1.0f;
		greenTurnMyValue = -1.0f;
		currentTurnVotes = 0;
		test0TurnValue = 0.0f;

		notMyRouletteController.turnOffAuxPanels ();

		// increment turn
		currentTurnPlayer = -1;
		turn++;



		saveQuickSaveData ();
		saveQuickSaveInfo ();

	}

	public void resetTimesPlayed() {

		for (int i = 0; i < MaxPlayers; ++i) {
			timesPlayed [i] = 0;	
		}

	}

	public bool everyBodyHasVoted(int minimum) {

		for (int i = 0; i < MaxPlayers; ++i) {
			if (playerPresent[i] && (timesPlayed [i] < minimum))
				return false;	
		}
		return true;

	}

	public void reset() {
		turn = 0;
		synchNumber = 0;
		numberOfVotations = 0;
		currentTurnVotes = 0;
		currentTurnPlayer = -1;
		playerRequestedEnd = -1;
		playerList = new List<Player> ();
		timesPlayed = new List<int> ();
		playerPresent = new List<bool> ();
		obtainedGifts = new List<int> ();
		turnOtherValues = new List<float> ();
		blueTurnOtherValues = new List<float> ();
		greenTurnOtherValues = new List<float> ();
		yellowTurnOtherValues = new List<float> ();
		currentVote = new List<Vote> ();
		for (int i = 0; i < MaxPlayers; ++i) {
			playerList.Add (new Player () );
			playerPresent.Add (false);
			timesPlayed.Add (0);
		}
		for (int i = 0; i < MaxPlayers; ++i) {
			turnOtherValues.Add (-1.0f);
			blueTurnOtherValues.Add (-1.0f);
			greenTurnOtherValues.Add (-1.0f);
			yellowTurnOtherValues.Add (-1.0f);
		}
		turnMyValue = -1.0f;
		blueTurnMyValue = -1.0f;
		yellowTurnMyValue = -1.0f;
		greenTurnMyValue = -1.0f;
		synchCanvas.SetActive (false);
		situationChooser.initialize ();
	}

	public void stop() {

		state = 0;

	}

	// Use this for initialization
	void Start () {
		reset ();
		scoreText.enabled = false;
		presentText.enabled = false;
		showDebug = false;
	}

	/* persistance methods */

	public void loadData() {

		if (File.Exists (Application.persistentDataPath + "/save000.dat")) {

			BinaryFormatter formatter = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/save000.dat", FileMode.Open);
			SaveData data = (SaveData) formatter.Deserialize (file);
			localUserLogin = data.currentLogin;
			localUserEMail = data.currentEMail;
			localUserPass = data.currentPass;
			file.Close ();

		}

	}

	public void loadMoarData() {

		if (File.Exists (Application.persistentDataPath + "/save001.dat")) {

			BinaryFormatter formatter = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/save001.dat", FileMode.Open);
			MoarSaveData data = (MoarSaveData) formatter.Deserialize (file);
			obtainedGifts = data.obtainedGifts;
			file.Close ();

		}
	}

	public void quickSave() {



	}

	public void saveData() {

		BinaryFormatter formatter = new BinaryFormatter ();
		FileStream file = File.Open(Application.persistentDataPath + "/save000.dat", FileMode.Create);

		SaveData data = new SaveData ();
		data.currentLogin = localUserLogin;
		data.currentEMail = localUserEMail;
		data.currentPass = localUserPass;

		formatter.Serialize (file, data);
		file.Close ();
	}

	public void saveMoarData() {

		BinaryFormatter formatter = new BinaryFormatter ();
		FileStream file = File.Open(Application.persistentDataPath + "/save001.dat", FileMode.Create);

		MoarSaveData data = new MoarSaveData ();
		data.obtainedGifts = obtainedGifts;

		formatter.Serialize (file, data);
		file.Close ();
	}
	
	// Update is called once per frame
	void Update () {

		currentListCount = currentVote.Count;

		currentTurnVotesText.text = "" + currentTurnVotes;

		presentText.text = "playerTurn: " + playerTurn + "\n\n";
		for (int i = 0; i < MaxPlayers; ++i) {
			presentText.text += ("pPrest[" + i + "] = " + playerPresent [i] + "\n");
		}
		/*
		scoreText.text = 	"blueMyTurnValue: " + blueTurnMyValue + "\n" +
							"yellowMyTurnValue: " + yellowTurnMyValue + "\n" +
							"greenMyTurnValue: " + greenTurnMyValue + "\n\n" +
							"currentTurnVotes: " + currentTurnVotes + "\n\n";
		for (int i = 0; i < MaxPlayers; ++i) {
			scoreText.text += "blueOtherTurnValue[" + i + "]: " + blueTurnOtherValues [i] + "\n";
		}
		scoreText.text += "\n";
		for (int i = 0; i < MaxPlayers; ++i) {
			scoreText.text += "yellowOtherTurnValue[" + i + "]: " + yellowTurnOtherValues [i] + "\n";
		}
		scoreText.text += "\n";
		for (int i = 0; i < MaxPlayers; ++i) {
			scoreText.text += "greenOtherTurnValue[" + i + "]: " + greenTurnOtherValues [i] + "\n";
		}
		*/
		scoreText.text = "blueGivenScore: " + playerList [localPlayerN].blueScoreGiven + "(" + playerList [localPlayerN].blueVotesGiven + ")\n" +
			"greenGivenScore: " + playerList [localPlayerN].greenScoreGiven + "(" + playerList [localPlayerN].greenVotesGiven + ")\n" +
			"yellowGivenScore: " + playerList [localPlayerN].yellowScoreGiven + "(" + playerList [localPlayerN].yellowVotesGiven + ")\n\n";
		

		for (int i = 0; i < MaxPlayers; ++i) {
			if (playerPresent [i] && i != localPlayerN) {
				scoreText.text += "blueReceivedScore[" + i + "] = " + playerList [localPlayerN].blueScoreReceived [i] + "(" + playerList [localPlayerN].blueVotesReceived[i] + ")\n";
			}
		}
		scoreText.text += "\n";
		for (int i = 0; i < MaxPlayers; ++i) {
			if (playerPresent [i] && i != localPlayerN) {
				scoreText.text += "greenReceivedScore[" + i + "] = " + playerList [localPlayerN].greenScoreReceived [i] + "(" + playerList [localPlayerN].greenVotesReceived[i] + ")\n";;
			}
		}
		scoreText.text += "\n";
		for (int i = 0; i < MaxPlayers; ++i) {
			if (playerPresent [i] && i != localPlayerN) {
				scoreText.text += "yellowReceivedScore[" + i + "] = " + playerList [localPlayerN].yellowScoreReceived [i] + "(" + playerList [localPlayerN].yellowVotesReceived[i] + ")\n";
			}
		}

		scoreText.text += "\n\n";

		scoreText.text += "blueDisperssion: " + playerList [localPlayerN].blueDisperssion + "\n";
		scoreText.text += "greenDisperssion: " + playerList [localPlayerN].greenDisperssion + "\n";
		scoreText.text += "yellowDisperssion: " + playerList [localPlayerN].yellowDisperssion + "\n";


		scoreText.text += "\n";
	
	}

	public void setUserLogin(string user) {

		localUserLogin = user;

	}


	public string getUserLogin() {

		return localUserLogin;

	}

	public void setUserPass(string pass) {

		localUserPass = pass;

	}

	public string getUserPass() {

		return localUserPass;

	}

	int playerNFromLogin(string login) {

		for(int i = 0; i<MaxPlayers; ++i) {
			if (playerList [i].login.Equals (login)) {
				return i;
			}
		}
		return -1;

	}

	public void setPlayerLogin(int playern, string login) {
		playerList [playern].login = login;
	}

	public void network_processCommand(string comm) {

		string[] commands = comm.Split ('$'); // split back to back commands

		char[] charcomm = comm.ToCharArray ();
		int nCommands = 0;
		for (int i = 0; i < charcomm.Length; ++i) {
			if (charcomm [i] == '$')
				++nCommands;
		}

		for (int i = 0; i < nCommands; ++i) {

			string command = commands [i];
			command = command.Replace ("\n", "");

			int safeIndex = command.IndexOf ('#');
			string[] safeArg;
			bool processThisCommand = true;
			bool safeCommand = false;
			int safeSeqNum = -1;
			int originOfSafeCommand = -1;
			int expectedSeq = -1;

			if (safeIndex != -1) { // safe command
				safeCommand = true;
				safeArg = command.Split('#');
				int.TryParse (safeArg [0], out safeSeqNum);
				command = safeArg [2];
				int.TryParse (safeArg [1], out originOfSafeCommand);
				expectedSeq = networkAgent.receiveSeqFor (originOfSafeCommand);
				if (safeSeqNum != expectedSeq)
					processThisCommand = false;

				if (safeSeqNum < expectedSeq) {
					networkAgent.sendCommandUnsafe (originOfSafeCommand, "ACK:" + safeSeqNum + ":" + localUserLogin);
				}

			}

			if (processThisCommand) {

				string[] arg = command.Split (':');

				if (command.StartsWith ("ACK")) {
					int sq;
					int.TryParse (arg [1], out sq);
					int origin;
					int.TryParse (arg [2], out origin);
					networkAgent.ack (sq, origin);
				}

				// playerready is issued by clients trying to join in
				if (command.StartsWith ("playerready")) {

					int pl;
					int.TryParse (arg [1], out pl);
					createNewGameController.addPlayer (pl, arg [2]);
				} 

				// roomplayers:< >:< >:< > etc.... hasta null
				else if (command.StartsWith ("roomplayers")) {

					int id = 1;
					while (!arg [id].Equals ("null")) {
						int origin = -1;
						int.TryParse (arg [id], out origin);
						networkAgent.receiveSeqFor (origin); // register roommate for broadcast
						id++;

					}

				}

				//
				else if (command.StartsWith ("rouletteangle")) {

					float d;
					int t;
					float.TryParse (arg [1], out d);
					int.TryParse (arg [2], out t);
					notMyRouletteController.setRouletteAngle (d);


				}



				// buyforest:<forest>:<playern>:<price>
				else if (command.StartsWith ("startgame")) {

					joinNewGameController.startGame (arg [1], arg [2]);

				} 



				// notification:<text>:<param1>:<param2>:<param3>
				else if (command.StartsWith ("notification")) {

					string text;
					string param1;
					string param2;
					string param3;
					text = arg [1];
					param1 = arg [2];
					param2 = arg [3];
					param3 = arg [4];
					//addNotification (text, param1, param2, param3);

				} 

				// setnplayers is issued by NewGameActivityController and accepted by clients
				else if (command.StartsWith ("setnplayers")) {

					int howMany;
					int.TryParse (arg [1], out howMany);
					joinNewGameController.setNPlayers (howMany);

				} 

				// query avaiable color
				else if (command.StartsWith ("querycoloravailable")) {

					int c;
					int.TryParse (arg [1], out c);
					//scannerController.queryColorAvailable (c);

				} 

				//
				else if (command.StartsWith ("startconfirmend")) {
					mainGameController.showEndVotationPanel ();
				}

				//
				else if (command.StartsWith ("endvoteyes")) {
					mainGameController.supportVotation ();
				}

				//
				else if (command.StartsWith ("endvoteno")) {
					mainGameController.cancelVotation ();
				}

				//
				else if (command.StartsWith ("cancelend")) {
					mainGameController.hideEndVotationPanel ();
					playerRequestedEnd = -1;
				}

				// only the master receives this
				else if (command.StartsWith ("requestend")) {

					int pl;
					int.TryParse (arg [1], out pl);

					if (playerRequestedEnd == -1) {
						playerRequestedEnd = pl;

						////mainGameController.end ();
						////networkAgent.broadcast ("playerend:" + pl + ":");
						networkAgent.broadcast ("startconfirmend:");
						mainGameController.showEndVotationPanel ();
						mainGameController.waitForVotation ();
					} else { // already taken
						// to be implemented later:
						// send a NACK to the requester
					}

				}

				//
				else if (command.StartsWith ("requestticket")) {

					int pl;
					int.TryParse (arg [1], out pl);

					if (playerRequestedEnd != -1)
						return; // can't use ticket right now...
					if (currentTurnPlayer <= -1) {
						currentTurnPlayer = pl;
						mainGameController.setTurn (pl);
						timesPlayed [pl]++;
						networkAgent.broadcast ("setturn:" + pl + ":");
					} else { // already taken
						// to be implemented later:
						// send a NACK to the requester
					}

				}

				// 
				else if (command.StartsWith ("refreshserverstatus")) {

					//masterController.refreshServerStatus ();

				} 

				// sethitos:n player:hitos      sent from slaves to master
				else if (command.StartsWith ("sethitos")) {

					int np, hit;
					int.TryParse (arg [1], out np);
					int.TryParse (arg [2], out hit);
					preFinishController.setHitos (np, hit);

				} 

				// setshoes:n player:shoes      sent from slaves to master
				else if (command.StartsWith ("setshoes")) {

					int np, sh;
					int.TryParse (arg [1], out np);
					int.TryParse (arg [2], out sh);
					preFinishController.setShoes (np, sh);

				} 

				// setfinishplayer:n player:f player     sent from slaves to master
				else if (command.StartsWith ("setfinishplayer")) {

					int np, fp;
					int.TryParse (arg [1], out np);
					int.TryParse (arg [2], out fp);
					preFinishController.setFinishPlayer (np, fp);

				} 

				// prefinishdone:my amount of shoes:f player      sent from master to slaves
				else if (command.StartsWith ("prefinishdone")) {
					Debug.Log ("prefinish done!!!");
					int np, fp, nh, wp;
					int.TryParse (arg [1], out wp);
					int.TryParse (arg [2], out np);
					int.TryParse (arg [3], out fp);
					int.TryParse (arg [4], out nh);
					preFinishController.preFinishDone (wp, np, fp, nh);

				}

				// finishns:<color>   in a NegativeSituation, the master commands a slave to show mask with lema
				else if (command.StartsWith ("finishns")) {

					commonTestController.finishNS ();

				}

				// finishns:<color>   in a NegativeSituation, the master commands a slave to show mask with lema
				else if (command.StartsWith ("setanswers")) {

					int t, r, c, c1, c2, member, family, mood, subtype;
					int.TryParse (arg [1], out t);
					int.TryParse (arg [2], out r);
					int.TryParse (arg [3], out c);
					int.TryParse (arg [4], out c1);
					int.TryParse (arg [5], out c2);
					int.TryParse (arg [6], out member);
					int.TryParse (arg [7], out family);
					int.TryParse (arg [8], out mood);
					int.TryParse (arg [9], out subtype);
					notMyRouletteController.setAnswerText (t, r, c, c1, c2, member, family, mood, subtype);
					notMyRouletteController.answerAvailable = true;
					//Debug.Log ("<color=purple>Subtype from network: </color> " + subtype);
				}
		
				// nsmask:<color>   in a NegativeSituation, the master commands a slave to show mask with lema
				else if (command.StartsWith ("nsmask:")) {

					int a;
					int.TryParse (arg [1], out a);
					NSPlayerAttitude = a; // remember which attitude is ours
					commonTestController.startNS (a);

				}

				// nsmask:<color>   in a NegativeSituation, the master commands a slave to show mask with lema
				else if (command.StartsWith ("nsmaskprime")) {

					int a;
					int.TryParse (arg [1], out a);
					NSPlayerAttitude = a; // remember which attitude is ours
					commonTestController.startNS (a);

				}


				// nsmask:<color>   in a NegativeSituation, the master commands a slave to show mask with lema
				else if (command.StartsWith ("maskokbutton")) {

					commonTestController.nsMaskOkButton ();

				}

				// nsvote:<color>   in a NegativeSituation, the master commands a slave to vote
				else if (command.StartsWith ("nsvote")) {

					int a;
					int.TryParse (arg [1], out a);
					commonTestController.voteNS (a);

				}

				// colornotavailable:<color>
				else if (command.StartsWith ("votation")) {

					notMyRouletteController.votation ();

				}

				//
				else if (command.StartsWith ("finishnotmyroulette")) {

					notMyRouletteController.finishNotMyRoulette ();

				}

				// colornotavailable:<color>
				else if (command.StartsWith ("allplaysituation")) {
					int t, r;
					int.TryParse (arg [1], out t);
					int.TryParse (arg [2], out r);
					mainGameController.startAllPlaySituation (t, r);

				} 

				//
				else if (command.StartsWith ("showrepeated")) {
					joinNewGameController.showRepeatedUser (arg [1]);
				}

				//
				else if (command.StartsWith ("userrepeated")) {
					joinNewGameController.showRepeatedUser (arg [1]);
					createNewGameController.showRepeatedUser (arg [1]);
				}

				//
				else if (command.StartsWith ("showcompat")) {
					joinNewGameController.showCompatibility ();
				}

				// coloravailable:<color>
				else if (command.StartsWith ("vote0:")) {

					int giver;
					int receiver;
					int vote;

					int.TryParse (arg [1], out giver);
					int.TryParse (arg [2], out receiver);
					int.TryParse (arg [3], out vote);

					float value = (float)vote;

					playerList [receiver].blueScoreReceived [giver] += value; // update vote received
					playerList [giver].blueScoreGiven += value;

					playerList [receiver].greenScoreReceived [giver] += value; // update vote received
					playerList [giver].greenScoreGiven += value;

					playerList [receiver].yellowScoreReceived [giver] += value; // update vote received
					playerList [giver].yellowScoreGiven += value;



					playerList [receiver].blueVotesReceived [giver]++;
					playerList [receiver].greenVotesReceived [giver]++;
					playerList [receiver].yellowVotesReceived [giver]++;
					playerList [giver].blueVotesGiven++;
					playerList [giver].greenVotesGiven++;
					playerList [giver].yellowVotesGiven++;

					blueTurnOtherValues [giver] = value; // update turnOtherValues for this turn
					greenTurnOtherValues [giver] = value;
					yellowTurnOtherValues [giver] = value;

					test0TurnValue += (float)vote;
					currentTurnVotes++;


				}


				// coloravailable:<color>
				else if (command.StartsWith ("vote:")) {

					int giver;
					int receiver;
					int vote;
					int votetype;
					int.TryParse (arg [1], out giver);
					int.TryParse (arg [2], out receiver);
					int.TryParse (arg [3], out votetype);
					int.TryParse (arg [4], out vote);


					float value = (float)vote;

					if (votetype == TicketScreenController.BlueBottle) {
						//playerList [receiver].blueScoreReceived [giver] += value; // update vote received
						//playerList [giver].blueScoreGiven += value;
						//playerList [receiver].blueVotesReceived [giver]++;
						//playerList [giver].blueVotesGiven++;
						playerList [giver].bottlesGiven [TicketScreenController.BlueBottle] += vote;
						playerList [receiver].bottlesReceived [TicketScreenController.BlueBottle] += vote;
						blueTurnOtherValues [giver] = value; // update turnOtherValues for this turn
					}
					if (votetype == TicketScreenController.RedBottle) {
						//playerList [receiver].greenScoreReceived [giver] += value; // update vote received
						//playerList [giver].greenScoreGiven += value;
						//playerList [receiver].greenVotesReceived [giver]++;
						//playerList [giver].greenVotesGiven++;
						playerList [giver].bottlesGiven [TicketScreenController.RedBottle] += vote;
						playerList [receiver].bottlesReceived [TicketScreenController.RedBottle] += vote;
						greenTurnOtherValues [giver] = value;
					}
					if (votetype == TicketScreenController.YellowBottle) {
						//playerList [receiver].yellowScoreReceived [giver] += value; // update vote received
						//playerList [giver].yellowScoreGiven += value;
						//playerList [receiver].yellowVotesReceived [giver]++;
						//playerList [giver].yellowVotesGiven++;
						playerList [giver].bottlesGiven [TicketScreenController.YellowBottle] += vote;
						playerList [receiver].bottlesReceived [TicketScreenController.YellowBottle] += vote;
						yellowTurnOtherValues [giver] = value;
					}



					currentVote.Add (new Vote (votetype, vote)); // will receive nplayers-1 of these

					//test0TurnValue += (float)vote;
					currentTurnVotes++; // for controlling flow


				}

				// setplayern:<login>:<playern>
				else if (command.StartsWith ("setplayerloginn")) {

					int pl;
					int.TryParse (arg [2], out pl);
					playerList [pl].login = arg [1];

				} 

				//scorereport:<which player>:<first to goal score>:<shoes score>:<to others>:<from others>
				else if (command.StartsWith ("scorereport")) {

					int pl;
					int poi;
					int.TryParse (arg [1], out pl);
					int.TryParse (arg [2], out poi);
				
					statisticsController.setScore (pl, poi);

				}

				//
				else if (command.StartsWith ("disableplayer")) {

					int pl;
					int.TryParse (arg [1], out pl);
					int owner;
					int.TryParse (arg [2], out owner);
					choosePlayerController.disablePlayer (pl, owner);

				}



				//
				else if (command.StartsWith ("claim:")) {

					int id;
					int.TryParse (arg [1], out id);
					int pl;
					int.TryParse (arg [2], out pl);
					choosePlayerController.claimPlayer (id, pl);

				}

				//
				else if (command.StartsWith ("claimplayerACK")) {

					choosePlayerController.claimPlayerACK ();

				}

				//
				else if (command.StartsWith ("claimplayerNACK")) {

					choosePlayerController.claimPlayerNACK ();

				}

				// start the scanning!
				else if (command.StartsWith ("nschosenmask")) {

					int m;
					int.TryParse (arg [1], out m);
					type3AuxController.setChosenMask (m);

				} 

				// start the scanning!
				else if (command.StartsWith ("setplayerpresent")) {

					int pl;
					int.TryParse (arg [1], out pl);
					playerList [pl].login = arg [2];
					playerPresent [pl] = true;

				} 

				// start the scanning!
				else if (command.StartsWith ("startscan")) {

					//joinGameController.startscan ();

				} else if (command.StartsWith ("negativesituation:")) {

					int situ;
					int.TryParse (arg [1], out situ);
					//notMyRouletteController.

				}

				//
				//
				else if (command.StartsWith ("synch:")) {

					++synchNumber;

				}


				// setplayern:<login>:<nplayer>
				// set player number
				else if (command.StartsWith ("setplayern")) {

					if (localUserLogin.Equals (arg [1])) {
						int p;
						int.TryParse (arg [2], out p);
						localPlayerN = p;
					}

				}

				// playerend:<who>
				else if (command.StartsWith ("playerend")) {

					//int pl;
					//int.TryParse (arg [1], out pl);

					//playerList [pl].firstToGoal = true;

					mainGameController.end ();

				}


				// setturn:<playerturn>
				else if (command.StartsWith ("setturn")) {

					int t;
					int.TryParse (arg [1], out t);
					timesPlayed [t]++;
					mainGameController.setTurn (t);

				}


				// setturn:<playerturn>
				else if (command.StartsWith ("nextturn")) {

					//playerActivityController.nextTurn ();

				}
				


				// setturn:<playerturn>
				else if (command.StartsWith ("votevolcano:yes")) {

					int t;
					int.TryParse (arg [2], out t);
					playerList [t].endGameVote = 1;

				}

				//
				else if (command.StartsWith ("restartturn")) {
					notMyRouletteController.cancelNoyMyRoulette ();
					mainGameController.restartTurn ();
				}

				// setturn:<playerturn>
				else if (command.StartsWith ("votevolcano:no")) {

					int t;
					int.TryParse (arg [2], out t);
					playerList [t].endGameVote = -1;

				}

				// setturn:<playerturn>
				else if (command.StartsWith ("votereset:yes")) {

					int t;
					int.TryParse (arg [2], out t);
					playerList [t].endGameVote = 1;

				}

				// setturn:<playerturn>
				else if (command.StartsWith ("reportcontinue")) {
					int otherUser;
					int.TryParse (arg [1], out otherUser);
					int ttl;
					int.TryParse (arg [3], out ttl);
					continueGameController.ReportContinue (otherUser, arg [2], ttl);

				} 


				else if (command.StartsWith ("roomuuid")) { // hard coded
					if (!continueGameController.tryingToContinue) {
						localUserLogin = arg [1];
					}
				}



				// report:<player>:<life>:<work>... etc...
				else if (command.StartsWith ("report:")) {

					int player;
					int test;
					int work;
					int school;
					int gompa;
					int guru;
					int volcano;
					int build;
					int v, y, n, t;
					int.TryParse (arg [1], out player);
					int.TryParse (arg [2], out test);
					int.TryParse (arg [3], out work);
					int.TryParse (arg [4], out school);
					int.TryParse (arg [5], out gompa);
					int.TryParse (arg [6], out guru);
					int.TryParse (arg [7], out volcano);
					int.TryParse (arg [8], out build);
					int.TryParse (arg [9], out v);
					int.TryParse (arg [10], out y);
					int.TryParse (arg [11], out n);
					int.TryParse (arg [12], out t);

					//notMyTurnController.turnReport (player, test == 1, work == 1, school == 1, gompa == 1, guru == 1, volcano == 1, build == 1,
					//	v == 1, y == 1, n == 1, t == 1);

				}

				// acquire secondary wisdoms
				else if (command.StartsWith ("secondarywisdoms")) {

					int pl;
					int.TryParse (arg [1], out pl);
					//playerList [pl].hasSecondaryWisdoms = true;

				}

				// cofirm end of turn OK
				else if (command.StartsWith ("confirm:yes")) {

					int pl;
					int.TryParse (arg [2], out pl);
					//PlayerTurnVotation [pl] = 1;

				}

				// cofirm end of turn NO
				else if (command.StartsWith ("confirm:no")) {

					int pl;
					int.TryParse (arg [2], out pl);
					//PlayerTurnVotation [pl] = -1;

				}
				

				// playerreconnect:<playerturn>
				//	request from a player that has previously lost it's connection to recover it's data
				else if (command.StartsWith ("playerreconnect")) {

					//int player = playerNFromLogin (arg [1]);
					//if (player == -1)
					//	return; // some error, do nothing



					//++nPlayers; // one player less
					//state = 100;

				}


				// setturn:<playerturn>
				else if (command.StartsWith ("playerdisconnect")) {

					if ((arg [1] == null) || arg [1].Equals ("<null>"))
						return; // just ignore these

					//playerDisconnectInterface.SetActive (true);
					//playerDisconnectText.text = "El jugador " + arg [1] + " se ha desconectado del servidor";
					//// show qr code with session info for recovery
					//globalQRSessionInfoImage.texture = qrEncodedSessionInfo;
					//--nPlayers; // one player less
					//if (nPlayers == 1) {
					//	dismissPlayerText.enabled = false; // can't just dismiss, can't play alone!
					//}
					state = 100;

				}
				

				// synch:<player>:<turn>
				// player <player> declares has updated it's state to turn <turn>
				else if (command.StartsWith ("polo")) {

					//String aaa = ":3";
					if (flashDot_N != null) {
						flashDot_N.flash ();
					}
					if (noConnectionIcon_N != null) {
						noConnectionIcon_N.keepAlive ();
					}

					networkAgent.poloElapsedTime = 0.0f;


				}

				// synch:<player>:<turn>
				// player <player> declares has updated it's state to turn <turn>
				else if (command.StartsWith ("nuke")) {

					masterController.hardReset ();


				}



				// just for fun
				else if (command.StartsWith ("ding")) {
					//masterController.playSound (dingSound);
				}
		
				if (safeCommand) { // if safe command, acknowledge processing

					networkAgent.incReceiveSeqFor (originOfSafeCommand);
					networkAgent.sendCommandUnsafe (originOfSafeCommand, "ACK:" + safeSeqNum + ":" + getUserLogin ());


				}
			
			} // end of if(processThisCommand...




		}


	}

	public void resetQuickSaveInfo() {

		BinaryFormatter formatter = new BinaryFormatter ();
		FileStream file = File.Open(Application.persistentDataPath + "/quicksaveinfo.dat", FileMode.Create);

		QuickSaveInfo quickSaveInfo_ = new QuickSaveInfo ();
		quickSaveInfo_.numberOfPlayers = 0;
		quickSaveInfo_.roomId = "";
		quickSaveInfo_.randomChallenge = "";
		quickSaveInfo_.turn = 0;
		quickSaveInfo_.datetime = "";
		quickSaveInfo_.playcode = quickSaveInfo.playcode;

		formatter.Serialize (file, quickSaveInfo_);
		file.Close ();

	}

	public bool checkQuickSaveInfo() {

		if (File.Exists (Application.persistentDataPath + "/quicksaveinfo.dat")) {

			BinaryFormatter formatter = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/quicksaveinfo.dat", FileMode.Open);
			QuickSaveInfo data = (QuickSaveInfo)formatter.Deserialize (file);
			quickSaveInfo = data;
			file.Close ();
			return (quickSaveInfo.numberOfPlayers != 0);

		} else {
			quickSaveInfo = new QuickSaveInfo ();
			return false;
		}

	}

	public void saveQuickSaveInfo() {

		BinaryFormatter formatter = new BinaryFormatter ();
		FileStream file = File.Open(Application.persistentDataPath + "/quicksaveinfo.dat", FileMode.Create);

		QuickSaveInfo quickSaveInfo_ = new QuickSaveInfo ();
		quickSaveInfo_.numberOfPlayers = nPlayers;
		quickSaveInfo_.roomId = gameRoom;
		quickSaveInfo_.randomChallenge = randomChallenge;
		quickSaveInfo_.turn = turn;
		quickSaveInfo_.datetime = datetimeOfGame;
		quickSaveInfo_.login = localUserLogin;
		quickSaveInfo_.playcode = quickSaveInfo.playcode;

		formatter.Serialize (file, quickSaveInfo_);
		file.Close ();


	}

	public void noConnectionIconSetEnabled(bool en) {
		if (noConnectionIcon_N != null) {
			if (en)
				noConnectionIcon_N.go ();
			else
				noConnectionIcon_N.stop ();
		}
	}

	public bool loadQuickSaveData() {

		if (File.Exists (Application.persistentDataPath + "/quicksavedata.dat")) {

			BinaryFormatter formatter = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/quicksavedata.dat", FileMode.Open);
			QuickSaveData data = (QuickSaveData)formatter.Deserialize (file);
		

			awardedPlayers = data.awardedPlayers;
			blueTurnMyValue = data.blueTurnMyValue;
			blueTurnOtherValues = data.blueTurnOtherValues;
			currentTurnPlayer = -1; //data.currentTurnPlayer;
			currentVote = data.currentVote;
			greenTurnMyValue = data.greenTurnMyValue;
			greenTurnOtherValues = data.greenTurnOtherValues;
			isMaster = data.isMaster;
			localPlayerN = data.localPlayerN;
			localUserLogin = data.localUserLogin;
			localUserPass = data.localUserPass;
			masterLogin = data.masterLogin;
			nPlayers = data.nPlayers;
			numberOfVotations = data.numberOfVotations;
			playerList = data.playerList;
			playerPresent = data.playerPresent;
			playerTurn = -1; //data.playerTurn;
			reportedPlayers = data.reportedPlayers;
			selectedItem = data.selectedItem;
			timesPlayed	= data.timesPlayed;
			turn = data.turn;
			yellowTurnMyValue = data.yellowTurnMyValue;
			yellowTurnOtherValues = data.yellowTurnOtherValues;

			file.Close ();
			return true;

		} else
			return false;

	}

	public void saveQuickSaveData() {

		BinaryFormatter formatter = new BinaryFormatter ();
		FileStream file = File.Open(Application.persistentDataPath + "/quicksavedata.dat", FileMode.Create);

		QuickSaveData quickSaveData = new QuickSaveData ();

		quickSaveData.awardedPlayers = awardedPlayers;
		quickSaveData.blueTurnMyValue = blueTurnMyValue;
		quickSaveData.blueTurnOtherValues = blueTurnOtherValues;
		quickSaveData.currentTurnPlayer = currentTurnPlayer;
		quickSaveData.currentVote = currentVote;
		quickSaveData.greenTurnMyValue = greenTurnMyValue;
		quickSaveData.greenTurnOtherValues = greenTurnOtherValues;
		quickSaveData.isMaster = isMaster;
		quickSaveData.localPlayerN = localPlayerN;
		quickSaveData.localUserLogin = localUserLogin;
		quickSaveData.localUserPass = localUserPass;
		quickSaveData.masterLogin = masterLogin;
		quickSaveData.nPlayers = nPlayers;
		quickSaveData.numberOfVotations = numberOfVotations;
		quickSaveData.playerList = playerList;
		quickSaveData.playerPresent = playerPresent;
		quickSaveData.playerTurn = playerTurn;
		quickSaveData.reportedPlayers = reportedPlayers;
		quickSaveData.selectedItem = selectedItem;
		quickSaveData.timesPlayed = timesPlayed;
		quickSaveData.turn = turn;
		quickSaveData.yellowTurnMyValue = yellowTurnMyValue;
		quickSaveData.yellowTurnOtherValues = yellowTurnOtherValues;

		formatter.Serialize (file, quickSaveData);
		file.Close ();

	}



}


[Serializable]
class SaveData {

	public string currentLogin;
	public string currentPass;
	public string currentEMail;

}

[Serializable]
class MoarSaveData {

	public List<int> obtainedGifts;

}

[Serializable]
public class QuickSaveInfo {

	public int turn;
	public int numberOfPlayers; // 0 means NO quicksaveinfo
	public string roomId;
	public string randomChallenge;
	public string login;
	public string datetime;
	public string playcode;
	public string myServerNetworkAddress;
	public string myNetworkAddress;

	public QuickSaveInfo() {
		turn = 0;
		numberOfPlayers = 0;
		roomId = "";
		randomChallenge = "";
		login = "";
		datetime = "";
		playcode = "";
		myServerNetworkAddress = "";
		myNetworkAddress = "";
	}

}


[Serializable]
public class QuickSaveData {

	public List<bool> playerPresent;
	public List<Player> playerList;
	public List<int> timesPlayed;
	public int numberOfVotations;
	public int selectedItem;
	public int turn;
	public bool isMaster;
	public int nPlayers;
	public int reportedPlayers;
	public int awardedPlayers;
	public string localUserLogin;
	public string localUserEMail;
	public string localUserPass;
	public int localPlayerN;
	public string masterLogin;
	public int currentTurnPlayer;
	public List<Vote> currentVote;
	public List<float> blueTurnOtherValues;
	public float blueTurnMyValue;
	public List<float> greenTurnOtherValues;
	public float greenTurnMyValue;
	public List<float> yellowTurnOtherValues;
	public float yellowTurnMyValue;
	public int playerTurn;

}