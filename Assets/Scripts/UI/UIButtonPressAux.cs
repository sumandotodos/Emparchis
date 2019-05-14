using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonPressAux : MonoBehaviour,ButtonPressListener {

	public CommonTestController commonTestController;

	public void buttonPress() {
		commonTestController.OKButton ();
	}
}
