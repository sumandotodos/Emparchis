using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchButton : MonoBehaviour {

	public GalleryController galleryController;
	public GalleryController_mono galleryControllerMono;
	public int id;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// event callback
	public void touchCallback() {
		if(galleryController!=null)
			galleryController.touchButton (id);
		if(galleryControllerMono!=null)
			galleryControllerMono.touchButton (id);
	}
}
