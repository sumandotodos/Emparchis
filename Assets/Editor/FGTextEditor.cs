

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(FGText))]
public class FGTextEditor : Editor  {
	

	public override void OnInspectorGUI() {

		DrawDefaultInspector ();

		FGText textRef = (FGText)target;

		GameObject RosettaGO = GameObject.Find ("RosettaWrapper");
		if (RosettaGO != null)
			textRef.rosetta = RosettaGO.GetComponent<RosettaWrapper> ().rosetta;

		if (GUILayout.Button ("Upload to rosetta")) {
			if (textRef.rosetta != null) {
				textRef.rosetta.registerString (textRef.key, textRef.text);
				Object prefab = PrefabUtility.CreateEmptyPrefab ("Assets/Prefabs/Rosetta.prefab");
				PrefabUtility.ReplacePrefab (textRef.rosetta.gameObject, prefab, ReplacePrefabOptions.ConnectToPrefab);


			}
		}

	}

}

#endif