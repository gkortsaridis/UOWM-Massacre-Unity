using UnityEngine;
using System.Collections;

public class zombie_manager : MonoBehaviour {

	private GameObject[] MDspawnpoints;
	private GameObject[] UOWMspawnpoints;
	private GameObject zombie_prefab;
	private int zombies_Left;
	private int the_wave;

	public GameObject createParticle;


	private GUIStyle guiStyle;
	public Font ammoFont;

	private int points;

	private int waveSizeX;
	private int waveSizeY;
	private int wavePosX;
	private int wavePosY;

	private bool inMD = true;


	
	// Use this for initialization
	void Start () {

		waveSizeX = Screen.width / 5;
		waveSizeY = Screen.height / 15;
		wavePosX = Screen.width / 30;
		wavePosY = Screen.height/20;

		guiStyle = new GUIStyle ();
		guiStyle.font = ammoFont;
		guiStyle.fontSize = Screen.width / 35;
		
		zombie_prefab = Resources.Load("ZombieAI") as GameObject;
		MDspawnpoints = GameObject.FindGameObjectsWithTag ("spawnpointMD");
		UOWMspawnpoints = GameObject.FindGameObjectsWithTag ("spawnpointUOWM");

		//Debug.Log ("I got "+MDspawnpoints.Length+" MDspawnpoints, and "+UOWMspawnpoints.Length + "UOWMspawnpoints");

		StartCoroutine(ZombieCreate(1));
	}

	IEnumerator ZombieCreate(int wave) {
		GameObject.FindGameObjectWithTag ("Player").GetComponentInChildren<PlayerShootingOffline> ().myHealth = 100;

		zombies_Left = wave*5 + 5;
		the_wave = wave;

		while (zombies_Left > 0)
		{
			yield return new WaitForSeconds(3);

			GameObject point;
			if(inMD)
			{
				point = MDspawnpoints[Random.Range(0,MDspawnpoints.Length)];
				//Debug.Log("Zombie in MD");
			}
			else
			{
				point = UOWMspawnpoints[Random.Range(0,UOWMspawnpoints.Length)];
				//Debug.Log("Zombie in UOWM");
			}

			Object destroySmoke = Instantiate(createParticle , point.transform.position , point.transform.rotation);
			Destroy(destroySmoke , 1f);
			
			GameObject zombie = Instantiate (zombie_prefab,point.transform.position,Quaternion.identity) as GameObject;
			zombie.GetComponent<NavmeshAI>().CreateZombie(2.0f);
			zombies_Left -= 1;
		}

		while(true)
		{
			int remaining = GameObject.FindGameObjectsWithTag("zombie").Length;
			yield return new WaitForSeconds(1);
			if(remaining == 0) break;
		}
		StartCoroutine (ZombieCreate(wave + 1));
	}

	void OnGUI(){
		GUI.Label (new Rect (wavePosX, wavePosY, waveSizeX, waveSizeY), "Wave " + the_wave,guiStyle);
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Player")
		{
			inMD = true;
		}
	}

	void OnTriggerExit(Collider other) {
		if(other.gameObject.tag == "Player")
		{
			inMD = false;
		}
	}

}
