using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleController_mono : Task {

	public InputField playcodeInput;

	public GameObject buyButton;
	public Text creditsHUD;

	public MasterController_mono masterController;
	public GameController_mono gameController;

	public AudioClip buttonClickClip;
	public AudioClip giftClip;
	public AudioClip diceClip;

	public GameObject IAPCanvas;

	public UIFaderScript whiteCover;

	public GameObject rootMenu;
	public GameObject signInMenu;
	public GameObject sessionMenu;
	public GameObject sessionMenu2;
	public GameObject sessionMenu2Alt;
	public GameObject noMagicMenu;
	public GameObject registroMenu;
	public GameObject initialMenu;
	public GameObject recoveryMenu;

	public UITextFader versionText;

	public UIScaleFader continueGamePanel;
	public UIScaleFader noMoarCreditsPanel;
	public UIScaleFader noMoarMagicPanel;

	public RawImage goBackButton;
	public GameObject instructionsButton;
	public UIScaleFader instructionsPanel;
	public string PDFLink;
	public string VideoLink;

	public GameObject passwordDoNotMatch;
	public GameObject passwordCantBeEmpty;

	public GameObject checkMailNotice;
	public GameObject loginIncorrectText;
	public GameObject serverUnreachableText;

	public InputField loginUser;
	public InputField loginPasswd;

	public InputField newMagicInput;

	public InputField newUser;
	public InputField passwd1;
	public InputField passwd2;
	public InputField magicField;

	public Text loggedInUserText;
	public Text loggedInUserAltText;
	public Text loggedInUserTextbis;
	public Text loggedInUserTexttris;

	public UIHighlight diceButton;
	public UIHighlight giftButton;

	public int accountCredits;

	public GameObject taccanvas;

	int newGameType = 0; // 0 : create    1 : join

	public int state0;
	float timer0;

	public int substate0;

	bool continueGame = false;

	WWW www;

	const float TitleDelay = 1.0f;

	public UIFaderScript fader;

	public void stop() {
		state0 = 0;
		substate0 = 0;
		timer0 = 0.0f;
		cancel ();
	}

	const int playcodeYear = 2018;
	const int playcodeMonth = 7;
	const int playcodeDay = 20;

	bool freePlay = false;

	void reset() {
		state0 = 1;
		substate0 = 2;
		timer0 = 0.0f;
		rootMenu.SetActive (false);
		signInMenu.SetActive (false);
		sessionMenu.SetActive (false);
		sessionMenu2.SetActive (false);
		sessionMenu2Alt.SetActive (false);
		registroMenu.SetActive (false);
		recoveryMenu.SetActive (false);
		checkMailNotice.SetActive (false);
		instructionsButton.SetActive (false);
		versionText.setOpacity (0.0f);
		giftButton.unpress ();
		diceButton.unpress ();
		continueGamePanel.Start ();
		continueGamePanel.scaleOutImmediately ();

		continueGame = gameController.checkQuickSaveInfo ();
		System.DateTime dt = System.DateTime.Now;
		System.DateTime pcdt = new System.DateTime (playcodeYear, playcodeMonth, playcodeDay, 0, 0, 0);
		if (dt.CompareTo (pcdt) <= 0) {
			playcodeInput.text = CorrectPlayCode;
			playcodeInput.gameObject.SetActive (false);
			freePlay = true;
		} else {
			playcodeInput.text = gameController.quickSaveInfo.playcode;
            //playcodeInput.gameObject.SetActive (true);

            //freePlay = false;
            playcodeInput.gameObject.SetActive(false);
            freePlay = true;
		}


		fader.fadeIn ();
		instructionsPanel.Start ();
		instructionsPanel.scaleOutImmediately ();
		instructionsPanel.gameObject.SetActive (false);
		creditsHUD.text = "Créditos: ";
		buyButton.GetComponent<Button> ().interactable = false;
	}

	const string CorrectPlayCode = "pensarenelotro108";

	public void startTitleActivity(Task w) {
		w.isWaitingForTaskToComplete = true;
		waiter = w;
		IAPCanvas.SetActive (false);
		//whiteCover.setFadeValue (1.0f);
		reset ();
	}

	public void updateCreditsHUD() {

		if (accountCredits >= 0) {
			creditsHUD.text = "Créditos: " + accountCredits;
			buyButton.GetComponent<Button> ().interactable = true;
		} else if (accountCredits == -1) {
			creditsHUD.text = "Créditos: ∞";
			buyButton.GetComponent<Button> ().interactable = false;
		} else if (accountCredits == -2) {
			creditsHUD.text = "Créditos: ?";
			buyButton.GetComponent<Button> ().interactable = false;
		}

	}


	void Start() {
		state0 = 0;
		passwordDoNotMatch.SetActive (false);
		passwordCantBeEmpty.SetActive (false);
	}
	
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Escape))
			goBackCallback ();

		if (state0 == 0) { // idling

			return;

		}

		// end of state0 == 0

		else if (state0 == 1) { // State 0: FlyGames logo

			if (substate0 == 0) { // showing the logo

			}

			if (substate0 == 2) 
			{
				timer0 = 0.0f;

				fader.fadeIn ();

				initialMenu.SetActive (true);
				rootMenu.SetActive (false);
				noMagicMenu.SetActive (false);
				goBackButton.enabled = false;
				instructionsButton.SetActive (true);

				state0 = 2;
				substate0 = 0;
			}
		}

		// end state0 == 1

		else if (state0 == 2) { // State 1: showing Title

			if (substate0 == 0) {
				timer0 += Time.deltaTime;
				if (timer0 > TitleDelay) {
					substate0 = 1;
					whiteCover.fadeIn (); // show enabled menu
					timer0 = 0.0f;
				}
			} else if (substate0 == 1) { // delay a bit

				timer0 += Time.deltaTime;
				if (timer0 > 0.33f) {
					substate0 = 4000; // wait for user input...
					versionText.fadeIn ();
				}

			} else if (substate0 == 4000) { // waiting for user input via callbacks

				// do nothing

			} else if (substate0 == 20) { // initial menu
				if (!isWaitingForTaskToComplete) {

					initialMenu.SetActive (false);
					goBackButton.enabled = true;

					gameController.loadData ();
					gameController.localUserLogin = "";

					substate0 = 300;
//					if (!gameController.getUserLogin ().Equals ("")) {
//						// send data to server
//						WWWForm wwwForm = new WWWForm ();
//						wwwForm.AddField ("email", gameController.localUserEMail);
//						wwwForm.AddField ("passwd", gameController.getUserPass ());
//						wwwForm.AddField ("app", "EmpLite");
//						loggedInUserTexttris.text = gameController.localUserEMail;
//
//						www = new WWW (gameController.networkAgent.bootstrapData.loginServer + ":" + gameController.networkAgent.bootstrapData.loginServerPort + Utils.CheckUserScript, wwwForm);
//						substate0 = 300; // wait for www to get result data
//					} else {
//						whiteCover.fadeIn ();
//						rootMenu.SetActive (true);
//						substate0 = 1;
//					}
				}
			} else if (substate0 == 30) { // clicked on gift... wait for fadeout and start gallery activity
				if (!isWaitingForTaskToComplete) {
					state0 = 0; // idle this
					masterController.startActivity = "Gallery";

					notifyFinishTask (); // return to parent task
					buyButton.GetComponent<Button> ().interactable = false;
				}
			} else if (substate0 == 2) { // register menu

				if (isWaitingForTaskToComplete)
					return;
				// disable root menu and enable registry menu
				rootMenu.SetActive (false);
				registroMenu.SetActive (true);
				instructionsButton.SetActive (false);
				goBackButton.enabled = true;
				whiteCover.fadeIn ();
				substate0 = 3;

			} else if (substate0 == 4) { // sign in menu

				if (isWaitingForTaskToComplete)
					return;
				// disable root menu and enable registry menu
				rootMenu.SetActive (false);
				loginIncorrectText.SetActive (false);
				serverUnreachableText.SetActive (false);
				instructionsButton.SetActive (false);
				signInMenu.SetActive (true);
				goBackButton.enabled = true;
				whiteCover.fadeIn ();
				substate0 = 5;
			} 


			// new user submittal
			else if (substate0 == 100) { // waiting for www to return data from new user submittal
				if (www.isDone) {
					whiteCover.fadeOutTask (this);
					timer0 = 0.0f;
					substate0 = 0;
					substate0 = 101;
				}

			} else if (substate0 == 101) { // www has returned data, wait for fadeout
				if (isWaitingForTaskToComplete)
					return;
				registroMenu.SetActive (false);
				checkMailNotice.SetActive (true);
				goBackButton.enabled = false;
				whiteCover.fadeIn ();
				substate0 = 102;
			} else if (substate0 == 102) {
				timer0 += Time.deltaTime;
				if ((timer0 > TitleDelay * 4) || Input.GetMouseButtonDown (0)) {
					whiteCover.fadeOutTask (this);
					substate0 = 103;
				}
			} else if (substate0 == 103) { // wait for fadeout

				if (isWaitingForTaskToComplete)
					return;
				timer0 = 0.0f; // go back to root menu

				fader.fadeIn ();
				whiteCover.enabled = true;
				whiteCover.setFadeValue (1.0f);
				checkMailNotice.SetActive (false);
				diceButton.unpress ();
				giftButton.unpress ();
				initialMenu.SetActive (true);
				instructionsButton.SetActive (true);
				substate0 = 0;
			} 


			//
			else if (substate0 == 200) { // waiting for www to return user id confirm data (manual input)
				if (www.isDone) {

					if (www.text.Equals ("")) {

						serverUnreachableText.SetActive (true);
						substate0 = 1;
					} else {
						string[] field = www.text.Split (':');

						if (field.Length == 2) {

							int credits;
							int.TryParse (field [1], out credits);
							accountCredits = 10;//credits;
							updateCreditsHUD ();

							if (accountCredits == -2) { // special meaning
								newMagicInput.text = "";
								signInMenu.SetActive (false);
								instructionsButton.SetActive (false);
								noMagicMenu.SetActive (true);
								noMoarMagicPanel.Start ();
								noMoarMagicPanel.scaleIn ();
								gameController.localUserEMail = loginUser.text;
								gameController.setUserPass (loginPasswd.text);
								//gameController.saveData ();
								state0 = 0;
								return;
							}

							int userUUID;
							int.TryParse (field [0], out userUUID);
							if (www.text.Equals ("")) { // server unreachable
								serverUnreachableText.SetActive (true);
								substate0 = 1; // return to user input polling
							} else {
								if (userUUID > -1) {
							
									loginIncorrectText.SetActive (false);
									serverUnreachableText.SetActive (false);
									whiteCover.fadeOutTask (this);
									timer0 = 0.0f;
									substate0 = 0;
									substate0 = 201;
									//gameController.setUserLogin ("" + userUUID);
									//gameController.localUserEMail = loginUser.text;
									//gameController.setUserPass (loginPasswd.text);
									//gameController.saveData ();
								} else {
									loginIncorrectText.SetActive (true);
									substate0 = 1; // return to user input polling
								}
							}

						} else {
							loginIncorrectText.SetActive (true);
							substate0 = 1; // return to user input polling
						}
					}
				}

			} else if (substate0 == 201) { // waiting for fadeout from substate0 = 200

//				if (isWaitingForTaskToComplete)
//					return;
				rootMenu.SetActive(false);
				goBackButton.enabled = true;
				sessionMenu.SetActive (true);
				instructionsButton.SetActive (true);
//				loggedInUserText.text = gameController.localUserEMail;
//				loggedInUserAltText.text = gameController.localUserEMail;
//				loggedInUserTextbis.text = gameController.localUserEMail;
//				loggedInUserTexttris.text = gameController.localUserEMail;
				signInMenu.SetActive (false);
				whiteCover.fadeIn ();
				substate0 = 1;

			} else if (substate0 == 300) { // waiting for www to return user id confirm data (stored)
//				if (www.isDone) {
//
//					string[] field = www.text.Split (':');
//
//					if (field.Length == 2) {
//
//						int credits;
//						int.TryParse (field [1], out credits);
//						accountCredits = 10;//credits;
//						updateCreditsHUD ();
//
//						if (accountCredits == -2) { // special meaning
//							newMagicInput.text = "";
//							signInMenu.SetActive (false);
//							instructionsButton.SetActive (false);
//							noMagicMenu.SetActive (true);
//							whiteCover.fadeIn ();
//							noMoarMagicPanel.Start ();
//							noMoarMagicPanel.scaleIn ();
//							state0 = 0;
//							return;
//						}
//
//						int userUUID;
//						int.TryParse (field[0], out userUUID);
//						if (userUUID > -1) {
//							gameController.localUserLogin = field [0];
//							loginIncorrectText.SetActive (false);
//							serverUnreachableText.SetActive (false);
//							loggedInUserText.enabled = true;
//							loggedInUserText.text = gameController.localUserEMail;
//							loggedInUserTexttris.text = gameController.localUserEMail;
//							loggedInUserAltText.enabled = true;
//							loggedInUserAltText.text = gameController.localUserEMail;
//							rootMenu.SetActive (false);
//							instructionsButton.SetActive (false);
//							timer0 = 0.0f;
//							//substate0 = 0;
//							substate0 = 201;
//
//						} else {
//							//signInMenu.SetActive (true);
//							rootMenu.SetActive (true);
//							instructionsButton.SetActive (true);
//							whiteCover.fadeIn ();
//							//loginIncorrectText.SetActive (true);
//							substate0 = 1; // return to user input polling
//						}
//
//					} else {
//						//signInMenu.SetActive (true);
//						rootMenu.SetActive (true);
//						instructionsButton.SetActive (true);
//						whiteCover.fadeIn ();
//						//loginIncorrectText.SetActive (true);
//						substate0 = 1; // return to user input polling
//					}
//				}
				substate0 = 201;

			} else if (substate0 == 500) { // waiting for fadeout in order to go back to substate 0

				if (isWaitingForTaskToComplete)
					return;
				timer0 = TitleDelay;
				noMagicMenu.SetActive (false);
				loginIncorrectText.SetActive (false);
				serverUnreachableText.SetActive (false);
				signInMenu.SetActive (false);

				registroMenu.SetActive (false);

				goBackButton.enabled = true;
				fader.fadeIn ();
				whiteCover.enabled = true;
				whiteCover.setFadeValue (1.0f);
				signInMenu.SetActive (false);
				sessionMenu.SetActive (false);
				rootMenu.SetActive (true);
				instructionsButton.SetActive (true);
				substate0 = 0;

			} else if (substate0 == 600) { // waiting for fade to black in order to start a new game

				if (isWaitingForTaskToComplete)
					return;
				masterController.startActivity = "StartNewGame";
				diceButton.unpress ();
				state0 = 0;
				notifyFinishTask ();
				buyButton.GetComponent<Button> ().interactable = false;

			} else if (substate0 == 700) { // waiting for fade to black in order to start a new game

				if (isWaitingForTaskToComplete)
					return;
				masterController.startActivity = "JoinNewGame";
				diceButton.unpress ();
				state0 = 0;
				notifyFinishTask ();
				buyButton.GetComponent<Button> ().interactable = false;

			} else if (substate0 == 900) { // hit password recovery button
				whiteCover.fadeOutTask (this);
				substate0 = 901;
			} else if (substate0 == 901) { // wait for fadeout and show recovery message
				if (!isWaitingForTaskToComplete) {
					signInMenu.SetActive (false);
					goBackButton.enabled = false;
					recoveryMenu.SetActive (true);
					goBackButton.enabled = true;
					instructionsButton.SetActive (false);
					whiteCover.fadeIn ();
					timer0 = 0.0f;
					substate0 = 902;
				}
			} else if (substate0 == 902) {
				timer0 += Time.deltaTime;
				if (timer0 > 5.0f) {
					whiteCover.fadeOutTask (this);
					substate0 = 903;
				}
				if (Input.GetMouseButton (0)) {
					timer0 = 5.0f;
				}
			} else if (substate0 == 903) {
				if (!isWaitingForTaskToComplete) {
					recoveryMenu.SetActive (false);
					rootMenu.SetActive (true);
					instructionsButton.SetActive (true);
					goBackButton.enabled = true;
					whiteCover.fadeIn ();
					substate0 = 4000; // wait for events...
				}
			}



			//
			else if (substate0 == 3000) { // continue game
				fader.fadeOutTask (this);
				substate0 = 3001;
			} else if (substate0 == 3001) {
				if (!isWaitingForTaskToComplete) {
					masterController.startActivity = "ContinueGame";
					notifyFinishTask ();
					buyButton.GetComponent<Button> ().interactable = false;
				}
			}


			//
			else if (substate0 == 8000) {
				whiteCover.fadeOutTask (this);
				substate0 = 8001;
			} else if (substate0 == 8001) {
				if (!isWaitingForTaskToComplete) {
					rootMenu.SetActive (false);
					sessionMenu.SetActive (false);
					initialMenu.SetActive (true);
					instructionsButton.SetActive (true);
					goBackButton.enabled = false;
					state0 = 2; // go back to gift/dice state
					substate0 = 1;
					giftButton.unpress ();
					diceButton.unpress ();
					whiteCover.fadeIn ();
				}
			}



		}

	}


	/*
	 * 
	 * OnClick event handlers
	 * 
	 */
	public void ClickOnPDF()
	{
		Application.OpenURL (PDFLink);
	}

	public void ClickOnVideo()
	{
		Application.OpenURL (VideoLink);
	}

	public void clickOnInfo()
	{
		instructionsPanel.gameObject.SetActive (true);
		instructionsPanel.Start ();
		instructionsPanel.scaleIn ();
	}

	public void clickOnCloseInfo()
	{
		instructionsPanel.scaleOut ();
	}

	public void newGameCallback() {

		if (!freePlay) {
			if (playcodeInput.text != CorrectPlayCode)
				return;

			gameController.quickSaveInfo.playcode = playcodeInput.text;
			gameController.saveQuickSaveInfo ();
		}

		masterController.playSound (buttonClickClip);

		goBackButton.enabled = true;

		createGameCallback();

	}


	public void newUserCallback() {

	}


	public void registerCallback() {

		masterController.playSound (buttonClickClip);

		whiteCover.fadeOutTask (this);
		passwordDoNotMatch.SetActive(false);
		passwordCantBeEmpty.SetActive(false);
		substate0 = 2;
	}


	public void signInCallback() {

		masterController.playSound (buttonClickClip);

		whiteCover.fadeOutTask (this);
		substate0 = 4;
	}


	public void passwdRecoveryCallback() {

		masterController.playSound (buttonClickClip);


	}


	public void logoutCallback() {

		noMoarMagicPanel.scaleOut ();

		creditsHUD.text = "Créditos: ";
		buyButton.GetComponent<Button> ().interactable = false;

		loginPasswd.text = "";

		
		masterController.playSound (buttonClickClip);

		whiteCover.fadeOutTask (this);
		substate0 = 500;
		state0 = 2;
		gameController.setUserLogin ("");
		gameController.setUserPass ("");
		//gameController.saveData ();
	}


	public void submitUserPassCallback() {

		masterController.playSound (buttonClickClip);
	
		substate0 = 200;
	}


	public void createGameCallbackWithConfirmation()
	{

		//if (accountCredits != 0) {

			masterController.playSound (buttonClickClip);

			continueGamePanel.scaleIn ();

			newGameType = 0;
		//} else {
		//	noMoarCreditsPanel.scaleIn ();
		//}
	}

	public void createGameCallback()
	{

		//if (accountCredits != 0) {
			
			masterController.playSound (buttonClickClip);

			substate0 = 600; // waiting for fadeout
			fader.fadeOutTask (this);

		//} else {
		//	noMoarCreditsPanel.scaleIn ();
		//}
	}

	public void clickOnNewMagic() {
		string user = gameController.localUserEMail;
		string pass = gameController.localUserPass;
		string newMagic = newMagicInput.text;
		WWWForm wwwForm = new WWWForm();
		wwwForm.AddField("email", user);
		wwwForm.AddField ("passwd", pass);
		wwwForm.AddField ("magic", newMagic);

		while (!www.isDone) {
		} // oh, no, you don't!!!

		noMoarMagicPanel.scaleOut ();
		newMagicInput.text = "";
		masterController.state = 666;
	}

	public void continueGameCallback() 
	{
		masterController.playSound (buttonClickClip);

		substate0 = 3000; // waiting for fadeout
		fader.fadeOutTask (this);
	}

	public void confirmCallback()
	{
		gameController.resetQuickSaveInfo ();
		if (newGameType == 0) {
			substate0 = 600; // waiting for fadeout
			fader.fadeOutTask (this);
		} else {
			substate0 = 700; // waiting for fadeout
			fader.fadeOutTask (this);
		}

	}

	public void joinGameCallbackWithConfirmation() {

		//if (accountCredits != 0) {

			masterController.playSound (buttonClickClip);

			continueGamePanel.scaleIn ();


			newGameType = 1;
			//substate0 = 700; // waiting for fadeout
			//fader.fadeOutTask (this);
//		} else {
//			noMoarCreditsPanel.scaleIn ();
//		}

	}

	public void joinGameCallback() {

		//if (accountCredits != 0) {
			masterController.playSound (buttonClickClip);

			substate0 = 700; // waiting for fadeout
			fader.fadeOutTask (this);
//		}
//		else {
//			noMoarCreditsPanel.scaleIn ();
//		}

	}


	public void goBackCallback() {

//		if (goBackButton.enabled == false)
//			return;

		if (sessionMenu.activeSelf == true) {
			substate0 = 8000; // go back to initial menu
			return;
		}

		if (continueGame) {
			if (sessionMenu2.activeSelf == true) {
				sessionMenu2.SetActive (false);
				sessionMenu.SetActive (true);
				goBackButton.enabled = true;
			} 
			else if (sessionMenu2Alt.activeSelf == true) {
				sessionMenu2Alt.SetActive (false);
				sessionMenu.SetActive (true);
				goBackButton.enabled = true;
			}
			else {
				whiteCover.fadeOutTask (this);
				substate0 = 500;
			}
		} else {
			if (sessionMenu2.activeSelf == true) {
				sessionMenu2.SetActive (false);
				sessionMenu.SetActive (true);
				goBackButton.enabled = true;
			} 
			else if (sessionMenu2Alt.activeSelf == true) {
				sessionMenu2Alt.SetActive (false);
				sessionMenu.SetActive (true);
				goBackButton.enabled = true;
			}
			else {
				whiteCover.fadeOutTask (this);
				substate0 = 500;
			}
		}



	}

	public void submitNewUserCallback() {

		masterController.playSound (buttonClickClip);

		if (!passwd1.text.Equals (passwd2.text)) {
			passwordDoNotMatch.SetActive(true);
			return;
		} 

		if (passwd1.text.Length == 0) {
			passwordCantBeEmpty.SetActive(true);
			return;
		}
		if (newUser.text.Length == 0) {
			passwordCantBeEmpty.SetActive(true);
			return;
		}

		else {

			passwordDoNotMatch.SetActive(false);
			passwordCantBeEmpty.SetActive(false);
			whiteCover.fadeOutTask (this);

			substate0 = 100;

		}

	}


	// touch event callbacks
	public void touchOnGift() {

		masterController.playSound (giftClip);

		substate0 = 30;
		fader.fadeOutTask (this);
	}

	public void touchOnDice() {

		masterController.playSound (diceClip);
		substate0 = 20;
		whiteCover.fadeOutTask (this);

	}

	public void clickOnBuy() {
		noMoarCreditsPanel.scaleOut ();
		IAPCanvas.SetActive (true);
	}

	public void cancelIAP() {
		IAPCanvas.SetActive (false);
	}

	public void touchOnPasswordRecovery() {
		masterController.playSound (buttonClickClip);

		substate0 = 900; // show password recovery screen
	}

	public void tacOKButton() {
		if (taccanvas.activeSelf == true)
			taccanvas.SetActive (false);
		else
			taccanvas.SetActive (true);
	}
}
