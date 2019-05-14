using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DebugMode { DebugMode, ReleaseMode };

public class FGDebug  {

	static Dictionary<string, bool> activeCategory = new Dictionary<string, bool> ();

	static DebugMode mode;

	public static void setMode(DebugMode m) {

		mode = m;

	}

	public static void setCategoryActive(string cat, bool active) {
		
		activeCategory [cat] = active;

		if(cat.Equals("all")) {
			activeCategory["none"] = !active;
		}
		if(cat.Equals("none")) {
			activeCategory["all"] = !active;
		}
		if((!cat.Equals( "all")) && (!cat.Equals("none")) && active == true) {

			activeCategory["none"] = false;
			activeCategory["all"] = false;


		}

	}


	public static void log(string msg, string category) {

		if(mode == DebugMode.ReleaseMode) return;

		if(activeCategory["none"] == true) return;

		if((activeCategory[category] == true) || (activeCategory["all"] == true)) {

			Debug.Log(msg);

		}

	}

}
