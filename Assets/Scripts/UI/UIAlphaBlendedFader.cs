using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIAlphaBlendedFader : Task {

	float opacity;
	float targetOpacity;
	public Color fadeColor;
	public float fadeSpeed = 0.9f;
	RawImage imageComponent;
	public bool autoFadeIn;
	public float initialOpacity;

	// Use this for initialization
	void Start () {
		imageComponent = this.GetComponent<RawImage> ();
		opacity = initialOpacity;
		targetOpacity = initialOpacity;
		imageComponent.material.SetColor("_TintColor", new Color (fadeColor.r, fadeColor.g, fadeColor.b, opacity));
		if (initialOpacity > 0.0f) {
			imageComponent.enabled = true;
		} else
			imageComponent.enabled = false;
		if (autoFadeIn) {
			opacity = 1.0f;
			imageComponent.enabled = true;
			imageComponent.material.SetColor("_TintColor", new Color (fadeColor.r, fadeColor.g, fadeColor.b, opacity));
			fadeIn ();
		}
	}

	public void setFadeValue(float nOp) {
		opacity = targetOpacity = nOp;
		if (nOp > 0.0f) {
			imageComponent.enabled = true;
			imageComponent.material.SetColor("_TintColor", new Color (fadeColor.r, fadeColor.g, fadeColor.b, opacity));
		}

	}

	public void fadeInTask(Task w) {
		waiter = w;
		waiter.isWaitingForTaskToComplete = true;
		fadeIn ();
	}

	public void fadeOutTask(Task w) {
		waiter = w;
		waiter.isWaitingForTaskToComplete = true;
		fadeOut ();
	}

	public void fadeOut() {
		targetOpacity = 0.0f;
	}

	public void fadeIn() {
		imageComponent.enabled = true;
		targetOpacity = 1.0f;
	}

	// Update is called once per frame
	void Update () {

		bool change = Utils.updateSoftVariable (ref opacity, targetOpacity, fadeSpeed);
		if (change) {
			imageComponent.material.SetColor("_TintColor", new Color (fadeColor.r, fadeColor.g, fadeColor.b, opacity));
			if (opacity == 0.0f) {
				imageComponent.enabled = false;
				notifyFinishTask ();
			} else if (opacity == 1.0f) {
				notifyFinishTask ();
			}
		} 

	}
}
