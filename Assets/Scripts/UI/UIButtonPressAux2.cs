﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonPressAux2 : MonoBehaviour, ButtonPressListener {

	public CommonTestController commonTestController;

	public void buttonPress() {

		commonTestController.advanceOKButton ();

	}
}
