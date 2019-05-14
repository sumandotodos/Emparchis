using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// child task of CommonTestController
public class Type2TestController_mono : Task {

	public Task parent;

	public RosettaWrapper rosetta;
	public string textPrefix;

	public UITextFader titleFader;
	public UITextFader descrFader;

	public GameController_mono gameController;

	public UIFaderScript fader;

	public UIMood mother1;
	public UIMood son1;
	public UIMood father1;
	public UIMood daughter1;
	public UIMood parents1;
	public UIMood siblings1;

	public UIMood mother2;
	public UIMood son2;
	public UIMood father2;
	public UIMood daughter2;

	public UIMood current;
	public UIMood next;

	public Animator explosionAnim;

	FamilyMember changedRole;

	float timer;
	const float delay = 1.5f;

	int state = 0;



	public void startType2Test(Task w, Situation s) {
		titleFader.Start ();
		titleFader.reset ();
		descrFader.Start ();
		descrFader.reset ();
		w.isWaitingForTaskToComplete = true;
		waiter = w;
		state = 1;

		son1.setEnabled(false);
		daughter1.setEnabled(false);
		mother1.setEnabled(false);
		father1.setEnabled(false);
		parents1.setEnabled(false);
		siblings1.setEnabled(false);
		son2.setEnabled(false);
		daughter2.setEnabled(false);
		mother2.setEnabled(false);
		father2.setEnabled(false);
		fader.fadeIn ();
		timer = 0.0f;



		// select a situation type
		//int situation = Random.Range(1, 8); // WARNING costantize!

		//while (!TestAux.isCombinationValid (gameController.currentPlayerRole, true, situation)) {
		//	situation = Random.Range (1, 8);
		//}

		changedRole = TestAux.getRoleChangeMember (gameController.currentPlayerRole, s.table);


		((CommonTestController_mono)waiter).destFamilyMember = changedRole;
		 // this 'target' role to the rest of players, from CommonController

		switch (gameController.currentPlayerRole) {
			case Family.son:
				son1.setEnabled (true);
				current = son1;
				break;
			case Family.father:
				father1.setEnabled (true);
				current = father1;
				break;
			case Family.daughter:
				daughter1.setEnabled (true);
				current = daughter1;
				break;
			case Family.mother:
				mother1.setEnabled (true);
				current = mother1;
				break;
		}

		if (changedRole.family == 1) {

			switch (changedRole.member) {
			case Family.son:
				son1.setEnabled (false);
				next = son1;
				break;
			case Family.father:
				father1.setEnabled (false);
				next = father1;
				break;
			case Family.daughter:
				daughter1.setEnabled (false);
				next = daughter1;
				break;
			case Family.mother:
				mother1.setEnabled (false);
				next = mother1;
				break;
			case Family.siblings:
				siblings1.setEnabled (false);
				next = siblings1;
				break;
			case Family.parents:
				parents1.setEnabled (false);
				next = parents1;
				break;
			}

		} else {
			
			switch (changedRole.member) {
			case Family.son:
				son2.setEnabled (false);
				next = son2;
				break;
			case Family.father:
				father2.setEnabled (false);
				next = father2;
				break;
			case Family.daughter:
				daughter2.setEnabled (false);
				next = daughter2;
				break;
			case Family.mother:
				mother2.setEnabled (false);
				next = mother2;
				break;
			
			}
		}

		current.setMood (Mood.neutral);
		next.setMood (Mood.sad);
	
		((CommonTestController_mono)waiter).destFamilyMember.mood = Mood.sad;
		//waiter.iReturnValue = MoodInteger.enumToInt (Mood.sad); // override situation mood
		// parent (commonController) iReturnValue holds MOOD
	
	}

	void Start() {
		
	}

	// animation event
	public void changeRoleEvent() {
		current.setEnabled (false);
		next.setEnabled (true);
	}

	
	// Update is called once per frame
	void Update () {
	
		if (state == 0) { // idling

		}



		if (state == 1) { // delaying
			timer += Time.deltaTime;
			if (timer > delay) {
				//current.fadeIn ();
				explosionAnim.SetTrigger ("Explode");
				titleFader.fadeIn ();
				//next.fadeOutTask (this);
				timer = 0.0f;
				state = 2;
			}
			if (Input.GetMouseButtonDown (0))
				timer = delay;


		}

		if (state == 2) {
			timer += Time.deltaTime;
			if (timer > 0.5f) {
				descrFader.fadeIn ();
				timer = 0.0f;
				state = 3;
			}
		}

		if (state == 3) {
			timer += Time.deltaTime;
			if (timer > 0.5f) {
				
				state = 4;
			}
		}

		if (state == 4) { // waiting for xfade
			
			if (!isWaitingForTaskToComplete) {
				timer = 0;
				state = 5;
			}
		}

		if (state == 5) { // another delay
			//timer += Time.deltaTime;
			if (timer > 30f) {
				fader.fadeOutTask (this);
				state = 6;
			}
			if (Input.GetMouseButtonDown (0))
				timer = 31f;
		}


		if (state == 6) { // waiting for fadeout
			if (!isWaitingForTaskToComplete) {
				state = 0;
			
				notifyFinishTask ();
			}
		}

	}

	public Family getRandomFamily() {

		int r = Random.Range (0, 4);
		switch (r) {
		case 0:
			return Family.daughter;
		case 1:
			return Family.father;
		case 2:
			return Family.mother;
		case 3:
			return Family.son;
		}

		// impossible case
		return Family.son;

	}


}
