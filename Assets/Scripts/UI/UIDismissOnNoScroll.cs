using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDismissOnNoScroll : MonoBehaviour {

	public CommonTestController commonTestController;

	public bool dismiss = true;

	public bool touching = false;

	public Vector3 touchCoords;


	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Escape)) {
			commonTestController.helpPanelHide ();
		}

		if (!touching) {
			if (Input.GetMouseButtonDown (0)) {
				dismiss = true;
				touchCoords = Input.mousePosition;
				touching = true;
			}
		}

		if (touching) {
			if ((Input.mousePosition - touchCoords).magnitude > 5.0f) {
				dismiss = false;
			}

			if (Input.GetMouseButtonUp (0)) {
				touching = false;
				if (dismiss) {
					commonTestController.helpPanelHide (); // close panel if we did not scroll the view
				}
			}
		}

	}
}
