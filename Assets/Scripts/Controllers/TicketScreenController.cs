using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TicketScreenController : MonoBehaviour {

	public GameController gameController;

	// orden de los colores: 0 rojo, 1 azul, 2 amarillo, 3 vacío
	public const int RedBottle = 0;
	public const int BlueBottle = 1;
	public const int YellowBottle = 2;
	public const int ClearBottle = 3;

	public GameObject[] bottlePrefab;

	public GameObject bottlesParent;


	public RawImage[] bottleRI;


	public Text[] bottleTxt;

	List<GameObject>[] list;


	const int DiscreteBottleLimit = 10;
	public const int MaxColors = 4;

	void Start() {

		list = new List<GameObject>[MaxColors];

		for (int i = 0; i < MaxColors; ++i) {
			list [i] = new List<GameObject> ();
		}

	}

	public void updateBottles() {

		for (int i = 0; i < MaxColors; ++i) {
			for (int j = 0; j < list [i].Count; ++j) {
				Destroy (list [i][j]);
			}
		}

		Player localPlayer = gameController.playerList [gameController.localPlayerN];

		for (int i = 0; i < MaxColors; ++i) {

			bottleRI [i].transform.localScale = Vector3.one;
			Vector2 spawnPos = bottleRI[i].transform.position;
			float yDelta = bottleRI [i].rectTransform.rect.height * Screen.height / 1280.0f;
			int nBottles = localPlayer.bottlesReceived [i];
			if (nBottles < DiscreteBottleLimit) { // few bottles
				
				bottleTxt [i].enabled = false;

				if (nBottles == 0) { // no bottles
					bottleRI [i].enabled = false;
				} 
				else { // a few bottles
					bottleRI [i].enabled = true;
					for (int j = 1; j < nBottles; ++j) {
						spawnPos.y += yDelta;
						GameObject newGO = Instantiate (bottlePrefab [i], spawnPos, Quaternion.Euler (0, 0, 0));
						newGO.transform.SetParent (bottlesParent.transform);
						newGO.transform.localScale = Vector3.one;
						list [i].Add (newGO);

					}
				}

			} 
			else { // lots of bottles

				bottleTxt [i].enabled = true;
				bottleTxt [i].text = "x " + localPlayer.bottlesReceived [i];
				bottleRI [i].enabled = true;

			}

		}

	}
}
