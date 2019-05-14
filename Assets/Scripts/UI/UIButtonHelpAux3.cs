using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonHelpAux3 : MonoBehaviour, ButtonPressListener {

	public CommonTestController commonTestController;

	public void buttonPress() {

		commonTestController.helpButtonPress ();

	}
}
