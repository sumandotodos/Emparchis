using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TestDeConectividadController : MonoBehaviour {

	public UIScaleFader bottonScaler;

	public string SceneToLoad;
	public string RetryScene;

	public RawImage httpIndicatorRI;
	public RawImage socketIndicatorRI;
	public FGBetterNetworkAgent networkController;

	public Texture greenIndicator;
	public Texture redIndicator;

	// Use this for initialization
	void Start () {
		bottonScaler.scaleOutImmediately ();
		StartCoroutine ("testCoRo");	
	}

	const string server = "apps.flygames.org";

	public IEnumerator testCoRo() {
		bool httpsPass = false;
		bool socketPass = false;
		WWW www;
		yield return www = new WWW ("https://" + server);
		if (www.error != null) {
			httpIndicatorRI.texture = redIndicator;
		} else {
			httpIndicatorRI.texture = greenIndicator;
			httpsPass = true;
		}
		yield return new WaitForSeconds (1f);
		int res = networkController.connectGently (server, FGUtils.socketPort);
		if (res != 0) {
			socketIndicatorRI.texture = redIndicator;
		} else {
			socketIndicatorRI.texture = greenIndicator;
			socketPass = true;
		}
		networkController.disconnectGently ();
		if (httpsPass && socketPass) {
			yield return new WaitForSeconds (2.5f);
			SceneManager.LoadScene (SceneToLoad);
		} else {
			bottonScaler.scaleIn ();
		}


	}

	public void retry() {
		SceneManager.LoadScene (RetryScene);
	}
}
