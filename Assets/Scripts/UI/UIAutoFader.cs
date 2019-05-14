using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIAutoFader : MonoBehaviour {

	float opacity;
	float targetOpacity;
	public float opacitySpeed = 5.0f;
	RawImage img;
	float r, g, b;

	public float maxOpacity = 1.0f;

	public void flash() {
		opacity = 1.0f;
	}

	// Use this for initialization
	void Start () {
		opacity = targetOpacity = 0.0f;
		img = this.GetComponent<RawImage> ();
		Color rgb = img.color;
		r = rgb.r;
		g = rgb.g;
		b = rgb.b;
		img.color = new Color (r, g, b, opacity);
	}
	
	// Update is called once per frame
	void Update () {

		bool change = Utils.updateSoftVariable (ref opacity, targetOpacity, opacitySpeed);
		if (change) {
			img.color = new Color (r, g, b, opacity);
		}
	}

	public void fadein() {
		targetOpacity = maxOpacity;
	}

	public void fadeout() {
		targetOpacity = 0.0f;
	}
}
