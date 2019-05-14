using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JoinNewGameController : Task {

	public MasterController masterController;
	public GameController gameController;
	public QRCodeDecodeController qrDecoder;
	public UIFaderScript fader;
	public GameObject webStream;

	public MasterControllerType mcType;

	public GameObject joinNewGameCanvas;

	public Text numberOfPlayersText;

	public AudioClip gong;

	int state;

	bool firstSetNPlayers = true;

	public FGTable messagesTable;
	public UIScaleFader updateNoticeScaler;
	public Text updateNoticeText;

	public void stop() {
		state = 0;
	}

	public void startJoinNewGameActivity(Task w) {

		#if UNITY_IOS
		webStream.transform.localScale = new Vector3(1, -1, 1);
		#endif

		w.isWaitingForTaskToComplete = true;
		waiter = w;
		state = 0;
		//qrDecoder.initialize ();
		//qrDecoder.startWebcam ();
		qrDecoder.e_DeviceController.assignCameraPlane(webStream);
		qrDecoder.e_DeviceController.StartWork ();

		qrDecoder.onQRScanFinished += qrDecodeReady;
		firstSetNPlayers = true;
		gameController.isMaster = false;
		numberOfPlayersText.text = "1 jugador";
		fader.fadeIn ();
		updateNoticeScaler.scaleOutImmediately ();
		updateNoticeText.text = (string)messagesTable.getElement (0, Utils.MsgIncompatVersion);

	}

	public void qrDecodeReady(string payload) 
	{

		Handheld.Vibrate ();

		string[] arg = payload.Split (':');
		gameController.masterLogin = arg [0];
		gameController.gameHost = arg [0];
		gameController.gameRoom = arg [1];

		//qrEncoder.Encode (arg[0] + arg[1] + ":recovery:");

//		if (!arg [1].StartsWith (FGUtils.localGamePrefix))
//			return;
//
//		if (!arg [3].Equals (FGUtils.compatibilityCode))
//			return;

		//if (arg [2].Equals ("newgame")) {
		bool sameVersion = false;
		if (mcType == MasterControllerType.multi) {
			if (arg [1].StartsWith ("EmpNA")) {
				sameVersion = true;
			}
		} else if (mcType == MasterControllerType.multikids) {
			if (arg [1].StartsWith ("EmpNK")) {
				sameVersion = true;
			}
		}
		if (!sameVersion) {
			updateNoticeScaler.scaleIn ();
			updateNoticeText.text = "Error: diferentes versiones del juego";
			return;
		}


			string myUser = gameController.getUserLogin ();
			if(arg[0].Equals(myUser)) {
				showRepeatedUser(myUser);
				myUser = myUser.Replace("@", "_");
			}
			//gameController.network_joinGame (arg [1], myUser);
			//gameController.networkAgent.sendCommand (arg [0], "playerready:" + gameController.getUserLogin () + ":" + "001");
			gameController.localUserLogin = "-1";
			masterController.playSound (gong);
			gameController.networkAgent.initialize ("",
				0);
			//gameController.network_initGame (gameController.gameRoom);
			state = 1;

		//} 

		//else { // recovery

			//gameController.network_joinGame (arg [1]);
			////gameController.networkAgent.sendCommand (arg [0], "playerreconnect:" + gameController.getUserLogin ());
			//gameController.networkAgent.broadcast("playerreconnect:" + gameController.getUserLogin() + ":");

		//}

		//qrDecoder.stopWebcam ();
		qrDecoder.StopWork();

	}

	// called by network command setnplayers
	//  gameController.localPlayerN is set here
	public void setNPlayers(int pl) {

		gameController.nPlayers = pl;
		numberOfPlayersText.text = pl + " jugadores";
		if (firstSetNPlayers) {
			// acquire player number
			gameController.localPlayerN = pl - 1;
			firstSetNPlayers = false;
		}

	}

	public void showCompatibility() {
		updateNoticeText.text = (string)messagesTable.getElement (0, Utils.MsgIncompatVersion);
		updateNoticeText.text = updateNoticeText.text.Replace ("\\n", "\n");
		updateNoticeScaler.scaleIn ();
	}

	public void showRepeatedUser(string offendingUser) {
		updateNoticeText.text = (string)messagesTable.getElement (0, Utils.MsgRepeatedLogin);
		updateNoticeText.text = updateNoticeText.text.Replace ("\\n", "\n");
		updateNoticeText.text = updateNoticeText.text.Replace ("<1>", "");
		updateNoticeScaler.scaleIn ();
	}

	void Start () 
	{
	
	}
	
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			backArrow ();
		}
	
		if (state == 0) { // idle state

		}

		if (state == 1) { // waiting for roomuuid
			if (!gameController.localUserLogin.Equals ("-1")) {
				Debug.Log ("<color=blue>Acquired login: " + gameController.localUserLogin + "</color>");
				gameController.quickSaveData.localUserLogin = gameController.localUserLogin;
				gameController.quickSaveInfo.login = gameController.localUserLogin;

				gameController.networkAgent.sendCommand (0, "playerready:" + gameController.localUserLogin + ":" + "001:");
				state = 0;
			}

		}

		if (state == 2) {
			if(!isWaitingForTaskToComplete) {
				gameController.currentPlayerRole = Family.daughter;
				masterController.startActivity = "ChoosePlayer";
				joinNewGameCanvas.SetActive (false);
				state = 0; // idle this
				notifyFinishTask (); // return to parent task
			}
		}


		// returning back to title
		if (state == 100) {
			fader.fadeOutTask (this); 
			state = 101;
		} else if (state == 101) {
			if (!isWaitingForTaskToComplete) {
				masterController.startActivity = "Title";
				//qrDecoder.stopWebcam ();
				qrDecoder.StopWork();
				notifyFinishTask ();
				state = 0;
			}
		}
	}

	// UI Events callbacks
	public void backArrow() {
		masterController.hardReset ();
		//state = 100;
	}

	// network callbacks

	// called by network 'startgame' command issued by master
	public void startGame(string random, string datetime) {
		gameController.randomChallenge = random;
		gameController.datetimeOfGame = datetime.Replace ("_", " ");
		gameController.datetimeOfGame = gameController.datetimeOfGame.Replace ("!", ":");
		fader.fadeOutTask (this);
		//gameController.network_sendMessage ("play EmpLite");
//		if (masterController.titleController.accountCredits > 0) {
//			masterController.titleController.creditsHUD.text = "Créditos: " + (masterController.titleController.accountCredits - 1);
//		}
		masterController.titleController.buyButton.GetComponent<Button> ().interactable = false;

		state = 2;
		gameController.saveData ();
	}
}
