using UnityEngine;
using System.Collections;

[System.Serializable]
public class BootStrapData {

	public string loginServer;
	public int loginServerPort;
	public string socketServer;
	public int socketServerPort;
	public string commandServer;
	public int commandsServerPort;

	public BootStrapData(string ls, int lsp, string ss, int ssp, string cs, int csp) {
		loginServer = ls;
		loginServerPort = lsp;
		socketServer = ss;
		socketServerPort = ssp;
		commandServer = cs;
		commandsServerPort = csp;
	}
}

public class Utils : MonoBehaviour {

	/* constants */

	public const string BootstrapURL = "http://apps.flygames.org/bootstrap";

	public const string RecoveryScript = "/requestNewPassword.php";
	public const string flygamesSSLAuthHost = "apps.flygames.org";
	public const string CheckUserScript = "/checkUser";
	public const string GetFreshRoomID = "/nextRoomID.php";
	public const string GetServerStatus = "/serverstatus";
	public const string ReleaseRoomID = "/clearRoomID";
	public const string getMinVersionScript = "/getMinimumCompatVersion.php";

	public const int build = 23;

	public const int socketPort = 993;

	public const string fallbackLoginServer = "https://apps.flygames.org";
	public const int fallbackLoginServerPort = 9090;
	public const string fallbackSocketServer = "apps.flygames.org";

	public const string fallbackCommandServer = "apps.flygames.org";


	public const int Msg1Player = 0;
	public const int Msg2Player = 1;
	public const int Msg3Player = 2;
	public const int Msg4Player = 3;
	public const int MsgIncompatVersion = 4;
	public const int MsgRepeatedLogin = 5;

	// Use this for initialization
	void Start () {
	
	}
		
	// Update is called once per frame
	void Update () {
	
	}

	public void mecagoentodo() {

	}

	public static char decToHexChar(int d) {
		switch (d) {
		case 0:
			return '0';
		case 1:
			return '1';
		case 2:
			return '2';
		case 3:
			return '3';
		case 4:
			return '4';
		case 5:
			return '5';
		case 6:
			return '6';
		case 7:
			return '7';
		case 8:
			return '8';
		case 9:
			return '9';
		case 10:
			return 'A';
		case 11:
			return 'B';
		case 12:
			return 'C';
		case 13:
			return 'D';
		case 14:
			return 'E';
		case 15:
			return 'F';
		}
		return '0';
	}

	public static string valueToHexstring(float v) {

		int iVal = (int)(v*255.0f);

		int lo = iVal & 15;
		int hi = (iVal >> 4) & 15;

		return "" + decToHexChar (hi) + decToHexChar (lo);

	}

	public static string chopSpaces(string s) {
		string[] strs = s.Split (' ');
		string res = strs [0];
		for (int i = 1; i < strs.Length; ++i) {
			res += "\n" + strs [i];
		}
		return res;
	}

	public static bool updateSoftVariable(ref float val, float target, float speed) {

		bool hasChanged = false;

		if (val < target) {
			val += speed * Time.deltaTime;
			hasChanged = true;
			if (val > target)
				val = target;
		}

		if (val > target) {
			val -= speed * Time.deltaTime;
			hasChanged = true;
			if (val < target)
				val = target;
		}


		return hasChanged;

	}

	/*public static void queueMessage(string msg) {

		string uuid = SystemInfo.deviceUniqueIdentifier;
		GameObject MailQueueGO = new GameObject ();
		MailQueueGO.name = "MailQueueAgent";
		MailQueueGO.AddComponent<QueueMailAgent> ().initialize (uuid, msg);
		DontDestroyOnLoad (MailQueueGO);


	}*/



}
