using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMood : MonoBehaviour {

	public Texture happy;
	public Texture sad;
	public Texture neutral;
	public Texture thoughtful;
	public Texture assertive;

	public RawImage image;

	void Start() {
		//image.texture = neutral;
	}

	public void setEnabled(bool en) {
		image.enabled = en;
	}

	public void setMood(int m) {
		switch (m) {
		case 0:
			image.texture = happy;
			break;
		case 1:
			image.texture = sad;
			break;
		case 2:
			image.texture = assertive;
			break;
		case 3:
			image.texture = neutral;
			break;
		case 4:
			image.texture = thoughtful;
			break;
		}
	}

	public void setMood(Mood m) {
		switch (m) {
			case Mood.happy:
				image.texture = happy;
				break;
			case Mood.sad:
				image.texture = sad;
				break;
			case Mood.assertive:
				image.texture = assertive;
				break;
			case Mood.neutral:
				image.texture = neutral;
				break;
			case Mood.thoughtful:
				image.texture = thoughtful;
				break;
		}
	}

}
