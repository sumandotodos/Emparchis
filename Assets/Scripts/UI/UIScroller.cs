﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScroller : MonoBehaviour {

	SoftFloat xPosition;
	public float pageWidth;
	int page;
	public int TotalPages;
	Vector3 initialPos;
	public float speed;

	public float xPosValue;

	public UIFaderScript leftArrow_N;
	public UIFaderScript rightArrow_N;

	bool started = false;

	// Use this for initialization
	public void Start () {
		if (started)
			return;
		started = true;
		xPosition = new SoftFloat ();
		xPosition.setSpeed (speed);
		xPosition.setEasyType (EaseType.tanh);
		xPosition.setValueImmediate (0.0f);
		initialPos = this.transform.position;
	}

	public void initialize() {
		Start ();
		xPosition.setValueImmediate (0.0f);
		xPosition.setValue (-pageWidth);
		page = 0;
		leftArrow_N.Start ();
		rightArrow_N.Start ();
		rightArrow_N.fadeOut ();
	}

	public void setPage(int p) {
		page = p;
		xPosition.setValue (-(page+1) * pageWidth);
	}

	public void nextPage() {
		if (page < (TotalPages-1)) {
			setPage (page + 1);
			leftArrow_N.fadeOut ();
		}
		if (page == (TotalPages-1))
			rightArrow_N.fadeIn ();
	}

	public void previousPage() {
		if (page > 0) {
			setPage (page - 1);
			rightArrow_N.fadeOut ();
		}
		if (page == 0)
			leftArrow_N.fadeIn ();
	}
	
	// Update is called once per frame
	void Update () {
		xPosValue = xPosition.getValue ();
		xPosition.update ();
		Vector3 newPos = new Vector3 (xPosition.getValue () * Screen.height / 1280.0f, 0, 0);
		this.transform.position = initialPos + newPos;
	}
}
