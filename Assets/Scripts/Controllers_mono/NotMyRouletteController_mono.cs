using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NotMyRouletteController_mono : Task {

	public string[] testDescrPrefix;


	public string textPrefix;
	public Text titleText;
	public UITextFader titleFader;
	public UITextFader testDescrText;

	public int receivedMember;
	public int receivedFamily;
	public int receivedMood;

	public GameController_mono gameController;
	public MasterController_mono masterController;

	public SituationChooser situationChooser;

	int targetRole;
	int targetFamily;
	int targetMood;

	public UIMood[] family1MembersAux1;
	public UIMood[] family1MembersAux2;
	public UIMood[] family2MembersAux1;
	public UIMood[] family2MembersAux2;


	public CircleDeploy rouletteRig;
	public CircleDeploy arrowRig;
	public CircleDeploy[] bigIcon;

	public Texture[] aux0Images;

	public RawImage aux0RawImage;
	public Text aux0AnswerText;
	public Text aux1AnswerText;
	public Text aux2AnswerText;
	public Text questionText;

	public CircleDeploy[] judges;
	public CircleDeploy botonaco;

	public GameObject wheel;
	public GameObject arrow;
	public UIOpacityWiggle wheelSelection;
	public UIFaderScript fader;

	public RosettaWrapper rosetta;
	public StringBank attitudeStrings;

	public UIScaleFader helpButtonScaler;
	public UIScaleFader helpPanelScaler;
	public Text helpPanelText;
	public FGTable tablaAyuda;
	public GameObject auxPanel0;
	public GameObject auxPanel1;
	public GameObject auxPanel2;

	public ObjectSpawner emotionSpawner;

	public UIFaderScript interrogationFader;

	public MainGameController_mono mainGameController;

	public Text voteText;

	bool go = false;
	float initialDelay;
	float time;

	public float maxAngSpeed = -20.0f;

	public float angSpeed;
	float angle;
	public float angAccel = 2.0f;
	const float SpeedThreshold = 0.1f;
	public float clampedAngle;
	public float clampledAngle_5;

	bool isNegativeSituation;
	public bool type0Vote = false;

	//[HideInInspector]
	public int selectedItem = -1;

	//RaceConditionVariables
	public bool answerAvailable = false;
	public bool mustFinish = false;

	[HideInInspector]
	public int testVote = -1;

	float initialSpeedSign;

	float finishAngle;
	float T;

	const float delay = 3.0f;

	public int state = 0;
	float timer;

public	int neededVotes;

	bool NSVoted = false;

	float deferredAngle = -2.0f;

	void Start() {
		
	}

	Situation sit;

	public void updateInterrogationText(bool negative, int subType) {
		int tType = gameController.selectedItem;
		if (!negative) { // Positive situation
			if (tType == 0) {
				helpPanelText.text = (string)tablaAyuda.getElement (0, 7);
			}
			if (tType == 1) {
				helpPanelText.text = (string)tablaAyuda.getElement (0, 7);
			}
			if (tType == 2) {
				helpPanelText.text = (string)tablaAyuda.getElement (0, 7);
			}
			if (tType == 3) {
				if (subType == 0) {
					helpPanelText.text = (string)tablaAyuda.getElement (0, 7);
				}
				if (subType == 1) {
					helpPanelText.text = (string)tablaAyuda.getElement (0, 7);
				}
				if (subType == 2) {
					helpPanelText.text = (string)tablaAyuda.getElement (0, 7);
				}
			}
			if (tType == 4) {
				helpPanelText.text = (string)tablaAyuda.getElement (0, 7);
			}
			helpPanelText.text = helpPanelText.text.Replace ("\\n", "\n").Replace("#", ":");
		} 

		else { // negative situacion
			if (tType == 0) {
				helpPanelText.text = (string)tablaAyuda.getElement (0, 0);
			}
			if (tType == 1) {
				helpPanelText.text = (string)tablaAyuda.getElement (0, 1);
			}
			if (tType == 2) {
				helpPanelText.text = (string)tablaAyuda.getElement (0, 2);
			}
			if (tType == 3) {
				if (subType == 0) {
					helpPanelText.text = (string)tablaAyuda.getElement (0, 4);
				}
				if (subType == 1) {
					helpPanelText.text = (string)tablaAyuda.getElement (0, 3);
				}
				if (subType == 2) {
					helpPanelText.text = (string)tablaAyuda.getElement (0, 5);
				}
			}
			if (tType == 4) {
				helpPanelText.text = (string)tablaAyuda.getElement (0, 6);
			}
			helpPanelText.text = helpPanelText.text.Replace ("\\n", "\n").Replace("#", ":");;
		}
	}

	public void cancelNoyMyRoulette() {
		for (int i = 0; i < bigIcon.Length; ++i) {
			bigIcon [i].reset ();
			bigIcon [i].retract ();
		}
	}

	public void startNotMyRoulette(Task w) {
		w.isWaitingForTaskToComplete = true;
		waiter = w;
		type0Vote = false;
		NSVoted = false;
		angSpeed = 0.0f;
		angle = 0.0f;
		state = -1;
		selectedItem = -1;
		testVote = -1;
		fader.fadeIn ();
		voteText.text = "";
		isNegativeSituation = false;
		neededVotes = gameController.nPlayers - 1;
		emotionSpawner.stopSpawning ();
		auxPanel0.SetActive (false);
		auxPanel1.SetActive (false);
		auxPanel2.SetActive (false);
		rouletteRig.gameObject.SetActive (true);
		arrowRig.gameObject.SetActive (true);
		interrogationFader.setFadeValue (0);
		helpPanelScaler.scaleOutImmediately();
		helpButtonScaler.scaleOutImmediately ();
		titleFader.Start ();
		testDescrText.Start ();
		titleFader.reset ();
		testDescrText.reset ();
		for (int i = 0; i < bigIcon.Length; ++i) {
			bigIcon [i].reset ();
			bigIcon [i].retract ();
		}
		for (int i = 0; i < family1MembersAux1.Length; ++i) {
			family1MembersAux1 [i].setEnabled (false);
			family1MembersAux2 [i].setEnabled (false);
		}
		for(int i = 0; i < family2MembersAux1.Length; ++i) {
			family2MembersAux1 [i].setEnabled (false);
			family2MembersAux2 [i].setEnabled (false);
		}
		wheelSelection.reset ();
		rouletteRig.extend ();
		rouletteRig.gameObject.transform.rotation = Quaternion.Euler (Vector3.zero);
		arrowRig.extend ();
		wheelSelection.transform.rotation = Quaternion.Euler (Vector3.zero);

	}

	public void startNSVotation(Task w, int attitude) 
	{
		// 
		w.isWaitingForTaskToComplete = true;
		waiter = w;
		angSpeed = 0.0f;
		angle = 0.0f;
		state = 6;
		selectedItem = gameController.selectedItem;
		titleText.text = rosetta.rosetta.retrieveString (textPrefix, selectedItem);
		titleFader.reset ();
		testDescrText.reset ();
		//titleFader.fadeIn ();
		bigIcon[selectedItem].extend();
		testVote = -1;

		attitudeStrings.rosetta = rosetta.rosetta;
		attitudeStrings.reset ();
		voteText.text = attitudeStrings.getString (attitude);
		interrogationFader.Start ();
		NSVoted = false;Debug.Log("<color=red>fadeOut línea 245</color>");
		interrogationFader.fadeOut ();
		helpButtonScaler.scaleIn ();
		rouletteRig.gameObject.SetActive (false);
		arrowRig.gameObject.SetActive (false);



		isNegativeSituation = true;
		neededVotes = gameController.nPlayers - 1;
		titleFader.fadeOut ();
		testDescrText.fadeOut ();
		for (int i = 0; i < judges.Length; ++i) {
			judges [i].gameObject.GetComponent<UIAnimatedImage> ().reset ();
			judges [i].extend ();
		}

		if (attitude != gameController.NSPlayerAttitude) 
		{
			botonaco.extend ();
			fader.fadeIn ();
		} else {
			fader.setFadeValue (1.0f);
			botonaco.retract ();
		}
	}

	public void turnOffAuxPanels() 
	{
		auxPanel0.SetActive (false);
		auxPanel1.SetActive (false);
		auxPanel2.SetActive (false);
	}
	
	void Update () 
	{
		if (state == -1) { // idling
			if (deferredAngle > -1.0f) {
				finishAngle = deferredAngle;
				T = Mathf.Sqrt ((2 * finishAngle) / angAccel);
				angle = finishAngle - 0.5f * angAccel * (T) * (T);
				wheel.transform.localRotation = Quaternion.Euler (0, 0, -angle);
				deferredAngle = -2.0f;
				state = 1;
			}
		}

		if (state == 0) { // idling

		}

		if (state == 1) { // spinning the wheel
			// do a very simple wheel dynamics update 
			//  (using a 2d rigid body would be overkill, I'm afraid)

			if (timer < T) {
				angle = finishAngle - 0.5f * angAccel * (T - timer) * (T - timer);
				timer += Time.deltaTime;
			} else {
				angle = finishAngle;
				timer = 0.0f;
				state = 2;
				selectedItem = 4-(int)Mathf.Floor ((angle - Mathf.Floor (angle / 360.0f) * 360.0f) / 72.0f);
				if(MasterController_mono.ForceTest != -1) selectedItem = MasterController_mono.ForceTest;
				//if (selectedItem == 0)
				//	selectedItem = 3;
				//if (selectedItem == 2)
				//selectedItem = 4;
				//selectedItem = masterController.testIncremento;
				//if(masterController.testIncremento != 3) masterController.testIncremento = (masterController.testIncremento + 1) % 5;
				mainGameController.tType = selectedItem;

				//if(selectedItem > 1)
				//				selectedItem = 1;
								gameController.selectedItem = selectedItem;

				wheelSelection.transform.Rotate (0, 0, -72.0f * selectedItem);
				wheelSelection.go ();
			}

			clampedAngle = angle - Mathf.Floor (angle / (360.0f / 5.0f)) * (360.0f / 5.0f);

			if ((clampedAngle + 5.0f) < 16.0f) 
			{
				arrow.transform.localRotation = Quaternion.Euler (0, 0, 6.0f * (clampedAngle + 5.0f));
			} else {
				arrow.transform.localRotation = Quaternion.Euler (0, 0, 0);
			}

			wheel.transform.localRotation = Quaternion.Euler (0, 0, -angle);
		}

		if (state == 2) { // small delay
			timer += Time.deltaTime;
			if (timer > delay) {
				rouletteRig.retract ();
				arrowRig.retract ();
				timer = 0.0f;
				state = 3;
			}
		}

		if (state == 3) { // waiting for fadeout
			timer += Time.deltaTime;
			if (timer > delay / 2) {
				titleText.text = rosetta.rosetta.retrieveString (textPrefix, selectedItem);
				testDescrText.GetComponent<Text> ().text = rosetta.rosetta.retrieveString (testDescrPrefix [selectedItem]);
				titleFader.fadeIn ();
				testDescrText.fadeIn ();
				bigIcon [selectedItem].extend ();
				state = 4;
				if (answerAvailable) {
					answerAvailable = false;
					Debug.Log("<color=red>fadeOut línea 355</color>");
					interrogationFader.fadeOut ();
					helpButtonScaler.scaleIn ();
				}
			}
		}

		if (state == 4) { // waiting for network callback
			if(mustFinish) { state = 6; neededVotes = 0; }
		}

		if (state == 6) { // waiting for other players to vote...
			if (gameController.currentTurnVotes >= neededVotes) 
			{
				gameController.updateVoteVariables ();
				fader.fadeOutTask (this);
				botonaco.retract ();
				titleFader.fadeOut ();
				testDescrText.fadeOut ();
				if ((selectedItem > 0) && (selectedItem < bigIcon.Length)) {
					bigIcon [selectedItem].retract ();
				}
				for (int i = 0; i < judges.Length; ++i) {
					judges [i].gameObject.GetComponent<UIAnimatedImage> ().reset ();
					judges [i].retract ();
				}
				gameController.currentTurnVotes = 0; // reset votes count
				state = 10;
			}
		}

		if (state == 10) { // waiting for fadeout
			if(!isWaitingForTaskToComplete) 
			{
				state = 0;
				wheelSelection.transform.Rotate (0, 0, 72.0f * selectedItem);
				wheelSelection.reset ();
				rouletteRig.extend ();
				arrowRig.extend ();
				answerAvailable = false;
				mustFinish = false;
				interrogationFader.fadeIn ();
				helpButtonScaler.scaleOut ();
				notifyFinishTask();
			}
		}
	}

	// network callbacks
	public void setRouletteAngle(float an) 
	{
		deferredAngle = an;
	}

	public void setAnswerText(int table, int row, int q, int c1, int c2, int member, int family, int mood, int _subtype) 
	{
		receivedMember = member;
		receivedMood = mood;
		receivedFamily = family;

		questionText.text = situationChooser.retrieveTextFromTables (table, row, q);
		aux0AnswerText.text = situationChooser.retrieveTextFromTables (table, row, c1);
		if(selectedItem == 0) {
			aux0RawImage.texture = aux0Images [row];
		}
		aux1AnswerText.text = situationChooser.retrieveTextFromTables (table, row, c1);
		aux2AnswerText.text = situationChooser.retrieveTextFromTables (table, row, c1);


		updateInterrogationText (situationChooser.isNegativeSituation (table, row), _subtype);

		// c2 is the column index that contains the emotions string
		if (c2 != -1) {
			string emotions = situationChooser.retrieveTextFromTables (table, row, c2);
			emotionSpawner.emotionString = emotions;

		} else {
			emotionSpawner.emotionString = "";
		}


		targetRole = member;
		// parche para la tabla 1 (sit. padres)
		if(table == 1) targetRole = Random.Range(0, 2) * 2 + 1;
		targetFamily = family;
		targetMood = mood;

		// we now know the test type (selectedItem), activate
		// family members in auxiliary panels
		/*if ((selectedItem == 1) || (selectedItem == 3) || (selectedItem == 4)) {
			int member = FamilyInteger.enumToInt (gameController.currentPlayerRole);

			familyMembersAux1 [member].SetActive (true);
			familyMembersAux2 [member].SetActive (true);
		
		} 
		if (selectedItem == 2) {*/
		for (int i = 0; i < family1MembersAux1.Length; ++i) {
			family1MembersAux1 [i].setEnabled (false);
			family1MembersAux2 [i].setEnabled (false);
		}
		for (int i = 0; i < family2MembersAux1.Length; ++i) {
			family2MembersAux1 [i].setEnabled (false);
			family2MembersAux2 [i].setEnabled (false);
		}
		if (targetFamily == 1) {
			family1MembersAux1 [targetRole].setEnabled (true);
			family1MembersAux2 [targetRole].setEnabled (true);
			family1MembersAux1 [targetRole].setMood (mood);
			family1MembersAux2 [targetRole].setMood (mood);
		} else {
			family2MembersAux1 [targetRole].setEnabled (true);
			family2MembersAux2 [targetRole].setEnabled (true);
			family2MembersAux1 [targetRole].setMood (mood);
			family2MembersAux2 [targetRole].setMood (mood);
		}
		//}
		Debug.Log("<color=red>fadeOut línea 467</color>");
		interrogationFader.fadeOut ();
		helpButtonScaler.scaleIn ();

	}

	public void votation() 
	{
		titleFader.fadeOut ();
		testDescrText.fadeOut ();
		for (int i = 0; i < judges.Length; ++i) {
			judges [i].extend ();
		}

		state = 5;
		type0Vote = false;
	}

	public void finishNotMyRoulette()
	{
		neededVotes = 0; // immediately finish
		//state = 6;
		mustFinish = true;
	}

	public void extendBotocano()
	{
		botonaco.extend ();
		state = 6; // waiting for turn to end
	}

	//public void pullTextureAndString(out Texture tex, out string str) {

		//int i = Random.Range(0, 
	//77
	//}

	// event callbacks
	public void botonacoTouch() 
	{
		if (NSVoted)
			return;
		NSVoted = true;
		gameController.currentTurnVotes++;
		float score = testVote;// / 5.0f;
		gameController.playerList [gameController.playerTurn].blueScoreReceived [gameController.localPlayerN] += score;
		gameController.playerList[gameController.playerTurn].blueVotesReceived[gameController.localPlayerN]++;
		gameController.playerList [gameController.localPlayerN].blueScoreGiven += score;
		gameController.playerList [gameController.localPlayerN].blueVotesGiven++;
		gameController.playerList [gameController.playerTurn].yellowScoreReceived [gameController.localPlayerN] += score;
		gameController.playerList[gameController.playerTurn].yellowVotesReceived[gameController.localPlayerN]++;
		gameController.playerList [gameController.localPlayerN].yellowScoreGiven += score;
		gameController.playerList [gameController.localPlayerN].yellowVotesGiven++;
		gameController.playerList [gameController.playerTurn].greenScoreReceived [gameController.localPlayerN] += score;
		gameController.playerList[gameController.playerTurn].greenVotesReceived[gameController.localPlayerN]++;
		gameController.playerList [gameController.localPlayerN].greenScoreGiven += score;
		gameController.playerList [gameController.localPlayerN].greenVotesGiven++;
		gameController.blueTurnMyValue = score;
		gameController.yellowTurnMyValue = score;
		gameController.greenTurnMyValue = score;

		botonaco.retract ();
	}

	public void interrogationButton() 
	{
		if (selectedItem == 0) {
			auxPanel0.SetActive (true);
		}
		else if (selectedItem == 3) {
			auxPanel2.SetActive (true);
		} else {
			emotionSpawner.startSpawning ();
			auxPanel1.SetActive (true);
		}
	}

	public void helpButtonPress() 
	{
		helpButtonScaler.scaleOut ();
		helpPanelScaler.scaleIn ();
	}

	public void closeHelp() 
	{
		helpPanelScaler.scaleOut ();
		helpButtonScaler.scaleIn ();
	}

	public void closeAux0Button() 
	{
		auxPanel0.SetActive (false);
	}

	public void closeAux1Button() 
	{
		emotionSpawner.stopSpawning ();
		auxPanel1.SetActive (false);
	}

	public void closeAux2Button() 
	{
		auxPanel2.SetActive (false);
	}
}