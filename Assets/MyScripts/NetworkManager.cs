using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class NetworkManager : Photon.MonoBehaviour {
	 
	[SerializeField] private Text connectionText;
	[SerializeField] private Transform[] spawnPoints;
	[SerializeField] private Camera sceneCamera;
	[SerializeField] private Button joinButton;
	[SerializeField] private InputField roomList;
	[SerializeField] private InputField nameInput;
	[SerializeField] private InputField roomInput;

	[SerializeField] public Camera[] spectateCameras;

	GameObject player;
	PhotonView photonView;
	string myPlayer;
	Queue<string> messages;
	const int messageCount = 6;
	string messageToShow="";

	private int msgPosX;
	private int msgPosY;
	private int msgSizeX;
	private int msgSizeY;
	

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		Debug.Log ("serialize"); // just added this, like you
	}

	void Start () {
		photonView = GetComponent<PhotonView> ();

		messages = new Queue<string> (messageCount);

		msgSizeX = 2* Screen.width / 5;
		msgSizeY = Screen.height / 5;
		msgPosX = 0;
		msgPosY = Screen.height - msgSizeY;

		PhotonNetwork.logLevel = PhotonLogLevel.Full;
		PhotonNetwork.ConnectUsingSettings ("0.3");
		StartCoroutine ("UpdateConnectionStatus");
	}
	
	IEnumerator UpdateConnectionStatus () {
		while(true)
		{
			connectionText.text = PhotonNetwork.connectionStateDetailed.ToString ();
			yield return null;
		}
	}

	void OnReceivedRoomListUpdate()
	{
		roomList.text = "";
		RoomInfo[] rooms = PhotonNetwork.GetRoomList ();
		foreach (RoomInfo room in rooms)
			roomList.text += room.name + "\n";
	}


	RoomOptions ro;
	void OnJoinedLobby()
	{
		ro = new RoomOptions (){isVisible = true, maxPlayers = 10};
		joinButton.onClick.AddListener(() => { JoinRoom();  });
	}

	void JoinRoom()
	{
		PhotonNetwork.player.name = nameInput.text;
		if(roomInput.text != "" && nameInput.text != "")PhotonNetwork.JoinOrCreateRoom (roomInput.text, ro, TypedLobby.Default);
	}
	
	void OnJoinedRoom()
	{
		StopCoroutine ("UpdateConnectionStatus");
		connectionText.text = "";
		StartSpawnProcess (0f);
	}
	
	void StartSpawnProcess (float respawnTime)
	{
		myPlayer = PhotonNetwork.player.name;
		//AddMessage ("Player Joined : "+myPlayer);

		if (respawnTime == -1f) {
			Application.LoadLevel("OnlineTest");
		}
		else
		{
			sceneCamera.gameObject.SetActive (true);
			StartCoroutine ("SpawnPlayer", respawnTime);
		}
	}
	
	IEnumerator SpawnPlayer(float respawnTime)
	{
		yield return new WaitForSeconds(respawnTime);
		
		int index = Random.Range (0, spawnPoints.Length);

		string modelToInstantiate = "PlayerSwat";

		//if (myPlayer == "justin")modelToInstantiate="PlayerJustin";
		//else if(myPlayer == "swat")modelToInstantiate="PlayerSwat";
		//else modelToInstantiate="PlayerSoldier";

		player = PhotonNetwork.Instantiate (modelToInstantiate, 
		                                    spawnPoints [index].position,
		                                    spawnPoints [index].rotation,
		                                    0);


		player.GetComponent<PlayerNetworkMover> ().RespawnMe += StartSpawnProcess;
		sceneCamera.gameObject.SetActive(false);


	}


	bool spectate = false;
	int cameraCnt = 0;
	public void SpectateGame()
	{
		Debug.Log ("Gonna spectate");
		spectate = true;
		spectateCameras[cameraCnt].gameObject.SetActive (true);
	}


	/*void AddMessage(string message)
	{
		photonView.RPC ("AddMessage_RPC", PhotonTargets.All, message);
	}
	
	[RPC]
	void AddMessage_RPC(string message)
	{
		messages.Enqueue (message);
		//if(messages.Count > messageCount) messages.Dequeue();
			
		messageToShow = "";
		string[] msg = messages.ToArray ();
		for(int i=0; i<msg.Length; i++)messageToShow += msg[i]+"\n";
	}*/
	
	
	
	void OnGUI(){
		if(spectate)
		{
			if(GUI.Button(new Rect(0,0,Screen.width/4,100),"Camera"))
			{
				if(cameraCnt < spectateCameras.Length-2)
				{
					spectateCameras[cameraCnt].gameObject.SetActive (false);
					cameraCnt++;
					spectateCameras[cameraCnt].gameObject.SetActive (true);
				}
				else
				{
					spectateCameras[cameraCnt].gameObject.SetActive (false);
					cameraCnt=0;
					spectateCameras[cameraCnt].gameObject.SetActive (true);
				}
			}
		
			if(GUI.Button(new Rect(Screen.width/4,0,Screen.width/4,100),"Go to Lobby"))
			{
				Application.LoadLevel("OnlineTest");
			}

		}

		GUI.Label (new Rect(msgPosX,msgPosY,msgSizeX, msgSizeY),messageToShow);

		if(Input.GetKeyUp(KeyCode.Escape))Application.LoadLevel("MainMenu");
		
	}

}