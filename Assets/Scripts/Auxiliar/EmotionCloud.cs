using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmotionCloud : MonoBehaviour {

	// leave Rosetta here, this will be in a prefab and shall be filled in at runtime
	public Rosetta rosetta;

	public UITextAndImageFader textAndImage;
	public Text text;
	public RawImage icon;

	int state;
	float timer;

	public Texture[] emotionTexture;
	public StringBank emotionText;

	public string[] stringsInBubble;

	public float showingTime = 4.0f;

	// Use this for initialization
	public void Start () {

		state = 1;
		timer = 0.0f;
		textAndImage.Start ();

		int i = Random.Range (0, stringsInBubble.Length);
		string laQueNo = stringsInBubble [i];

		emotionText.rosetta = rosetta;

		int k;

		int match = 0;
		for(k = 0; k<emotionText.nItems(); ++k) {
			string emotionTextk = emotionText.getString (k);
			if (emotionTextk.Equals (stringsInBubble [i])) {
				match = k;
				break;
			}
		}

		if (k < emotionText.nItems ()) {
			icon.texture = emotionTexture [match];
			text.text = emotionText.getString (match);
		} else {
			Destroy (this.gameObject);
			Debug.Log("Emotion not found: " + laQueNo);
			icon.enabled = false;
			text.enabled = false;
		}

	}
	
	// Update is called once per frame
	void Update () {

		if (state == 0) {
			// do nothing
		}
		if (state == 1) {
			textAndImage.fadeIn ();
			state = 2;
		}
		if (state == 2) {
			timer += Time.deltaTime;
			if (timer > (showingTime / 2.0f)) {
				timer = 0.0f;
				state = 3;
			}
		}
		if (state == 3) {
			textAndImage.fadeOut ();
			state = 4;
		}
		if (state == 4) {
			timer += Time.deltaTime;
			if (timer > (showingTime / 2.0f)) {
				timer = 0.0f;
				state = 0;
				Destroy (this.gameObject);
			}
		}

	}
}
