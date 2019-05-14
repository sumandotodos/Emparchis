using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScaleFader : Task {

	//public RawImage image;
	public float scale;
	public float targetScale;
	public float scaleSpeed = 1.0f;
	public float minScale = 0.0f;
	public float maxScale = 1.0f;

	public float startScale = 1.0f;

	bool started = false;

	// Use this for initialization
	public void Start () {
		if (started)
			return;
		started = true;
		scale = startScale;
		this.transform.localScale = new Vector3 (scale, scale, scale);
	}
	
	// Update is called once per frame
	void Update () {
		bool change = Utils.updateSoftVariable (ref scale, targetScale, scaleSpeed);
		if (change) {
			this.transform.localScale = new Vector3 (scale, scale, scale);
		}
		else {
			notifyFinishTask ();
		}
	}



	public void reset() {
		scale = targetScale = maxScale;
		this.transform.localScale = new Vector3 (scale, scale, scale);
	}

	public void scaleOut() {
		targetScale = minScale;
		//this.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
	}

	public void scaleIn() {
		targetScale = maxScale;
		//this.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
	}

	public void scaleOutTask(Task w) {
		w.isWaitingForTaskToComplete = true;
		waiter = w;
		scaleOut ();
	}

	public void scaleOutImmediately() {
		startScale = 0.0f;
		scale = targetScale = minScale;
		this.transform.localScale = new Vector3 (scale, scale, scale);
	}
}
