using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CommonTestController : Task {
	
	public int TypeOfTest;

	public RosettaWrapper rosetta;
	public StringBank PSHelpStrings;
	public StringBank NSHelpStrings;

	public SituationChooser situationChooser;

	public UIScaleFader helpButtonScaler;
	public UIScaleFader helpPanelScaler;
	public Text helpPanelText;
	public FGTable tablaAyuda;

	public UIScaleFader helpMediationButtonScaler;
	public UIScaleFader helpPanelMediationScaler;

	public RawImage helpButton;
	public GameObject helpPanel;



	public GameObject type0Test;
	public GameObject type1Test;
	public GameObject type2Test;
	public GameObject type3Test;
	public GameObject type3Aux;
	public GameObject type4Test;

	public GameObject nubarronActivity;

	public GameObject advanceCanvas;
	public Text AdvanceText;
	public GameObject circleAdvance;


	public GameObject test;
	public Text testText;

	public RawImage maskRawImage;
	public Texture[] attitudeMasks;

	public UIFaderScript maskInterrogation;

	public GameObject mask;

	public Texture neutralMaskImage;




	public GameObject notMyRoulette;

	public UIFaderScript faderText;
	public UIFaderScript faderMask;
	public UIFaderScript faderAdvance;

	public Text maskText;
	public StringBank maskStrings;
	public RawImage maskButton;

	public UIScaleFader butoncicoOK;

	public GameObject syncCanvas;

	[HideInInspector]
	public FamilyMember destFamilyMember;

	protected int tType;
	protected int subType;
	protected bool sitNegative;

	public int state = 0;
	public int state2 = 0; // two slots this time
	protected float timer2 = 0.0f;

	protected List<bool> negativeSituationChosenPlayers;
	protected int negativeSituationPickedPlayers;
	protected int chosenPlayer;
	protected int votedPlayers;

	protected List<int> randomAttitude;

	[HideInInspector]
	public int sitTable, sitRow; // These two are used when a master sends a broadcast to start a "all play" situation

	abstract public void OKButton();

	public void startCommonTestActivity(Task w, int type) {

		TypeOfTest = type;

		w.isWaitingForTaskToComplete = true;
		waiter = w;



		tType = type;
		state = 1;
		maskText.text = "";
		maskStrings.rosetta = rosetta.rosetta;
		maskStrings.reset ();
		bReturnValue = false;

		if (tType != GameController.NegativeSituation) {
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
		for (int i = 0; i < GameController.MaxPlayers; ++i) {
			negativeSituationChosenPlayers.Add (false);
		}

		maskText.text = "";
		maskButton.enabled = false;
		butoncicoOK.reset ();
	}

	protected Situation sit;

	protected void updateInterrogationText(bool negative, int subType) {
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



	// called by help Button
	public void helpButtonTouch() {
		helpPanel.SetActive (true);
	}

	public void helpPanelHide() {
		helpPanel.SetActive (false);
	}



	// called by test type 0, after "Avanza X casillas" message is shown
	public void advanceOKButton() {
		if (state == 62) {
			state = 63;
			//AdvanceOKButton.scaleOut ();
		}
	}


	public void helpButtonPress() {
		helpButtonScaler.scaleOut ();
		helpPanelScaler.scaleIn ();
	}

	public void closeHelp() {
		helpPanelScaler.scaleOut ();
		helpButtonScaler.scaleIn ();
	}

	public void helpMediationButtonPress()
	{
		helpMediationButtonScaler.scaleOut ();
		helpPanelMediationScaler.scaleIn ();
	}

	public void closeHelpMediation()
	{
		helpPanelMediationScaler.scaleOut ();
		helpMediationButtonScaler.scaleIn ();
	}


	// network callbacks

	// called by master. Slaves are sitting at <1, 43>
	public void startNS(int attitude) {

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

	public void voteNS(int attitude) {
	}

	public void finishNS() {
		state = 0;
		state2 = 0;
		notMyRoulette.SetActive (false);
		//mask.SetActive (false);
		type3Aux.SetActive(false);
		notifyFinishTask (); // finish commonTestController and return to MainGameController
		// waiting at state == 11, 20 for myTurn & notMyTurn
	}

	public void nsMaskOkButton() { // this must be execute by master only

		if (state2 == 5) { // NotMyTurn
			state2 = 6;
		}
		if (state2 == 11) { // MyTurn
			state2 = 12;
		}
	}



	// end of network callbacks

}
