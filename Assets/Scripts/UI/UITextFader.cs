using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum GameObjectFaderState { fadingIn, fadingOut, transparent, opaque };

public class UITextFader : MonoBehaviour {

	bool started = false;

	/* references */

	Text txt;


	/* properties */

	float opacity;
	GameObjectFaderState state;


	/* public properties */

	public float fadeInSpeed = 6.0f;
	public float fadeOutSpeed = 6.0f;
	public float minOpacity = 0.0f;
	public float maxOpacity = 1.0f;

	public bool startOpaque = false;

	public void reset() {
		if (startOpaque) {
			opacity = maxOpacity;
			state = GameObjectFaderState.transparent;
		} else {
			opacity = minOpacity;
			state = GameObjectFaderState.transparent;
		}
		updateMaterial ();
	}

	// Use this for initialization
	public void Start () {

		if (started)
			return;
		started = true;
		txt = this.GetComponent<Text> ();
		reset ();

	}

	public void setOpacity(float op) {

		opacity = op;
		updateMaterial ();

	}

	public float getFadeValue() {
		return opacity;
	}



	void updateMaterial() {

		if (txt == null)
			return;
		Color newColor = txt.color;
		newColor.a = opacity;
		txt.color = newColor;

	}

	// Update is called once per frame
	void Update () {

		if (state == GameObjectFaderState.transparent) {


		}

		if (state == GameObjectFaderState.fadingIn) {

			opacity += fadeInSpeed * Time.deltaTime;
			if (opacity > maxOpacity) {
				opacity = maxOpacity;
				state = GameObjectFaderState.opaque;
			}
			updateMaterial ();

		}

		if (state == GameObjectFaderState.fadingOut) {

			opacity -= fadeOutSpeed * Time.deltaTime;
			if (opacity < minOpacity) {
				opacity = minOpacity;
				state = GameObjectFaderState.transparent;
			}
			updateMaterial ();

		}

		if (state == GameObjectFaderState.opaque) {

		}

	}

	public void fadeIn() {

		state = GameObjectFaderState.fadingIn;

	}

	public void fadeOut() {

		state = GameObjectFaderState.fadingOut;

	}
}
