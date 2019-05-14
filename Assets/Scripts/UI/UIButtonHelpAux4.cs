using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonHelpAux4 : MonoBehaviour, ButtonPressListener {

	public NotMyRouletteController notMyRoulette;

	public void buttonPress() {

		notMyRoulette.helpButtonPress ();

	}
}
