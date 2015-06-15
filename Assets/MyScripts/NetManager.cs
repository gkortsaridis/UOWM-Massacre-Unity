using UnityEngine;
using gui = UnityEngine.GUILayout;

public class NetManager : MonoBehaviour
{
	public GameObject PlayerPrefab;
	string ip = "127.0.0.1";
	
	public void CreatePlayer()
	{
		Debug.Log ("Create player");
		connected = true;

		var g = (GameObject)Network.Instantiate(PlayerPrefab, transform.position, transform.rotation, 1);
		g.GetComponentInChildren<Camera>().enabled = true;


		GetComponent<Camera>().enabled = false;
	}

	void OnDisconnectedFromServer(){ connected = false; }

	void OnPlayerDisconnected(NetworkPlayer pl){ Network.DestroyPlayerObjects(pl); }

	void OnConnectedToServer(){ CreatePlayer(); }

	void OnServerInitialized(){ CreatePlayer(); }

	bool connected;
	void OnGUI()
	{
		if (!connected)
		{
			Debug.Log(Network.connections);
			ip = gui.TextField(ip);
			if (gui.Button("connect"))
			{
				Network.Connect(ip, 8086);
			}
			if (gui.Button("host"))
			{
				Network.InitializeServer(10, 8086, false);
				//Network.InitializeServer(4, 25000, !Network.HavePublicAddress());

			}
		}
	}
}