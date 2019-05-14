using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CreateNewGameController_mono : Task {

	public MasterController_mono masterController;

	public GameController_mono gameController;

	public GameObject createNewGameCanvas;

	public RosettaWrapper rosetta;
	public StringBank generalStringBank;

	public RawImage qrResults;
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

	List<string> seenPlayers;

	public void init() {
		seenPlayers = new List<string>();
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

			state = 5;

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



	public void addPlayer(string playerId, string compatCode) 
	{
		

		if (!gameController.isMaster)
			return;
		playerId = playerId.ToLower ();



		int myCompat, playerCompat;



		++nPlayers;
		seenPlayers.Add (playerId);
		gameController.nPlayers = nPlayers;

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

		state = 5;
		gameController.saveData ();
	}
}
