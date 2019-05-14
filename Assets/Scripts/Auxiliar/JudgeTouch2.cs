using UnityEngine;
using System.Collections;

public class JudgeTouch2 : MonoBehaviour {

	public VotationController votationController;
	public int judgeNumber;
	public int judgeType;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void touch() {
		//if (votationController.vote == -1) {
		//	votationController.extendBotonaco ();
		//}
		for (int i = 0; i < votationController.judges.Length; ++i) {
			//if(i/VotationController.NumberOfTypes == judgeType) {
				votationController.judges[i].gameObject.GetComponent<UIScaleFader>().scaleOutImmediately();
			//}
		}
		//votationController.judges[judgeNumber + judgeType * VotationController.NumberOfTypes].gameObject.GetComponent<UIAnimatedImage>().go ();
		//votationController.vote = judgeNumber; // judge "0" is in position 0*/
		votationController.judges[judgeNumber + judgeType * VotationController.NumberOfTypes].gameObject.GetComponent<UIScaleFader>().scaleIn();
		votationController.receiveVote(judgeNumber+1, judgeType);



	}


}
