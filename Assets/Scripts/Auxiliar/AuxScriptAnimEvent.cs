using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuxScriptAnimEvent : MonoBehaviour {

	public Type2TestController type2Controller;
	public Type2TestController_mono type2ControllerMono;

	public void changeEvent() {
		if (type2Controller != null) {
			type2Controller.changeRoleEvent ();
		}
		if (type2ControllerMono != null) {
			type2ControllerMono.changeRoleEvent ();
		}
	}
}
