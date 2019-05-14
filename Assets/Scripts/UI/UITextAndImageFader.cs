using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITextAndImageFader : MonoBehaviour {

	public UIFaderScript imageFader_N;
	public UIFaderScript iconFader_N;
	public UITextFader textFader_N;


	public void fadeIn() {

		if(textFader_N != null) textFader_N.fadeIn ();
		if(imageFader_N != null) imageFader_N.fadeOut ();
		if(iconFader_N != null) iconFader_N.fadeOut ();

	}


	public void fadeOut() {

		if(textFader_N != null) textFader_N.fadeOut ();
		if(imageFader_N != null)imageFader_N.fadeIn ();
		if(iconFader_N != null) iconFader_N.fadeIn ();

	}


	public void reset() {

		if(textFader_N != null) textFader_N.setOpacity (0.0f);
		if(imageFader_N != null) imageFader_N.setFadeValue (0.0f);
		if(iconFader_N != null) iconFader_N.setFadeValue (0.0f);

	}

	// Use this for initialization
	public void Start () {

		if(imageFader_N != null) imageFader_N.Start ();
		if(textFader_N != null) textFader_N.Start ();
		if(iconFader_N != null) iconFader_N.Start ();

	}
	

}
