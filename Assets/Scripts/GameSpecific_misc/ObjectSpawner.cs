using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectSpawner : MonoBehaviour {

	public GameObject objectToSpawn;

	public string emotionString;
	public string[] emotions;

	public RosettaWrapper rosettaWrapper;

	public NotMyRouletteController parentController;

	public float minTimeToSpawn;
	public float maxTimeToSpawn;

	public float timeToSpawn;
	public float timer;

	RawImage img;

	Vector3[] corners;

	int state;

	// Use this for initialization
	void Start () {

		state = 0;

		timeToSpawn = Random.Range (minTimeToSpawn, maxTimeToSpawn);
		timer = 0.0f;

		img = this.GetComponent<RawImage> ();

		corners = new Vector3[4];

		startSpawning ();

	}

	public void startSpawning() {

		if (!emotionString.Equals ("")) {
			emotionString = emotionString.Replace ("  ", " "); //
			emotionString = emotionString.Replace ("  ", " "); // revome duplicated spaces
			emotionString = emotionString.Replace(" ", ";");
			emotionString = emotionString.Replace("\n", ";");
			emotionString = emotionString.Replace("\\n", ";");

			emotions = emotionString.Split (';');
			Debug.Log("Emotions: " + emotionString + "(" + emotions.Length + ")");
			state = 1;
		}

	}

	public void stopSpawning() {

		state = 0;

	}
	
	// Update is called once per frame
	void Update () {

		if (state == 0) { // idling

		}

		if (state == 1) {
			timer += Time.deltaTime;
			if (timer > timeToSpawn) {
				img.rectTransform.GetWorldCorners (corners);
				int i = corners.Length;
				timeToSpawn = Random.Range (minTimeToSpawn, maxTimeToSpawn);
				timer = 0.0f;
				float randomY = Random.Range (corners [0].y, corners [1].y);
				float randomX = Random.Range (corners [0].x, corners [2].x);
				Vector3 spawnPos = new Vector3 (randomX, randomY, 0);
				GameObject newGO = (GameObject)Instantiate (objectToSpawn, spawnPos, Quaternion.Euler (0, 0, 0));
				newGO.GetComponent<EmotionCloud> ().rosetta = rosettaWrapper.rosetta;
				newGO.GetComponent<EmotionCloud>().stringsInBubble = emotions;
				newGO.GetComponent<EmotionCloud> ().Start ();
				newGO.transform.SetParent(this.transform);
				newGO.transform.localScale = Vector3.one;
				float factor = (Screen.width / 200.0f);
				//newGO.transform.localScale = new Vector3 (factor, factor, factor);
			}
		}


	}
}
