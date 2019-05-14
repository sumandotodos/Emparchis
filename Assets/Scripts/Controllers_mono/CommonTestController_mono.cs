using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CommonTestController_mono : CommonTestController {

	public GameController_mono gameController;

	public GameType gameType;

	public Type0TestController_mono type0TestController;
	public Type1TestController_mono type1TestController;
	public Type2TestController_mono type2TestController;
	public Type3PrimeTestController_mono type3TestController;
	public Type3PrimeAuxController_mono type3AuxController;
	public Type4TestController_mono type4TestController;

	public MiniatureController_mono miniatureController;

	public NubarronController_mono nubarronController;

	public NotMyRouletteController_mono notMyRouletteController;



	int tType;
	int subType;
	bool sitNegative;

	float timer2 = 0.0f;

	List<bool> negativeSituationChosenPlayers;
	int negativeSituationPickedPlayers;
	int chosenPlayer;


	List<int> randomAttitude;

	int scaleOut = 1000;

	new public void startCommonTestActivity(Task w, int type) {

		TypeOfTest = type;

		w.isWaitingForTaskToComplete = true;
		waiter = w;



		tType = type;
		state = 1;
		maskText.text = "";
		maskStrings.rosetta = rosetta.rosetta;
		maskStrings.reset ();
		bReturnValue = false;

		if (tType != GameController_mono.NegativeSituation) {
			helpButton.enabled = false;
		} else
			helpButton.enabled = true;
		
		maskRawImage.texture = neutralMaskImage;
		maskText.text = "";

		maskButton.enabled = false;



		butoncicoOK.scaleOutImmediately (); // hide the butoncico

		faderMask.Start ();
		faderMask.setFadeValue (1.0f);
		faderMask.fadeIn ();

	}

	// Use this for initialization
	void Start () {
		type0Test.SetActive (false);
		type1Test.SetActive (false);
		type2Test.SetActive (false);
		type3Test.SetActive (false);
		type3Aux.SetActive (false);
		type4Test.SetActive (false);
		test.SetActive (false);
		mask.SetActive (false);
		negativeSituationChosenPlayers = new List<bool> ();
		for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			negativeSituationChosenPlayers.Add (false);
		}

		maskText.text = "";
		maskButton.enabled = false;
		butoncicoOK.reset ();
	}

	Situation sit;

	void updateInterrogationText(bool negative, int subType) {
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
	
	void Update () {
	
		if (state == 0) { // idling

		}

		if (state > 0) {
			if (scaleOut>0) {
				if ((tType == 0) || (gameType == GameType.kids)) {
					helpButtonScaler.scaleOutImmediately ();
				}
				scaleOut--;
			}
		}

		if (state == 1) {
			
			sit = situationChooser.chooseSituation (gameController.currentPlayerRole, tType, 0);

			if (situationChooser.isNegativeSituation (sit.table, sit.row))
				sitNegative = true;
			else
				sitNegative = false;

			destFamilyMember = new FamilyMember ();
			destFamilyMember.member = gameController.currentPlayerRole;
			destFamilyMember.family = 1;
			destFamilyMember.mood = Mood.happy;



			iReturnValue = MoodInteger.enumToInt(sit.mood); // mood set from situation. Overriden by test type 2
			// iReturnValue holds MOOD

			if (tType == GameController_mono.Type0Test) {
				type0Test.SetActive (true);
				type0TestController.startType0Test (this);
				state = 2; // wait for this to finish
			}
			if (tType == GameController_mono.Type1Test) {
				type1Test.SetActive (true);
				type1TestController.startType1Test (this);
				destFamilyMember.mood = Mood.thoughtful;
				state = 2; // wait for this to finish
			}
			if (tType == GameController_mono.Type2Test) {
				type2Test.SetActive (true);
				type2TestController.startType2Test (this, sit);
				state = 2; // wait for this to finish
			}
			if (tType == GameController_mono.Type3Test) {
				type3Test.SetActive (true);
				type3TestController.startType3Test (this);
				destFamilyMember.mood = Mood.assertive;
				state = 2; // wait for this to finish
			}
			if (tType == GameController_mono.Type4Test) {
				type4Test.SetActive (true);
				type4TestController.startType4Test (this);
				destFamilyMember.mood = Mood.sad;
				state = 2; // wait for this to finish
			}
			if (tType == GameController_mono.AllPlaySituation) { // this type is only for notMyTurn's
				helpPanelScaler.scaleOutImmediately();
				helpButtonScaler.scaleOutImmediately ();
				if (tType > 0) {
					helpButtonScaler.scaleIn ();
				}
				helpPanelMediationScaler.scaleOutImmediately ();
				helpMediationButtonScaler.scaleOutImmediately ();
				test.SetActive (true);
				faderText.fadeIn ();
				tType = 3; // tType is actually 3
				bool NS = situationChooser.isNegativeSituation (sitTable, sitRow);
				//updateInterrogationText (NS, 2);
				miniatureController.updateMiniature (3, sitTable, NS, sitRow);
				state = 40;

			}
		}

		if (state == 2) { // waiting for header to finish
			if (!isWaitingForTaskToComplete) {
				//if (tType > 0) {
					if (!situationChooser.isNegativeSituation (sit.table, sit.row)) {
						destFamilyMember.mood = Mood.happy;
					}
				//}
				type0Test.SetActive (false);
				type1Test.SetActive (false);
				type2Test.SetActive (false);
				type3Test.SetActive (false);
				type4Test.SetActive (false);
				helpPanelScaler.scaleOutImmediately();
				helpButtonScaler.scaleOutImmediately ();
				if (gameType == GameType.normal) {
					if (tType > 0) {
						helpButtonScaler.scaleIn (); // viva la punta de flecha!
					}
				}
				helpPanelMediationScaler.scaleOutImmediately ();
				helpMediationButtonScaler.scaleOutImmediately ();
				test.SetActive (true);
				//if (tType > 0) {
				miniatureController.updateMiniature (tType, sit.table, situationChooser.isNegativeSituation (sit.table, sit.row), sit.row);
				//} else {
					//miniatureController.updateMiniature (tType, sit.table, true);
				//}
				int extraValue;
				if (tType == 2) {
					extraValue = iReturnValue;
				} else
					extraValue = FamilyInteger.enumToInt (gameController.currentPlayerRole);
				
				gameController.table = sit.table;
				gameController.tType = tType;
				gameController.family = destFamilyMember.family;
				gameController.mood = destFamilyMember.mood;
				gameController.member = destFamilyMember.member;

				if (tType > 0) 
				{
					gameController.isNegative = situationChooser.isNegativeSituation (sit.table, sit.row);
				}

				// GOTO 3: NORMAL SITUATION
				// GOTO 30: SPECIAL K

				// Aquí vamos a decidir si se trata de una situación speciak K o no
				//bool isNegativeSituation = false;
				bool isAllPlaySituation = false;

				subType = 0; 

				// if tType == 3 a new situation is chosen!!
				if (tType == 3) { // only in test #3 we can have the possibility of a neg. situation
					// subType is sittin in iReturnValue
					subType = iReturnValue;
					Debug.Log("<color=blue>Subtype via iReturnValue</color> : " + subType);

					isAllPlaySituation = (subType == 2);
					sit = situationChooser.chooseSituation (gameController.currentPlayerRole, tType, subType);
					while (!situationChooser.isNegativeSituation (sit.table, sit.row)) { // theoretically, this forces the situation to be negative
						sit = situationChooser.chooseSituation (gameController.currentPlayerRole, tType, subType);
					}
					miniatureController.updateMiniature (3, sit.table, situationChooser.isNegativeSituation (sit.table, sit.row), sit.row);
					testText.text = sit.question;
					helpPanelText.text = sit.answer1;

					//isNegativeSituation = situationChooser.isNegativeSituation (sit.table, sit.row);
				}
					
			
				testText.text = sit.question;
				helpPanelText.text = sit.answer1;
				notMyRouletteController.setAnswerText (sit.table, sit.row, sit.qc,
					sit.a1c, sit.a2c, FamilyInteger.enumToInt(destFamilyMember.member), destFamilyMember.family, MoodInteger.enumToInt(destFamilyMember.mood), subType);

				faderText.fadeIn ();

				PSHelpStrings.rosetta = rosetta.rosetta;
				NSHelpStrings.rosetta = rosetta.rosetta;
				bool localNS = situationChooser.isNegativeSituation (sit.table, sit.row);
				//updateInterrogationText (localNS, subType);
				// HERE we know type, subtype (iReturnValue) and ns. Set help text...
				//helpPanelText.text
				
				if (subType == 1) {
					helpPanelMediationScaler.scaleOutImmediately ();
					helpMediationButtonScaler.scaleOutImmediately ();
					helpMediationButtonScaler.scaleIn ();
				}

				if (isAllPlaySituation == true) { // a all play situation, special mechanics
					// tell other players to show test screen
					helpButton.enabled = true;

					state = 30; // wait for OK button to be hit (alt)
				} 

				else { // a single play situation, usual mechanics
					state = 3; // wait for test.OK button to be hit
				}


			}
		}

		if (state == 3) { // waiting for test.OK button to be hit

		}


		// HABRIA QUE COPIAR ESTE ESTADO ENTERO DANDOLE EL VALOR PREDETERMINADO DE 10 (AL DEL TURNO) LOS DEMAS SYNC
		if(state == 600) { // type0: waiting for fadeout after OK was hit
			if (!isWaitingForTaskToComplete) {
				test.SetActive (false);

				nubarronActivity.SetActive (true);
				if (tType == 2 || tType == 4) {
					nubarronController.startNubeActivity (this, 2);
				} else
					nubarronController.startNubeActivity (this, 1);
				//faderMask.fadeIn ();
				state = 601;

			}
		}
		// HABRIA QUE COPIAR ESTE ESTADO ENTERO DANDOLE EL VALOR PREDETERMINADO DE 10 (AL DEL TURNO) LOS DEMAS SYNC
		if(state == 700) { // type0: waiting for fadeout after OK was hit
			if (!isWaitingForTaskToComplete) {
				test.SetActive (false);
				test.SetActive (false);
				nubarronActivity.SetActive (true);
				nubarronController.startNubeActivity (this, 0);
				state = 601;

			}
		}
		if (state == 601) { // wait for nube activity to finish (will finish when votations arrive)
			if (!isWaitingForTaskToComplete) {
				nubarronActivity.SetActive (false);
				AdvanceText.text = "Avanza 10 casillas";
				circleAdvance.SetActive (false);
				advanceCanvas.SetActive (true);
				faderAdvance.fadeIn ();
				state = 62;
			}
		}
		if(state == 60) { // type0: waiting for fadeout after OK was hit
			if (!isWaitingForTaskToComplete) {
				test.SetActive (false);
				nubarronActivity.SetActive (true);
				nubarronController.startNubeActivity (this, 0);
				state = 61;
			}
		}
		if (state == 61) { // wait for nube activity to finish (will finish when votations arrive)
			if (!isWaitingForTaskToComplete) {
				nubarronActivity.SetActive (false);

				int nCasillas = (int)Mathf.Floor (fReturnValue);
				if (nCasillas != 1) {
					AdvanceText.text = "Avanza " +
					nCasillas +
					" casillas";
				} else {
					AdvanceText.text = "Avanza 1 casilla";
				}
				circleAdvance.SetActive (true);
				advanceCanvas.SetActive (true);
				faderAdvance.fadeIn ();
				state = 62;
			}
		}
		if (state == 62) { // wait for ok button to be pressed

		}
		if (state == 63) {
			faderAdvance.fadeOutTask (this);
			state = 64;
		}
		if (state == 64) { // waiting for fadeout
			if (!isWaitingForTaskToComplete) {
				test.SetActive (false);
				advanceCanvas.SetActive (false);
				state = 0;
				notifyFinishTask ();
			}
		}



		if (state == 4) { // type1, 2, 3, 4.. waiting for fadeout after OK was hit
			if (!isWaitingForTaskToComplete) {
				test.SetActive (false);
				nubarronActivity.SetActive (true);
				if (tType == 2 || tType == 4) {
					nubarronController.startNubeActivity (this, 2);
				} else
					nubarronController.startNubeActivity (this, 1);
				//faderMask.fadeIn ();
				state = 5;
			}
		}

		if (state == 5) { // wait for nubarron activity to finish
			if (!isWaitingForTaskToComplete) {
				//faderMask.fadeOutTask (this);
				nubarronActivity.SetActive (false);
				notifyFinishTask();
				state = 0;
			}
		}
		/*
		if(state == 6) { // waiting for fadeout
			if(!isWaitingForTaskToComplete) {
				//mask.SetActive(false);
				state = 0;
				notifyFinishTask(); // return to parent task
			}
		}
		*/


		if (gameController.isMaster) { // only the master must execute slot 2
			if (state2 == 0) {
			}  // idle
			if (state2 == 1) { 
				// clear array used for marking chosen players (do not choose same player twice!)
				for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
					negativeSituationChosenPlayers [i] = false;
				}

				// prepare randomized attitude array (last item must be assertive (3 o MaxPlayers-1)!)
				randomAttitude = new List<int>();
				for (int i = 0; i < gameController.nPlayers - 1; ++i) {
					int r = Random.Range (0, GameController_mono.MaxPlayers - 1);
					while (randomAttitude.Contains (r)) {
						r = Random.Range (0, GameController_mono.MaxPlayers - 1);
					}
					randomAttitude.Add (r);

				}
				randomAttitude.Add (GameController_mono.MaxPlayers - 1); // there's really no need to, but for extra consistency...


				negativeSituationPickedPlayers = 0;

				state2 = 3;
			}
			if (state2 == 3) {
				// the master chooses a player

				if(negativeSituationPickedPlayers < gameController.nPlayers-1) {
					// choose a suitable player at random
					chosenPlayer = Random.Range(0, GameController_mono.MaxPlayers);
					while((chosenPlayer == gameController.playerTurn) || (negativeSituationChosenPlayers[chosenPlayer] == true)
						|| (gameController.playerPresent[chosenPlayer] == false)) {
						chosenPlayer = Random.Range(0, GameController_mono.MaxPlayers);
					}
					negativeSituationChosenPlayers [chosenPlayer] = true;
					++negativeSituationPickedPlayers;
					state2 = 4; // start mask on notMyTurn
				}

				else { 
					chosenPlayer = gameController.playerTurn;
					state2 = 10; // start mask on myTurn
				}

			 
			}
			if (state2 == 4) { 
				// command player to do the mask thing

				state2 = 5;
			}
			if (state2 == 5) { // wait until nmt presses mask.OK button

			}
			if (state2 == 6) { // OK button pressed, to next player
				state2 = 3; // loop back and choose next player
			}
			if (state2 == 10) { // command turn player to do the mask thing
				
				votedPlayers = 0;
				state2 = 11;
			}
			if (state2 == 11) { // wait until remote mt presses mask.OK button
			}

			if(state2 == 12) { // tell everybody to vote attitude # <votedPlayers>
				timer2 = 0.0f;
				state2 = 13;
				//if (votedPlayers == gameController.nPlayers - 1) {
				//	// GameController.MaxPlayers - 1  is the  'Assertive'  one
				//	gameController.networkAgent.broadcast ("nsvote:" + (GameController.MaxPlayers - 1) + ":");
				//	voteNS((GameController.MaxPlayers - 1));
				//} else {
				//	gameController.networkAgent.broadcast ("nsvote:" + votedPlayers + ":");
				//	voteNS (votedPlayers);
				//}
				//state2 = 13;
			}
			if (state2 == 13) { // waiting for notMyRoulette votation to finish
				timer2 += Time.deltaTime;
				if (timer2 > 0.5f) {
					timer2 = 0.0f;
					state2 = 15;
				}
				//if (!isWaitingForTaskToComplete) {
				//	state2 = 14;
				//}
			}
			if (state2 == 14) {
				//++votedPlayers;
				//if (votedPlayers == gameController.nPlayers) {
				//	state2 = 15; // everybody voted
				//} else {
				//	state2 = 12; // loop back to tell everybody to vote #...
				//}
			}
			if (state2 == 15) {
				finishNS ();

			}


		}



		if (state == 30) { // negativesituation, myturn, wait for OK

		}
		if (state == 31) {
			if (!isWaitingForTaskToComplete) {
				syncCanvas.SetActive (true);

				state = 32;
			}
		}
		if (state == 32) { // wait for synch
			if (gameController.synchNumber >= gameController.nPlayers - 1) {
				gameController.synchNumber = 0;
				syncCanvas.SetActive (false);
				//state = 32;
				state = 80;
			}
		}






		if (state == 40) { // negativesituation, notMyTurn, wait for test.OK

		}
		if (state == 41) {
			if (!isWaitingForTaskToComplete) {
				syncCanvas.SetActive (true);	
				state = 42;
			}
		}
		if (state == 42) { // wait for synch
			if (gameController.synchNumber >= gameController.nPlayers - 1) {
				gameController.synchNumber = 0;
				syncCanvas.SetActive (false);
				//state = 42;
				state = 80;
			}
		}




		if (state == 80) {
			test.SetActive (false);
			type3Aux.SetActive (true);
			syncCanvas.SetActive (false);
			type3AuxController.startType3Aux (this);
			state = 81;
		}
		if (state == 81) { // wait until test3aux finishes
			if (!isWaitingForTaskToComplete) {
				finishNS ();
			}
		}

	}

	// event callbacks


	// called by help Button
	public void helpButtonTouch() {
		helpPanel.SetActive (true);
	}

	public void helpPanelHide() {
		helpPanel.SetActive (false);
	}


	// called by Test button (screen where you read the situation text)
	public override void OKButton() {

		Debug.Log ("OKButton called. sitNegative: " + sitNegative);
		
		faderText.fadeOutTask (this);


		// ESTO LO HA PUESTO CARLOS Y NO SE SABE SI FUNCIONARA
		if ((!sitNegative) && ((tType != 3))) // si tType == 3, esto falla (no puede haber situación positiva)
		{
			if (state == 3) // situación normal, pero positiva
			{
				if (tType == 0) { // si es un los que están lejos (hay que votar)
					state = 700; // el 700 tiene que ser como el 60, pero acabando con lo de positivo
				} else { // prueba normal
					state = 600; // el 600 e tiene que ser como el 4, pero acabando con lo de positivo
				}
			}
			if (state == 30) {
				state = 31;
			}
			if (state == 40)
			{
				state = 41;
			}
		} 

		else { // este ELSE está bien
			if (state == 3) { // waiting for OK, normal situation
				if ((1 > 2) && tType == 0) { // los que están lejos, hay que votar
					state = 60; // type 0 mechanics

					notMyRouletteController.type0Vote = true; // must vote on this turn, yes or yes
				} 
				else { // no hay que votar
					state = 4;
				}
			}

			if (state == 30) { // waiting for OK, negativesituation, myturn
				//gameController.networkAgent.broadcast ("synch:");
				state = 31;
			}

			if (state == 40) { // waiting for OK, negativesituation, notMyTurn
				//gameController.networkAgent.broadcast ("synch:");
				state = 41;

			}
		}
	}

	// called by Mask button (negativaSituation test where all players play and vote)
	new public void MaskOKButton() {

		// hay que mandarle este mensaje al Master
		int i;
		i = 9;
		//mask.SetActive(false);



	}

	// called by test type 0, after "Avanza X casillas" message is shown
	new public void advanceOKButton() {
		if (state == 62) {
			state = 63;
			//AdvanceOKButton.scaleOut ();
		}
	}


	new public void helpButtonPress() {
		helpButtonScaler.scaleOut ();
		helpPanelScaler.scaleIn ();
	}

	new public void closeHelp() {
		helpPanelScaler.scaleOut ();
		helpButtonScaler.scaleIn ();
	}

	new public void helpMediationButtonPress()
	{
		helpMediationButtonScaler.scaleOut ();
		helpPanelMediationScaler.scaleIn ();
	}

	new public void closeHelpMediation()
	{
		helpPanelMediationScaler.scaleOut ();
		helpMediationButtonScaler.scaleIn ();
	}


	// network callbacks

	// called by master. Slaves are sitting at <1, 43>
	new public void startNS(int attitude) {
		
		test.SetActive (false);
		mask.SetActive (true);
		butoncicoOK.reset ();
		maskInterrogation.setFadeValue (0.0f);
		maskInterrogation.Start ();
		if (attitude < (attitudeMasks.Length - 1))
			maskInterrogation.fadeOut ();
		maskButton.enabled = true;
		maskRawImage.texture = attitudeMasks [attitude];
		maskText.text = maskStrings.getString (attitude);
	}

	new public void voteNS(int attitude) {
		mask.SetActive (false);
		notMyRoulette.SetActive (true);
		notMyRouletteController.startNSVotation (this, attitude);
	}

	new public void finishNS() {
		state = 0;
		state2 = 0;
		notMyRoulette.SetActive (false);
		//mask.SetActive (false);
		type3Aux.SetActive(false);
		notifyFinishTask (); // finish commonTestController and return to MainGameController
								// waiting at state == 11, 20 for myTurn & notMyTurn
	}

	new public void nsMaskOkButton() { // this must be execute by master only
		
		if (state2 == 5) { // NotMyTurn
			state2 = 6;
		}
		if (state2 == 11) { // MyTurn
			state2 = 12;
		}
	}



	// end of network callbacks

}
