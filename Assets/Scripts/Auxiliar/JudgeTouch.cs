using UnityEngine;
using System.Collections;

public class JudgeTouch : MonoBehaviour {

	public NotMyRouletteController notMyRouletteController;
	public int judgeNumber;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void touch() {
		if (notMyRouletteController.testVote == -1) {
			notMyRouletteController.extendBotocano ();
		}
		for (int i = 0; i < notMyRouletteController.judges.Length; ++i) {
			notMyRouletteController.judges[i].gameObject.GetComponent<UIAnimatedImage>().reset ();
		}
		notMyRouletteController.judges[judgeNumber].gameObject.GetComponent<UIAnimatedImage>().go ();
		notMyRouletteController.testVote = judgeNumber; // judge "0" is in position 0
	}


}
