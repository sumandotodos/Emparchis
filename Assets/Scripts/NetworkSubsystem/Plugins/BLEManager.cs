/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class BLEManager : AbstractManager {

	// referencias / acoplamientos
	public GameObject btConnectingIcon;

	const string FGUUIDStringPre = @"77adbb46-7fd9-46a9-";
	const string FGUUIDStringPost = @"-a431d9f37dce";


	int abstractId = -1;

	public const int k_OK = 0;
	public const int k_UNAVAILABLE = -1;
	public const int k_OFF = -2;
	public const int k_NOTINIT = -3;
	public const int k_UNCAPABLE = -4;


	#if UNITY_IOS && !UNITY_EDITOR

	#region Native setup

	[DllImport ("__Internal")]
	private static extern string _startAsServer();

	[DllImport ("__Internal")]
	private static extern void _startAsClient(string addr);

	[DllImport ("__Internal")]
	private static extern int _initialize();

	[DllImport ("__Internal")]
	private static extern void _writeString (string str, int dest);

	[DllImport ("__Internal")]
	private static extern string _getServerAddress ();

	[DllImport ("__Internal")]
	private static extern void _retryHandshake ();

	[DllImport ("__Internal")]
	private static extern string _setServerAddress (string addr);

	[DllImport ("__Internal")]
	private static extern void _generateServerAddress ();

	[DllImport ("__Internal")]
	private static extern int _queryCapabilities();

	#endregion

	#endif

	#if UNITY_ANDROID && !UNITY_EDITOR

	AndroidJavaClass _unityPlayerClass;
	AndroidJavaObject _unityActivity;
	AndroidJavaClass _BLEpluginClass;

	public int _initialize() {

		_unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
	if(_unityPlayerClass == null) {
	MasterController.StaticLog("_unityPlayerClass is shit");
	}
		_unityActivity = _unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
	if(_unityActivity == null) {
	MasterController.StaticLog("_unityActivity is shit");
	}
		_BLEpluginClass = new AndroidJavaClass ("org.flygames.androidbleplugin.BLEManager");
	if(_BLEpluginClass == null) {
	MasterController.StaticLog("_BLEpluginClass is shit");
	}
		int res = _BLEpluginClass.CallStatic<int> ("Initialize", _unityActivity);
	MasterController.StaticLog("Result of pluing initializ: " + res);
		return res;

	}

	public int _startAsServer() { // peripheral
	//MasterController.StaticLog("BLEManager::_startAsServer - 1");
		int status = _BLEpluginClass.CallStatic<int> ("StartAsServer");
	//MasterController.StaticLog("BLEManager::_startAsServer - status: " + status);
		return status;

	}

	public string _getServerAddress() {
		return _BLEpluginClass.CallStatic<string> ("GetServerAddress");
	}

	public int _queryCapabilities() {
		AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaClass BLEpluginClass = new AndroidJavaClass ("org.flygames.androidbleplugin.BLEManager");
		return BLEpluginClass.CallStatic<int> ("QueryCapabilities", unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"));
	}

	public void _setServerAddress(string addr) {
		//MasterController.StaticLog("BLEManager::<android>_setServerAddress");
		_BLEpluginClass.CallStatic("SetServerAddress", addr);
	}

	public void _generateServerAddress() {
	//MasterController.StaticLog("BLEManager::<android>_generateServerAddress");
		_BLEpluginClass.CallStatic ("GenerateServerAddress");
	//MasterController.StaticLog("BLEManager::<android>_generateServerAddress finished");
	}

	public void _startAsClient(string addr) { // central
	//MasterController.StaticLog("BLEManager::_startAsClient - 1");
		int status = _BLEpluginClass.CallStatic<int> ("StartAsClient", addr, false);
	//MasterController.StaticLog("BLEManager::_startAsClient - status: " + status);
	}

	public void _retryHandshake() {
		_BLEpluginClass.CallStatic ("RetryHandshake");
	}

	public void _writeString(string str, int dest) {
		_BLEpluginClass.CallStatic ("WriteString", str, dest);
	}

	#endif

	#if UNITY_EDITOR 

	// THESE ARE DUMMIES!!

	public void _retryHandshake() {

	}

	public int _initialize() {

		return 0;

	}

	public int _startAsServer() { // peripheral

		return k_OK;

	}

	public string _getServerAddress() {
		return "0xABDEAD";
	}

	public void _generateServerAddress() {
		
	}

	public int _queryCapabilities() {
		return k_UNAVAILABLE;
	}

	public void _startAsClient(string addr) { // central
		
	}

	public void _writeString(string str, int dest) {
		
	}

	public void _setServerAddress(string addr) {
		
	}


	#endif

	System.Func<int, int> _serverConnectRequestCallback;
	System.Func<int, int> _serverAcceptConnectCallback;



	// common


	public override int QueryCapabilities() {
		return _queryCapabilities ();
	}


	public override int initialize(
		System.Func<string, int>receiveStringCallback,
		System.Func<int, int>setIdCallback,
		System.Func<string, int>setNextAddressCallback,
		System.Func<int, int>connectionActuallyStablishedCallback,
		System.Func<int, int>serverConnectRequestCallback,
		System.Func<int, int>serverAcceptConnectCallback) {
		btConnectingIcon.SetActive (false);
		_receiveStringCallback = receiveStringCallback;
		_setIdCallback = setIdCallback;
		_setNextAddressCallback = setNextAddressCallback;
		//...
		_serverConnectRequestCallback = serverConnectRequestCallback;
		_serverAcceptConnectCallback = serverAcceptConnectCallback;

		return _initialize ();
	}

	public override string StartServer(string forceAddr) {
		//MasterController.StaticLog("BLEManager::StartServer with forced addr - " + forceAddr);
		_setServerAddress (forceAddr);
		serverAddressGenerated = true;
		_startAsServer ();
		return forceAddr;
	}

	public override void SetServerAddress(string addr) {
		serverAddressGenerated = true;
		_setServerAddress (addr);
	}

	public override string StartServer() {
		//MasterController.StaticLog("BLEManager::StartServer - 1");
		if (!serverAddressGenerated) {
			_generateServerAddress ();
			serverAddressGenerated = true;
		}
		//MasterController.StaticLog("BLEManager::StartServer - 2");
		_startAsServer ();
		//MasterController.StaticLog("BLEManager::StartServer - 3");
		return _getServerAddress ().Substring(4, 4);
		//MasterController.StaticLog("BLEManager::StartServer - 4");
	}

	public override void SendString(int dest, string what) {
		_writeString (what + "$", dest);
	}

	bool serverAddressGenerated = false;

	IEnumerator StartClient_handshake_CoRo() {

		while (1 < 2) {
			yield return new WaitForSeconds(5.0f);
			RetryHandshake();
		}

	}

	public override void StartClient(string addr) {
		
		btConnectingIcon.SetActive (true);

		_generateServerAddress ();
		serverAddressGenerated = true;

		_startAsClient (addr);

		StartCoroutine ("StartClient_handshake_CoRo");

	}


	// callable from plugin
	public void ReceiveString(string what) {
		//MasterController.StaticLog ("<color=blue>BLEManager::ReceiveString " + what + " called</color>");
		_receiveStringCallback (what);
	}

	public override void ShowSendDataIcon() {
		btConnectingIcon.SetActive (true);
	}

	public override void HideSendDataIcon() {
		btConnectingIcon.SetActive (false);
	}

	// callable from plugin
	public void SetAbstractId(string idStr) {
		MasterController.StaticLog ("BLEManager::SetAbstractId " + idStr + " called");
		int id;
		int.TryParse (idStr, out id);
		if (id >= abstractId) {
			StopCoroutine ("StartClient_handshake_CoRo");
			abstractId = id;
			if(_setIdCallback != null)
			_setIdCallback (id);
		} 
		else {
			MasterController.StaticLog ("weird: BLEManager::SetAbstractId " + idStr);
			if(_setIdCallback != null)
			_setIdCallback (-2);
		}
		btConnectingIcon.SetActive (false);
		if (ConnectionActuallyStablishedCallback != null) {
			ConnectionActuallyStablishedCallback (0);
		}
	}

	public override string LongAddressFromShortAddress(string shortAddress) {
		return FGUUIDStringPre + shortAddress + FGUUIDStringPost;
	}

	// callable from plugin
	public void NextServer(string nextAddr) {
		FGDebug ("<color=red>BLEManager::NextServer = " + nextAddr + "</color>");
		if (_setNextAddressCallback != null) {
			_setNextAddressCallback (nextAddr);
		}
	}



	IEnumerator ConnectionDidSucceed_CoRo() {
		yield return new WaitForSeconds (1.0f);

		StartServer ();
		MasterController.StaticLog ("<color=orange>Chain starting server "+_getServerAddress()+"</color>");
	}
	// callable from plugin
	public void ConnectionDidSucceed() { // connection as client success
		//MasterController.StaticLog ("<color=yellow>As client: connection did succeed</color>");
		StartCoroutine ("ConnectionDidSucceed_CoRo");
	}



	// callable from plugin
	public void ServerConnectRequest(string param) {
		btConnectingIcon.SetActive (true);
		if (_serverConnectRequestCallback != null)
			_serverConnectRequestCallback (0);
	}


	public void ServerAcceptConnect(string param) { // connection as server success
		btConnectingIcon.SetActive (false);
		if (_serverAcceptConnectCallback != null)
			_serverAcceptConnectCallback (0);
	}

	public void FGDebug(string str) {
		MasterController.StaticLog (str);
	}

	// reverse API: called from plugin
	public void PeripheralDidDisconnect() {
		
	}

	// reverse API: called from plugin
	public void CentralDidDisconnect() {

	}


	void Start() {
		if (btConnectingIcon != null) {
			btConnectingIcon.SetActive (false);
		}
	}

	private void RetryHandshake() {
		_retryHandshake ();
	}

	public override bool HandlesDestination(int dest) {
		return true;
	}

	public override void Cleanup() {
		if (btConnectingIcon != null)
			btConnectingIcon.SetActive (false);
	}

}
*/