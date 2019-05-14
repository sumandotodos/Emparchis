using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmocionarioController : Task {

	public GameObject emocionarioCanvas;

	public UIFaderScript fader;
	//public UIFaderScript ticketFader;

	public Text descriptionText;
	public UITextFader descriptionFader;

	public UIScroller scroller;

	public UIScaleFader okButton;

	public FGTable table;
	public Text[] cloudText;

	string nextText;

	float delay = 0.25f;
	float timer = 0.0f;

	int state;

	bool started = false;

	// Use this for initialization
	public void Start () {
		if (started)
			return;
		started = true;
		state = 0;
		okButton.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		if (state == 0) { // stopped

		}
		if (state == 100) { // waiting for fadeout
			if (!isWaitingForTaskToComplete) {
				state = 101;
			}
		}
		if (state == 101) {
			emocionarioCanvas.SetActive (false);
			//notifyFinishTask ();

			state = 0;
		}
		if (state == 200) { //  waiting for text to disappear
			if(descriptionFader.getFadeValue() == 0.0f) {
				descriptionText.text = nextText;
				descriptionFader.fadeIn ();
				timer = 0.0f;
				state = 0;
			}
		}


	}

	public void startEmocionario() {
		Start ();
		descriptionFader.Start ();
		okButton.scaleIn ();
		for (int i = 0; i < cloudText.Length; ++i) {
			cloudText [i].text = (string)table.getElement (0, i);
		}

		//w.isWaitingForTaskToComplete = true;
		//waiter = w;
		emocionarioCanvas.SetActive (true);
		descriptionText.text = "";
		fader.fadeIn ();
		scroller.initialize ();
		state = 0;
	}

	public void okButtonPress() {
		fader.fadeOutTask (this);
		state = 100;
	}

	public void setText(int id) {
		nextText = (string)table.getElement (1, id);
		descriptionFader.fadeOut ();
		timer = 0.0f;
		state = 200;
	}
}
