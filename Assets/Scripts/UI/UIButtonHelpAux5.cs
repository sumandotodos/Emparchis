using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonHelpAux5 : MonoBehaviour, ButtonPressListener {

	public CommonTestController commonTestController;

	public void buttonPress() {

		commonTestController.helpMediationButtonPress ();

	}
}
