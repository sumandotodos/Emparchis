using UnityEngine;
using System.Collections;

public class UIButtonPress : MonoBehaviour {

	public MonoBehaviour buttonPressListener_N;

	public float maxScale = 1.0f;
	public float minScale = 0.8f;
	float scale;

	// Use this for initialization
	void Start () {
		scale = 1.0f;
		this.transform.localScale = new Vector3 (maxScale, maxScale, maxScale);
	}

	public void onPress() {
		this.transform.localScale = new Vector3 (minScale, minScale, minScale);
		if (buttonPressListener_N != null) {
			ButtonPressListener bl = (ButtonPressListener)buttonPressListener_N;
			bl.buttonPress ();
		}
	}

	public void onRelease() {
		this.transform.localScale = new Vector3 (maxScale, maxScale, maxScale);
	}


}
