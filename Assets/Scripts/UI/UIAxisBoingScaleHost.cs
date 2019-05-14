using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAxisBoingScaleHost : MonoBehaviour {

	public UIAxisScaleBoing[] axisBoings;

	public void go() {
		for (int i = 0; i < axisBoings.Length; ++i) {
			axisBoings [i].go ();
		}
	}

	public void reset() {
		for (int i = 0; i < axisBoings.Length; ++i) {
			axisBoings [i].reset ();
		}
	}
}
