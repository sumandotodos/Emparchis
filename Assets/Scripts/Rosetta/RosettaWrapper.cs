using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RosettaWrapper : MonoBehaviour {

	public Rosetta rosetta;

	// Use this for initialization
	void Start () {
		if (rosetta == null) {
			rosetta = GameObject.Find ("RosettaWrapper").GetComponent<RosettaWrapper> ().rosetta;
		}
	}
		
}
