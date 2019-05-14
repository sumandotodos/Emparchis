using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CreateNewGameController : Task {

	static CreateNewGameController theInstance;

	public MasterController masterController;
	public MasterControllerType mcType;

	public GameController gameController;

	public GameObject createNewGameCanvas;

	public RosettaWrapper rosetta;
	public StringBank generalStringBank;

	public RawImage qrResults;
	public RawImage resultImage;
	public QRCodeEncodeController qrEncoder;

	public Text numberOfPlayersText;
	public Button startGameButton;

	public UIFaderScript fader;

	public FGTable messagesTable;
	public UIScaleFader updateNoticeScaler;
	public Text updateNoticeText;

	WWWForm newForm;
	WWW newWWW;

	int state;

	public int nPlayers = 1;

	List<int> seenPlayers;

	public int HideQRResult(int param) {
		resultImage.enabled = false;
		return param;
	}

	string _nextServer = "";
	public int SetNextServer(string s) {
		//MasterController.StaticLog ("<color=red>Next server: " + s + "</color>");
		_nextServer = s;
		return 0;
	}

	public int ShowQRResult(int param) {
		//MasterController.StaticLog ("<color=purple>ShowQRResult again</color>");
		qrEncoder.Encode (_nextServer + ":" );
		return param;
	}

	public void init() {
		seenPlayers = new List<int>();
		//qrEncoder.initialize ();
		qrEncoder.onQREncodeFinished += qrEncodeReady;
		startGameButton.interactable = false;
		gameController.quickSaveData.localUserLogin = "-1";
		gameController.localUserLogin = "-1";
		state = 1;
		fader.fadeIn ();
		gameController.isMaster = true;
		updateNoticeScaler.scaleOutImmediately ();
		string sss = (string)messagesTable.getElement (0, Utils.MsgIncompatVersion);
		updateNoticeText.text = (string)messagesTable.getElement (0, Utils.MsgIncompatVersion);
	}

	public void startCreateNewGameActivity(Task w) {
		w.isWaitingForTaskToComplete = true;
		waiter = w;
		numberOfPlayersText.text = "1 jugador";
		nPlayers = 1;
		init ();
	}

	// Use this for initialization
	void Start () {
		state = 0;
		theInstance = this;
	}

	public static CreateNewGameController GetSingleton() {
		return theInstance;
	}

	void qrEncodeReady(Texture encodedQR) 
	{
		qrResults.texture = encodedQR;
	}
	
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			backArrow ();
		}

		if (state == 0) { // idling

		}

		if (state == 1) { // a new request
			
			//newWWW = new WWW (gameController.networkAgent.bootstrapData.loginServer + "/nextRoomID.php");

			state = 2;
		}

		if (state == 2) { // waiting for www result

			//if (newWWW.isDone) {
				
					string prefix = "Emp";
					switch (mcType) {
					case MasterControllerType.multi:
						prefix = "EmpNA";
						break;
					case MasterControllerType.mono:
						prefix = "EmpMA";
						break;
					case MasterControllerType.multikids:
						prefix = "EmpNK";
						break;
					case MasterControllerType.monokids:
						prefix = "EmpMK";
						break;
					}
					
				string roomname = prefix + newWWW.text;

				//otherQREncoder.Encode (qrContents + ":recovery:");
				gameController.gameRoom = roomname;
				gameController.datetimeOfGame = System.DateTime.Now.ToString();
				gameController.randomChallenge = "Emp" + Random.Range (0, System.Int32.MaxValue ).ToString ();
				gameController.networkAgent.initialize ("", 0);
				++state;
			//}
		}

		if (state == 3) { // waiting for all users to be ready
			if (!gameController.localUserLogin.Equals ("-1")) {
				Debug.Log ("<color=green>Login acquired :  " + gameController.localUserLogin + "</color>");
				gameController.quickSaveData.localUserLogin = gameController.localUserLogin;
				gameController.quickSaveInfo.login = gameController.localUserLogin;
				gameController.masterLogin = gameController.localUserLogin;
				string qrContents = gameController.localUserLogin + ":" + gameController.gameRoom;
				// store contents of qrCode into gameController
				gameController.qrCodeContents = qrContents + ":";
				string encodeThis = qrContents;
				Debug.Log ("qr: " + encodeThis);
				qrEncoder.Encode (encodeThis);
				state = 4;
			}
		}

		if (state == 4) { // waiting for all users to be ready

		}

		if (state == 5) 
		{
			if (!isWaitingForTaskToComplete) {
				gameController.currentPlayerRole = Family.daughter;
				masterController.startActivity = "ChoosePlayer";
				createNewGameCanvas.SetActive (false);
				notifyFinishTask (); // return to MasterController flow
				state = 0;
			}
		}
			// returning back to title
		if (state == 100) {
			fader.fadeOutTask (this); 
			state = 101;
		} else if (state == 101) {
			if (!isWaitingForTaskToComplete) {
				
				masterController.startActivity = "Title";
				notifyFinishTask ();
				state = 0;
			}
		}
	}



	public void addPlayer(int playerId, string compatCode) 
	{
		

		if (!gameController.isMaster)
			return;
		

		int myCompat, playerCompat;
	

		if (nPlayers == GameController.MaxPlayers) {
			gameController.networkAgent.sendCommandUnsafe (playerId, "nuke:$");
			gameController.networkAgent.unseeOrigin (playerId);
			return;
		}

		++nPlayers;
		seenPlayers.Add (playerId);
		gameController.nPlayers = nPlayers;
		gameController.networkAgent.broadcast ("setnplayers:" + nPlayers);
		numberOfPlayersText.text = nPlayers + " jugadores";
		startGameButton.interactable = true;
	}

	public void showCompatibility() 
	{
		updateNoticeText.text = (string)messagesTable.getElement (0, Utils.MsgIncompatVersion);
		updateNoticeText.text = updateNoticeText.text.Replace ("\\n", "\n");
		updateNoticeScaler.scaleIn ();
	}

	public void showRepeatedUser(string offendingUser) 
	{
		updateNoticeText.text = (string)messagesTable.getElement (0, Utils.MsgRepeatedLogin);
		updateNoticeText.text = updateNoticeText.text.Replace ("\\n", "\n");
		updateNoticeText.text = updateNoticeText.text.Replace ("<1>", "");
		updateNoticeScaler.scaleIn ();
	}


	/*
	 * 
	 * 
	 * Callbacks
	 * 
	 */

	// UI Events callbacks
	public void backArrow() 
	{
		masterController.hardReset ();
		//state = 100;
	}

	// called by button  'startGameButton'  push
	public void startGameCallback() 
	{
		fader.fadeOutTask (this);
		string networkDateTime = gameController.datetimeOfGame.Replace (" ", "_"); // no spaces
		networkDateTime = networkDateTime.Replace (":", "!"); // or colons, please
		gameController.networkAgent.broadcast("startgame:" + gameController.randomChallenge + ":" + networkDateTime + ":");
		//gameController.network_sendMessage ("play Emp"); // no cuentes créditos
		if (masterController.titleController.accountCredits > 0) {
			masterController.titleController.creditsHUD.text = "Créditos: " + (masterController.titleController.accountCredits - 1);
		}
		masterController.titleController.buyButton.GetComponent<Button> ().interactable = false;
		string playersString = "";
		for (int i = 0; i < seenPlayers.Count; ++i) 
		{			
			playersString += (seenPlayers[i] + ":");
		}
		playersString += "null:";
		gameController.networkAgent.broadcast ("roomplayers:" + playersString);
		state = 5;
		gameController.saveData ();
	}
}
