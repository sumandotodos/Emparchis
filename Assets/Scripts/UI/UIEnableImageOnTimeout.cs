using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnableImageOnTimeout : MonoBehaviour {

	public float timeout;
	float remainingTime;

	public RawImage theImage;
	public RawImage cubreTodo;
	public Text text;

	public bool going = false;
	float time;
	bool started;

	public void Start () 
	{
		if (started)
			return;
		started = true;
		remainingTime = timeout;
		//theImage = this.GetComponent<RawImage> ();
		theImage.enabled = false;
		cubreTodo.gameObject.SetActive (false);
		going = false;
		//text = this.GetComponentInChildren<Text> ();
		//text.enabled = false;
		text.gameObject.SetActive (false);
	}
	
	void Update ()
	{
		if (!going)
			return;

		if (remainingTime > 0.0f) {

			remainingTime -= Time.deltaTime;
			if (remainingTime <= 0.0f)
			{
				theImage.enabled = true;
				cubreTodo.gameObject.SetActive (true);
				text.enabled = true;
				text.gameObject.SetActive (true);
				text.GetComponent<UITextBlinker> ().startBlink ();
			}
		}
	}

	public void go() {
		going = true;
		remainingTime = timeout;
	}

	public void stop() {
		going = false;
		keepAlive ();
	}

	public void keepAlive() {
		
		if (theImage != null) {
			theImage.enabled = false;
			cubreTodo.gameObject.SetActive (false);
		}
		text.GetComponent<UITextBlinker> ().disable ();
		text.GetComponent<UITextBlinker> ().stopBlink ();
		text.gameObject.SetActive(false);
		//text.enabled = false;
		remainingTime = timeout;
	}
}
