using UnityEngine;
using System.Collections;

public class Task : MonoBehaviour {

	//[HideInInspector]
	public bool isWaitingForTaskToComplete = false;
	[HideInInspector]
	public Task waiter = null;
	[HideInInspector]
	public int iReturnValue;
	[HideInInspector]
	public float fReturnValue;
	[HideInInspector]
	public bool bReturnValue;

	public void notifyFinishTask() {
		if (waiter != null) {
			waiter.isWaitingForTaskToComplete = false;
			waiter = null;
		}
	}

	public void cancel() {
		waiter = null;
		isWaitingForTaskToComplete = false;
	}

}
