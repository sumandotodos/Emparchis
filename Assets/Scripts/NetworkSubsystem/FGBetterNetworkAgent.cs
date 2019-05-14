using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

[System.Serializable]
public class EnqueuedMessage {

	public int seq;
	public int dest;
	public string fullMessage;
	public int ttl;

}

public class FGBetterNetworkAgent : MonoBehaviour {

	// constants
	public const int Module_BLE = 0;

	// Referencias/acoplamientos
	public MasterController masterController;
	public GameController gameController;
	public Text borrameLuego_N;

	float ResendQuantumTime = 1.0f;
	float ResendRemainingTime = 1.0f;

	byte[] bytes;

	// Modules?
	public AbstractManager[] managers;

	public int id = -1;

	public bool connected = false;
	public bool ConnectedToServer = false;
	bool initialized = false;


	Dictionary <int, int> receiveSeq;
	Dictionary <int, int> sendSeq;
	public List<EnqueuedMessage> sendList;
	Queue<string> commandQueue;

	System.Func<int, int> ConnectionActuallyStablishedCallback;



	bool dataAvailable = false;
	string readData;
	const int poloMs = 1000;
	const int TTL = 2;
	public float poloElapsedTime;
	public const float poloTimeout = 10.0f;
	public bool tryingToReconnect = false;
	protected float reconnectElapsedTime = 0.0f;
	protected const float reconnectRetry = 4.0f;






	// API Methods :: Connection
	public void initialize(string serverURL, int port) {

		if (initialized)
			return;

		commandQueue = new Queue<string> ();
		receiveSeq = new Dictionary<int, int> ();
		sendSeq = new Dictionary<int, int> ();
		sendList = new List<EnqueuedMessage> ();


		bytes = new byte[1024];


		// sólo vamos a tener un manager de momento, managers[0] = bleManager
		int state = managers[0].initialize(
			receiveCommand,
			SetId,
			SetNextServer_RelayToNewGameActivityController,
			ConnectionDidStablish,
			HideQRResult_RelayToNewGameActivityController,
			ShowQRResult_RelayToNewGameActivityController);
		switch (state) {

		case BLEManager.k_OFF:
			break;

		case BLEManager.k_UNAVAILABLE:
			break;

		}

		if (borrameLuego_N != null) {
			borrameLuego_N.text = "" + (-1);
		}
		initialized = true;

	}

	public void Cleanup() {
		foreach (AbstractManager m in managers) {
			m.Cleanup ();
		}
	}

	public string StartServer(string addr) {
		return managers [0].StartServer (addr);
	}

	public string StartServer() {
		return managers [0].StartServer ();
	}

	public void SetServerAddress(string addr) {
		managers [0].SetServerAddress (addr);
	}

	public void StartClient(string connectToAddr, System.Func<int, int> callback) {
		StartClient (connectToAddr);
		ConnectionActuallyStablishedCallback = callback;
	}

	public void StartClient(string addr) {
		//MasterController.StaticLog ("FGBetterNetwork::StartClient");
		managers [0].StartClient (addr);
		//MasterController.StaticLog("FGBetterNetwork::StartClient - finish");
	}

	public void disconnect() {

		if (!connected)
			return;

		connected = false;
	}

	public void disconnectGently() {

		if (!connected)
			return;

		connected = false;
	}

	public int connectGently(string url, int port) {
		return 0;
	}

	public void sendCommand(int recipient, string command) {

		int seq = sendSeqFor (recipient);
		string safeCommand = seq + "#" + id + "#" + command;
		string fullMessage = //"sendmessage " + recipient + " " + 
			safeCommand;
		sendMessage (recipient, fullMessage);
		incSendSeqFor (recipient);

		EnqueuedMessage newMessage = new EnqueuedMessage ();
		newMessage.seq = seq;
		newMessage.dest = recipient;
		newMessage.fullMessage = fullMessage;
		newMessage.ttl = TTL;
		sendList.Add (newMessage);

	}

	public void broadcast(string command) {


		for (int i = 0; i < gameController.nPlayers; ++i) {
			if (i != id) {
				MasterController.StaticLog ("<color=red>broadcasting to " + i + "</color>");
				sendCommand (i, command);
			}
		}

	}


		
	// callbacks
	public int SetNextServer_RelayToNewGameActivityController(string next) {
		//MasterController.StaticLog ("<color=red>FGBetterNA::NextServer = " + next + "</color>");
		return CreateNewGameController.GetSingleton ().SetNextServer (next);
	}

	public int HideQRResult_RelayToNewGameActivityController(int param) {
		return CreateNewGameController.GetSingleton ().HideQRResult (param);
	}

	public int ShowQRResult_RelayToNewGameActivityController(int param) {
		return CreateNewGameController.GetSingleton ().ShowQRResult (param);
	}



	// Reverse API

	public int receiveCommand(string data) {
		MasterController.StaticLog ("<color=cyan>Receive: " + data + "</color>");
		commandQueue.Enqueue (data);
		return 0;
	}




	public void sendCommandUnsafe(int recipient, string command) {

		sendMessage (recipient, command);

	}

	public void broadcastUnsafe(string command) {
		for (int i = 0; i < gameController.nPlayers; ++i) {
			if (i != id) {
				sendCommandUnsafe (i, command);
			}
		}
	}


	public void sendMessage(int dest, string command) {

		for (int i = 0; i < managers.Length; ++i) {
			if(managers[i].HandlesDestination(dest)) {
				managers [i].SendString (dest, command);
				break;
			}
		}

	}


	// Reverse API AKA Callbacks

	//public System.Func<int, int> ConnectionDidStablish;
	int ConnectionDidStablish(int p) {
		if (ConnectionActuallyStablishedCallback != null) {
			return ConnectionActuallyStablishedCallback (p);
		} else
			return 0;
	}


	public int SetId(int _id) {
		//MasterController.StaticLog ("FGBEtter...::SetId called : " + _id);
		if (borrameLuego_N != null) {
			borrameLuego_N.text += "" + _id;
		}
		id = _id;
		return id;
	}

	public int receiveSeqFor(int origin) {

		if (receiveSeq.ContainsKey (origin)) {
			return receiveSeq [origin];
		} else {
			receiveSeq [origin] = 0;
			return 0;
		}

	}

	public int sendSeqFor(int dest) {

		if (sendSeq.ContainsKey (dest)) {
			return sendSeq [dest];
		} else {
			sendSeq [dest] = 0;
			return 0;
		}

	}

	public void unseeOrigin(int o) {
		if(receiveSeq.ContainsKey(o)) {
			receiveSeq.Remove (o);
		}
		if (sendSeq.ContainsKey (o)) {
			sendSeq.Remove (o);
		}
	}

	public void incReceiveSeqFor(int origin) {
		receiveSeq [origin]++;
	}

	public void incSendSeqFor(int dest) {
		sendSeq [dest]++;
	}

	// remove from sendlist once message has been acknowledged
	public void ack(int seq, int origin) {
		MasterController.StaticLog ("<color=green>acking msg " + seq + " from user " + origin +"...</color>");
		//int rSeq; 
		//rSeq = receiveSeqFor (origin);

			for (int i = 0; i < sendList.Count; ++i) {
				EnqueuedMessage msg = sendList [i];
				MasterController.StaticLog ("<color=yellow>   >> msg " + i + " : msg.seq=" + msg.seq + ", msg.dest=" + msg.dest + "</color>");
				if ((msg.seq == seq) && (msg.dest == origin)) { 
					MasterController.StaticLog ("<color=green>   >> found and removed from sendlist</color>");
					sendList.RemoveAt (i);
					--i;
				}
			}

	}

	//
	public string consumeData() {

		string res;
		res = commandQueue.Dequeue ();
		return res;

	}

	// API
	public void showSendDataIcon() {
		managers [0].ShowSendDataIcon ();
	}

	public void hideSendDataIcon() {
		managers [0].HideSendDataIcon ();
	}

	// Update is called once per frame
	void Update () {

		if (!initialized)
			return;

		ResendRemainingTime -= Time.deltaTime;
		if (ResendRemainingTime <= 0.0f) {
			for (int i = 0; i < sendList.Count; ++i) {
				--sendList [i].ttl;
				if (sendList [i].ttl == 0) {
					MasterController.StaticLog ("Actually Resending " + sendList [i].fullMessage + " to " + sendList[i].dest);
					sendMessage (sendList [i].dest, sendList [i].fullMessage); // from the Main Thread only!!
					sendList [i].ttl = TTL;
				}
			}
			ResendRemainingTime = ResendQuantumTime;
		}

		//if(connected) poloElapsedTime += Time.deltaTime;

		/*if (tryingToReconnect) {
			reconnectElapsedTime += Time.deltaTime;
			if (reconnectElapsedTime > reconnectRetry) {
				reconnectElapsedTime = 0.0f;
				int res = connect ();
				if (res == 0) {
					tryingToReconnect = false;
					sendMessage("initgame " + gameController.getUserLogin() + " " + gameController.gameRoom);
				}
			}
		}*/

		/*if (poloElapsedTime > (poloTimeout)) {
			poloElapsedTime = 0.0f;
			disconnect ();
			tryingToReconnect = true;
			reconnectElapsedTime = reconnectRetry + 1.0f;
		}*/

		while (commandQueue.Count > 0) {


			string command = consumeData ();
			//MasterController.StaticLog ("<color=orange>Command: " + command + "</color>");

			if (gameController != null) {
				//MasterController.StaticLog ("<color=orange>processing...</color>");
				gameController.network_processCommand (command);
			}


		}

	}

}
