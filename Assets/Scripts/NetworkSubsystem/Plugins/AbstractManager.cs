/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractManager : MonoBehaviour {

	public System.Func<string, int> ReceiveStringCallback;
	public System.Func<int, int> _setIdCallback;
	public System.Func<string, int>_setNextAddressCallback;
	public System.Func<string, int> _receiveStringCallback;
	public System.Func<int, int>ConnectionActuallyStablishedCallback;

	public abstract int initialize (
		System.Func<string, int>receiveStringCallback,
		System.Func<int, int>setIdCallback,
		System.Func<string, int>setNextAddressCallback,
		System.Func<int, int>connectionActuallyStablishedCallback,
		System.Func<int, int>serverConnectRequestCallback,
		System.Func<int, int>serverAcceptConnectCallback);

	public abstract string StartServer ();
	public abstract string StartServer (string forceAddr);

	public abstract void SetServerAddress (string addr);

	public abstract void StartClient (string addr);

	public abstract void SendString (int dest, string what);

	public abstract string LongAddressFromShortAddress (string shortAddress);

	public abstract bool HandlesDestination(int dest);

	public abstract int QueryCapabilities();

	public abstract void ShowSendDataIcon ();
	public abstract void HideSendDataIcon();

	public abstract void Cleanup();
}
*/