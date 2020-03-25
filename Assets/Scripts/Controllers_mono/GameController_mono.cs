using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class GameController_mono : MonoBehaviour {

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

	public SituationChooser situationChooser;

	public MasterController_mono masterController;

	public NotMyRouletteController_mono notMyRouletteController;
	public ChoosePlayerController_mono choosePlayerController;

	//public CreateNewGameController_mono createNewGameController;
	//public JoinNewGameController_mono joinNewGameController;

	public MainGameController_mono mainGameController;
	public CommonTestController_mono commonTestController;

	public PreFinishController_mono preFinishController;

	public StatisticsController_mono statisticsController;

	public Type3PrimeAuxController_mono type3AuxController;

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

	public void updateVoteVariables() {

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

	/* network methods */




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


