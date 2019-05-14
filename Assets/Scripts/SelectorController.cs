using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectorController : MonoBehaviour {

	public string StandardBootstrapScene;
	public string KidsBootstrapScene;
	public string MonoBootstrapScene;
	public string KidsMonoBootstrapScene;
	public AudioClip clickSound;
	public AudioManager audioManager;
	public UIFaderScript fader;
    public GameObject loadMosca;

	// Use this for initialization
	void Start () {
        loadMosca.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void clickOnWisStandard() {
		load(StandardBootstrapScene);
	}

	public void clickOnWisKids() {
		load(KidsBootstrapScene);
    }

	public void clickOnWisMono() {
        load(MonoBootstrapScene);
    }

	public void clickOnWisKidsMono() {
        load(KidsMonoBootstrapScene);
	}

    IEnumerator LoadCoRo() {
        yield return new WaitForSeconds(1.0f);
        loadMosca.SetActive(true);
    }

    private void load(string level)
    {
        audioManager.playSound(clickSound);
        fader.fadeOut();
        SceneManager.LoadSceneAsync(level);
        StartCoroutine("LoadCoRo");
    }
}
