/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueGameController : Task {

	public MasterController masterController;
	public GameController gameController;

	public int neededPlayers;
	public int reportedPlayers;

	public Text playerCountText;
	public Text dateTimeText;

	float elapsedTime;
	const float InitialInterval = 1.0f;
	const float Interval = 5.0f;
	float timeToWait;
	public int state;

	public bool tryingToContinue = false;

	string userLogin;
	string userRoom;

	public bool isActive = false;

	public List<int> joinedPlayers;

	public void startContinueGame(Task w) {
		w.isWaitingForTaskToComplete = true;
		waiter = w;

		tryingToContinue = true;

		isActive = true;

		neededPlayers = gameController.quickSaveInfo.numberOfPlayers;
		reportedPlayers = 0;

		userLogin = gameController.localUserLogin = gameController.quickSaveInfo.login;
		gameController.gameRoom = userRoom = gameController.quickSaveInfo.roomId;
		gameController.randomChallenge = gameController.quickSaveInfo.randomChallenge;
		gameController.datetimeOfGame = gameController.quickSaveInfo.datetime;


		joinedPlayers = new List<int> ();

		dateTimeText.text = gameController.quickSaveInfo.datetime;

		timeToWait = InitialInterval;

		masterController.titleController.buyButton.GetComponent<Button> ().interactable = false;

		state = 1;

	}

	// Use this for initialization
	void Start () {

		isActive = false;

	}

	public void deinitialize() {
		isActive = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (state == 0) {

		}

		if (state == 1) {
			timeToWait -= Time.deltaTime;
			if (timeToWait < 0.0f) {
				timeToWait = Interval;

				gameController.networkAgent.broadcastUnsafe("reportcontinue:" + userLogin + ":" + gameController.quickSaveInfo.randomChallenge + ":2:");
			}

			if (reportedPlayers == (neededPlayers-1)) {
				state = 2;
			}

			playerCountText.text = (reportedPlayers + 1) + "/" + (neededPlayers);

		}

		if (state == 2) {
			masterController.startActivity = "MainGame";
			gameController.loadQuickSaveData ();

			deinitialize ();
			notifyFinishTask ();
			state = 0;
		}

	}
		

	public void ReportContinue(int otherUser, string randomChallenge, int ttl) {

		if (!isActive)
			return;

		if (ttl > 0) {

			if (randomChallenge.Equals (gameController.quickSaveInfo.randomChallenge)) {

				if (!joinedPlayers.Contains (otherUser)) {
					joinedPlayers.Add (otherUser);
					++reportedPlayers;
				}

			}

			gameController.networkAgent.sendCommandUnsafe (otherUser, "reportcontinue:" + userLogin + ":" + gameController.quickSaveInfo.randomChallenge + ":" + (ttl - 1) + ":");

		}

	}
}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueGameController : Task {

	public Text playerCountText;
	public Text dateTimeText;
	int foundPlayers;

	public MasterController masterController;
	public GameController gameController;

	public int neededPlayers;
	public int reportedPlayers;

	float elapsedTime;
	const float InitialInterval = 1.0f;
	const float Interval = 5.0f;
	float timeToWait;
	public int state = 0;

	string userLogin;
	string userRoom;

	public List<int> joinedPlayers;

	public bool working = false;

	public bool tryingToContinue = false;

	public string myNetworkAddress = "";
	public string myServerAddress = "";

	public void startContinueGame(Task w) {

		MasterController.StaticLog ("Starting continue game...");
		gameController.checkQuickSaveInfo ();
		MasterController.StaticLog ("...quick save info checked");
		MasterController.StaticLog ("<color=red>Connect to address: " + gameController.quickSaveInfo.myServerNetworkAddress + "</color>");
		MasterController.StaticLog ("<color=blue>My address: " + gameController.quickSaveInfo.myNetworkAddress + "</color>");



		tryingToContinue = true;
		w.isWaitingForTaskToComplete = true;
		waiter = w;

		neededPlayers = gameController.quickSaveInfo.numberOfPlayers;
		reportedPlayers = 0;

		userLogin = gameController.getUserLogin ();
		//userRoom = gameController.quickSaveInfo.roomId;

		userLogin = gameController.localUserLogin = gameController.quickSaveInfo.login;
		//gameController.gameRoom = userRoom = gameController.quickSaveInfo.roomId;

		gameController.nPlayers = gameController.quickSaveInfo.numberOfPlayers;

		myNetworkAddress = gameController.quickSaveInfo.myNetworkAddress;
		myServerAddress = gameController.quickSaveInfo.myServerNetworkAddress;

		gameController.randomChallenge = gameController.quickSaveInfo.randomChallenge;
		gameController.datetimeOfGame = gameController.quickSaveInfo.datetime;



		joinedPlayers = new List<int> ();

		gameController.networkAgent.initialize ("", 0);
		gameController.gameRoom = userRoom;
		//gameController.networkAgent.connectAndEnterRoom ();
		// esto de continuar vamos a ver cómo lo hacemos....


		gameController.networkAgent.ConnectedToServer = true;

		MasterController.StaticLog ("<color=yellow>about to set dateTiemText.text with "+gameController.quickSaveInfo.datetime+"</color>");
		dateTimeText.text = gameController.quickSaveInfo.datetime;

		timeToWait = InitialInterval;

		working = true;
		state = 1;

		if (myServerAddress != "") {
			MasterController.StaticLog ("<color=red>Continue connecting to: " + myServerAddress + "</color>");
			gameController.networkAgent.StartClient (myServerAddress, ClientDidConnect);
			gameController.networkAgent.SetServerAddress (myNetworkAddress);
			MasterController.StaticLog ("<color=red>Continue connecting to: " + myServerAddress + " - done</color>");
		} else {
			MasterController.StaticLog ("<color=red>Continue starting server as: " + myNetworkAddress + "</color>");
			gameController.networkAgent.StartServer (myNetworkAddress);
			MasterController.StaticLog ("<color=red>Continue starting server as: " + myNetworkAddress + " - done </color>");
		}


	}

	public int ClientDidConnect(int p) {
		MasterController.StaticLog ("<color=red>Continue#client did connect: </color>");

		return 0;
	}

	// Use this for initialization
	void Start () {


	}

	// Update is called once per frame
	void Update () {

		if (state == 0) {

		}

		if (state == 1) {
			timeToWait -= Time.deltaTime;
			if (timeToWait < 0.0f) {
				timeToWait = Interval;
				MasterController.StaticLog ("<color=magenta>Sending beacon...</color>");
				if (gameController.networkAgent.id != -1) {
					gameController.networkAgent.broadcastUnsafe ("reportcontinue:" + gameController.networkAgent.id + ":" + gameController.quickSaveInfo.randomChallenge + ":2:");
				}
			}

			if (reportedPlayers == (neededPlayers-1)) {
				state = 2;
			}

			playerCountText.text = (reportedPlayers + 1) + "/" + (neededPlayers);

		}

		if (state == 2) {
			masterController.startActivity = "MainGame";
			gameController.loadQuickSaveData ();

			gameController.quickSaveInfo.turn--;

			gameController.networkAgent.Cleanup ();
			//setupRoom ();
			notifyFinishTask ();
			working = false;
			state = 0;
			//masterController.playerImage.texture = masterController.playerTexture [gameController.localPlayerN];
			//masterController.playerImage.enabled = true;
		}

	}

	/*
	public void setupRoom() {

		for (int i = 0; i < GameController.MaxPlayers; ++i) {
			if (gameController.playerPresent [i]) {
				gameController.networkAgent.receiveSeqFor (gameController.playerList [i].id);
			}
		}
		//gameController.networkAgent.receiveSeqFor (gameController.masterLogin);


	}
	*/

	public void ReportContinue(int otherUser, string randomChallenge, int ttl) {

		if (!working)
			return;

		if (ttl > 0) {

			if (randomChallenge.Equals (gameController.quickSaveInfo.randomChallenge)) {

				if (!joinedPlayers.Contains (otherUser)) {
					joinedPlayers.Add (otherUser);
					++reportedPlayers;
				}

			}

			gameController.networkAgent.sendCommandUnsafe (otherUser, "reportcontinue:" + gameController.networkAgent.id + ":" + gameController.quickSaveInfo.randomChallenge + ":" + (ttl - 1) + ":");

		}

	}


}
