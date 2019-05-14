using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FadeType { ToOpaque, ToTransparent };

[RequireComponent(typeof(UITextAndImageFader))]
public class UIAutoDelayFade : MonoBehaviour {

    public float delay;
    float remaining;
    UITextAndImageFader textAndImageFader;

    public FadeType fadeType;

	// Use this for initialization
	void Start () {
        remaining = delay;
        textAndImageFader = this.GetComponent<UITextAndImageFader>();
	}
	
	// Update is called once per frame
	void Update () {
        if (remaining > 0.0f)
        {
            remaining -= Time.deltaTime;
            if (remaining <= 0.0f)
            {

                if (fadeType == FadeType.ToOpaque) textAndImageFader.fadeIn();
                else textAndImageFader.fadeOut();

            }
        }
	}
}
