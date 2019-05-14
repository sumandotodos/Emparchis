using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonHelpAux2 : MonoBehaviour, ButtonPressListener {

	public VotationController votationController;
	public UIScaleFader selfScaleFader;

	public void buttonPress() {

		votationController.helpPanelScaler.scaleIn ();
		selfScaleFader.scaleOut ();

	}
}
