using UnityEngine;
using System.Collections;

public class PlayerNetworkMover : Photon.MonoBehaviour {
	
	public delegate void Respawn(float time);
	public event Respawn RespawnMe;

	public float health = 100f;

	private Vector3 position;
	private Quaternion rotation;
	private Quaternion headRot;
	private Quaternion rightShoulderRot;
	private Quaternion leftShoulderRot;
	private float smoothing = 20f;
	private Animator anim;
	private int cur;
	private GameObject rightShoulder , leftShoulder , head;
	private PlayerShooting ps;

	bool initialLoad = true;

	GameObject networkManager;
	private Camera[] spectateCameras;



	void Start () {
			
		handleMySripts ();
	}

	void Update(){

	}

	void handleMySripts()
	{

		Debug.Log ("Handle My scripts of "+PhotonNetwork.player.name + " at : "+Time.time);
		anim = GetComponent<Animator> ();
		ps = this.GetComponentInChildren<PlayerShooting> ();
		
		rightShoulder = GameObject.FindGameObjectWithTag ("rightShoulder");
		if(rightShoulder == null) Debug.LogWarning("RIGHT SHOULDER NOT FOUND");
		
		leftShoulder = GameObject.FindGameObjectWithTag ("leftShoulder");
		if(leftShoulder == null) Debug.LogWarning("LEFT SHOULDER NOT FOUND");
		
		head = GameObject.FindGameObjectWithTag ("head");
		if(head == null) Debug.LogWarning("HEAD NOT FOUND");

		networkManager = GameObject.FindGameObjectWithTag ("NetworkManager");
		if(networkManager == null) Debug.LogWarning("Network Manager NOT FOUND");
		spectateCameras = networkManager.GetComponent<NetworkManager>().spectateCameras;
		//spectateCamera.gameObject.SetActive (false);
		
		if(photonView.isMine)
		{
			GetComponent<Rigidbody>().useGravity = true;
			GetComponent<MouseLook>().enabled = true;
			
			Component[] mouses;
			mouses = GetComponentsInChildren<MouseLook>();
			foreach (MouseLook mouse in mouses) {
				mouse.enabled = true;
			}
			
			Component[] cams;
			cams = GetComponentsInChildren<Camera>();
			foreach (Camera cam in cams) {
				cam.enabled = true;
			}
			
			GetComponentInChildren<PlayerShooting>().enabled = true;
			GetComponent<MoveMyPlayer>().enabled = true;
			GetComponentInChildren<AudioListener>().enabled = true;
			
			//for(int i=0; i<spectateCameras.Length; i++)spectateCameras [i].gameObject.SetActive (false);

			
		} 
		else{	 
			StartCoroutine("UpdateData");
		}
	}

	IEnumerator UpdateData()
	{
		if(initialLoad)
		{
			initialLoad = false;
			transform.position = position;
			transform.rotation = rotation;
		}



		while(true)
		{
			if(Mathf.Abs(transform.position.x-position.x) >= 0.01)transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * 15);
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 30);

			if(cur >0)
			{
				ps.weapons[cur-1].SetActive(false);
				ps.weapons[cur].SetActive(true);
			}
			else
			{
				ps.weapons[ps.weapons.Length-1].SetActive(false);
				ps.weapons[cur].SetActive(true);
			}

			yield return null;
		} 
	}


	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if(stream.isWriting)
		{
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);

			stream.SendNext(ps.currentWeapon);

			stream.SendNext(anim.GetBool("walk"));
			stream.SendNext(anim.GetBool("crouch"));

		}
		else
		{
			position = roundVector((Vector3)stream.ReceiveNext(),2);
			rotation = roundQuaternion((Quaternion)stream.ReceiveNext(),2);

			cur = (int)stream.ReceiveNext();

			anim.SetBool("walk" , (bool)stream.ReceiveNext());
			anim.SetBool("crouch" , (bool)stream.ReceiveNext());


		}
	}

	private Vector3 roundVector(Vector3 input , int dec)
	{
		Vector3 output;
		float mult = Mathf.Pow(10.0f, (float)dec);
		output.x = Mathf.Round(input.x * mult) / mult;
		output.y = Mathf.Round(input.y * mult) / mult;
		output.z = Mathf.Round(input.z * mult) / mult;
		
		return output;
	}

	private Quaternion roundQuaternion(Quaternion input , int dec)
	{
		Quaternion output;
		float mult = Mathf.Pow(10.0f, (float)dec);
		output.x = 0;//Mathf.Round(input.x * mult) / mult;
		output.y = Mathf.Round(input.y * mult) / mult;
		output.z = Mathf.Round(input.z * mult) / mult;
		output.w = Mathf.Round(input.w * mult) / mult;

		return output;
	}
	

	[RPC]
	public void GetShot(float damage)
	{
		if(damage == -150f)
		{
			PhotonNetwork.Destroy (gameObject);
			PhotonNetwork.Disconnect();
			Application.LoadLevel("MainMenu");
		}

		Debug.Log ("I GOT SHOT");
		health -= damage;
		if(health <= 0 && photonView.isMine)
		{
			//if(RespawnMe != null) RespawnMe(-1f);
			networkManager.GetComponent<NetworkManager> ().SpectateGame ();
			PhotonNetwork.Destroy (gameObject);
			//PhotonHandler.DestroyImmediate(gameObject);
			PhotonNetwork.Disconnect();

		}
	}



	void OnDisconnectedFromPhoton(){
		Debug.Log ("I GOT DISCONNECTED!!");
	}

	void OnGUI()
	{
		if(Input.GetKeyUp(KeyCode.Escape))GetShot(-150f);
	}
	
		
}