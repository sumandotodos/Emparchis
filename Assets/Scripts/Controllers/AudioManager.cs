using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	// this will have channels and stuff...

	AudioSource aSource;

	public void playSound(AudioClip c) {
		if(c!=null)
		aSource.PlayOneShot (c);
	}

	public static AudioManager instance = null;
	public static AudioManager getInstance() {
		return instance;
	}

	// Use this for initialization
	void Start () {
	
		instance = this;
		aSource = this.GetComponent<AudioSource> ();

	}
	

}
