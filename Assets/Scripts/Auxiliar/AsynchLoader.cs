using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;



public class AsynchLoader : MonoBehaviour {

	public string scene;

	// Use this for initialization
	IEnumerator Start () {
		AsyncOperation loadAll = SceneManager.LoadSceneAsync ("Scenes/" + scene);
		yield return loadAll;
	}
	

}
