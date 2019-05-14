using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmotionBubble : MonoBehaviour {

	public RawImage icon;
	public Text text;

	public float buoyancy;
	public float wiggleAmplitude;
	public float wiggleFrenquency;

	public Texture[] emotionTexture;
	public StringBank emotionText;

	public string[] stringsInBubble;

	public RosettaWrapper rosetta;

	public int emotionIndex;

	float timer; 

	// Use this for initialization
	public void Start () {

		timer = 0.0f;

		int i = Random.Range (0, stringsInBubble.Length);

		emotionText.rosetta = rosetta.rosetta;

		int k;

		int match = 0;
		for(k = 0; k<emotionText.nItems(); ++k) {
			if (emotionText.getString (k).Equals (stringsInBubble [i])) {
				match = k;
				break;
			}
		}

		if (k < emotionText.nItems ()) {
			icon.texture = emotionTexture [match];
			text.text = emotionText.getString (match);
		} else
			Destroy (this.gameObject);

	}
	
	// Update is called once per frame
	void Update () {

		this.transform.Translate (new Vector3 ((Screen.height / 300.0f) * buoyancy, wiggleAmplitude * (Screen.width / 200.0f) * Mathf.Cos(wiggleFrenquency * timer), 0));

		if (this.transform.position.x > Screen.width * 1.33f) {
			Destroy (this.gameObject);
		}

		timer += Time.deltaTime;

	}
}
