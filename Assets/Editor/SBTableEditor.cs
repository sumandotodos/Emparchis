#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;


[CustomEditor(typeof(SBTable))]
public class SBTableEditor : Editor {




	public override void OnInspectorGUI() {


		SBTable tableRef = (SBTable)target;

		DrawDefaultInspector ();

		if (GUILayout.Button ("export")) {

			string crsvRep = tableRef.export ();
			EditorGUIUtility.systemCopyBuffer = crsvRep;


		}



	}


}

#endif