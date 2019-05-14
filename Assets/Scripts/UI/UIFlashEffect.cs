using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFlashEffect : Task {

	int state;
	float scale;

	public float maxScale = 40.0f;

	public float effectSpeed = 60.0f;

	// Use this for initialization
	void Start () {
		state = 0;
		scale = 0.0f;
		this.transform.localScale = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {

		if (state == 0) { // 
			return;
		}

		if (state == 1) { // expanding
			bool change = Utils.updateSoftVariable(ref scale, maxScale, effectSpeed);
			if (!change) {
				notifyFinishTask ();
				state = 0;
			}
			this.transform.localScale = new Vector3 (scale, scale, scale);
		}

		if (state == 2) { // contracting
			bool change = Utils.updateSoftVariable (ref scale, 0.0f, effectSpeed);
			if (!change) {
				notifyFinishTask ();
				state = 0;
			}
			this.transform.localScale = new Vector3 (scale, scale, scale);
		}

	}

	public void expandTask(Task w) {

		w.isWaitingForTaskToComplete = true;
		waiter = w;

		state = 1;

	}

	public void contract() {

		state = 2;

	}
}
