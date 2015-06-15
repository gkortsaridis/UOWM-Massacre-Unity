using UnityEngine;
using System.Collections;

public class PlayerShootingOffline : MonoBehaviour {
	
	public GameObject[] muzzleFlash;
	public GameObject[] impactPrefab;
	public string[] weaponStats;
	public GameObject[] weapons;
	[HideInInspector]public int currentWeapon;

	private bool shooting = false;
	private int maxRoundBullets;
	private float damage;
	private int automatic; 
	private string[] info;


	private float fireRate = 0.1f;
	private float nextFire = 0.0f;

	public AudioClip shot;
	
	[SerializeField] Texture2D bullet;
	[SerializeField] Texture2D aimTexture;
	[SerializeField] Texture2D health1;
	[SerializeField] Texture2D health2;
	[SerializeField] Texture2D health3;
	[SerializeField] Texture2D health4;
	[SerializeField] Texture2D health5;
	[SerializeField] Texture2D[] gunPics;

	[SerializeField] Font ammoFont;
	private int maxBullets;
	[HideInInspector]public int curRoundBullets;
	[HideInInspector]public int restBullets;
	private GUIStyle ammoStyle;

	private PlayerNetworkMover pnm;
	public float myHealth;

	private int ammoSizeX;
	private int ammoSizeY;
	private int ammoPosX;
	private int ammoPosY;

	private int bulletSizeX;
	private int bulletSizeY;
	private int bulletPosX;
	private int bulletPosY;

	private int healthSizeX;
	private int healthSizeY;
	private int healthPosX;
	private int healthPosY;

	private int healthLogoSizeX;
	private int healthLogoSizeY;
	private int healthLogoPosX;
	private int healthLogoPosY;

	private int gunPicSizeX;
	private int gunPicSizeY;
	private int gunPicPosX;
	private int gunPicPosY;

	private int scoreSizeX;
	private int scoreSizeY;
	private int scorePosX;
	private int scorePosY;

	private int totalSpace;

	private float timer;

	private long targetsLeft;


	public int points;


	Animator anim;
	// Use this for initialization
	void Start () {

		Cursor.visible = false;

		anim = GetComponentInParent<Animator> ();

		myHealth = 100;
		points = 0;

		ammoStyle = new GUIStyle ();
		ammoStyle.font = ammoFont;
		ammoStyle.fontSize = Screen.width / 35;


		info = weaponStats [0].Split ('|');
		maxBullets = int.Parse(info[0]);
		maxRoundBullets = int.Parse (info [1]);
		curRoundBullets = int.Parse (info[2]);
		damage = float.Parse(info[3]);
		automatic = int.Parse(info[4]);
		restBullets = maxBullets - maxRoundBullets;

		
		currentWeapon = 0;
		weapons [0].SetActive (true);

		muzzleFlash[currentWeapon].SetActive (false);

		totalSpace = (int)(Screen.width - 10 - 4.5*Screen.width / 6);
		bulletSizeX = totalSpace / maxRoundBullets;
		bulletSizeY = 60;
		bulletPosX = (int)(4.5 * Screen.width / 6);
		bulletPosY = Screen.height-bulletSizeY - 15;

		ammoSizeX = Screen.width/4;
		ammoSizeY = Screen.height / 10;
		ammoPosX = bulletPosX;
		ammoPosY = bulletPosY - 30;

		healthLogoSizeX = Screen.width / 10;
		healthLogoSizeY = ammoSizeY;
		healthLogoPosX = ammoPosX + Screen.width/10;
		healthLogoPosY = ammoPosY-healthLogoSizeY/2;

		healthSizeX = healthLogoSizeX;
		healthSizeY = ammoSizeY;
		healthPosX = healthLogoPosX + Screen.width / 20;
		healthPosY = ammoPosY;

		gunPicPosX = ammoPosX;
		gunPicPosY = ammoPosY - Screen.height / 6;
		gunPicSizeX = Screen.width - ammoPosX;
		gunPicSizeY = Screen.height / 8;

		scoreSizeX = Screen.width / 5;
		scoreSizeY = Screen.height / 15;
		scorePosX = Screen.width - scoreSizeX - Screen.width / 30;
		scorePosY = Screen.height/20;

		GameObject[] targets = GameObject.FindGameObjectsWithTag ("target");
		targetsLeft = targets.Length;
	}

	// Update is called once per frame
	void Update () {
		if(targetsLeft>0) timer += Time.deltaTime;

		if(Input.GetMouseButtonDown(0))GetComponent<Camera>().fieldOfView = 70;
		if(Input.GetMouseButtonUp(0))GetComponent<Camera>().fieldOfView = 60;

		if(Input.GetKeyUp(KeyCode.R))
		{
			int toReload = maxRoundBullets - curRoundBullets;
			if(toReload<=restBullets)
			{
				curRoundBullets += toReload;
				restBullets -= toReload;
			}
			else
			{
				curRoundBullets +=restBullets;
				restBullets -= restBullets;
			}
		}

		if(Input.GetKeyUp(KeyCode.P))
		{
			if(Application.loadedLevelName != "Zombies" )
			{
				if(currentWeapon<weapons.Length-2)
				{
					currentWeapon++;
					weapons[currentWeapon-1].SetActive(false);
					weapons[currentWeapon].SetActive(true);
				}
				else
				{
					weapons[currentWeapon].SetActive(false);
					currentWeapon =0;
					weapons[currentWeapon].SetActive(true);
				}
			}
			else
			{
				if(currentWeapon<weapons.Length-1)
				{
					currentWeapon++;
					weapons[currentWeapon-1].SetActive(false);
					weapons[currentWeapon].SetActive(true);
				}
				else
				{
					weapons[currentWeapon].SetActive(false);
					currentWeapon =0;
					weapons[currentWeapon].SetActive(true);
				}
			}


			info = weaponStats [currentWeapon].Split ('|');
			//Debug.Log(info.Length);
			maxBullets = int.Parse(info[0]);
			maxRoundBullets = int.Parse (info [1]);
			curRoundBullets = int.Parse (info[2]);
			damage = float.Parse(info[3]);
			automatic = int.Parse(info[4]);
			restBullets = maxBullets - curRoundBullets;


		}
		if(automatic == 1)
		{
			Fire ();
		}
		else
		{
			if (Input.GetButtonDown ("Fire") && !Input.GetKey (KeyCode.LeftShift)) {
				if(curRoundBullets > 0)
				{
					Fire ();
				}
			}
		}
	}

	IEnumerator shoot(){
		AudioSource.PlayClipAtPoint(shot, this.transform.position);
		muzzleFlash[currentWeapon].SetActive (true);
		yield return new WaitForSeconds(0.02f);
		muzzleFlash[currentWeapon].SetActive (false);
	}

	void Fire () {
		if(Input.GetButton("Fire") && Time.time > nextFire) {
			
			nextFire =  Time.time + fireRate;

			if(curRoundBullets>0)
			{
				if(maxBullets>0)maxBullets--;
				curRoundBullets--;
				weaponStats[currentWeapon] = maxBullets+"|"+maxRoundBullets+"|"+curRoundBullets+"|"+damage+"|"+automatic;
				StartCoroutine(shoot());
				ForceFire();
			}
			
		}
		
	}

	void ForceFire () {	
		Vector3 fwd = transform.forward;
		RaycastHit hit;
		Debug.DrawRay(transform.position, fwd * 10, Color.green); 
		
		if(Input.GetButton("Fire") && Physics.Raycast(transform.position, fwd, out hit, Mathf.Infinity))
		{
			if(currentWeapon != 6)
			{
				Object smoke = Instantiate(impactPrefab[currentWeapon] , hit.point , hit.transform.rotation);
				Destroy(smoke , 0.2f);
				
				//print (hit.transform.tag);
				if(hit.transform.tag == "target")
				{
					Debug.Log("Found target");
					Object destroySmoke = Instantiate(impactPrefab[currentWeapon] , hit.point , hit.transform.rotation);
					Destroy(destroySmoke , 1f);
					Destroy(hit.transform.gameObject);

					targetsLeft--;

					if(targetsLeft == 0)
					{
						Debug.Log("Going Internet");
						PlayerPrefs.SetInt("timeToSend",minutes*60+seconds);
						Application.LoadLevel("submitScore");
					}
				}
				else if(hit.transform.tag == "zombie")
				{
					Object destroySmoke = Instantiate(impactPrefab[currentWeapon] , hit.point , hit.transform.rotation);
					Destroy(destroySmoke , 1f);
					hit.transform.GetComponent<NavmeshAI>().GetShot(damage);
					points += 20;
				}
			}
			else if(currentWeapon == 6)
			{
				Debug.Log("GRENADE!s");
				Object smoke = Instantiate(impactPrefab[currentWeapon] , hit.point , hit.transform.rotation);
				Destroy(smoke , 0.2f);

				Collider[] ArroundNear = Physics.OverlapSphere(transform.position,5.0f);
				
				foreach(Collider intoExp in ArroundNear)
				{              
					if(intoExp.transform.tag == "zombie")
						intoExp.transform.GetComponent<NavmeshAI>().GetShot(damage);                       
				}

			}
			
		}
		
	}


	public void gotShot(float damage){
		if (damage > myHealth) {
			//game over
			Cursor.visible = true;
			PlayerPrefs.SetInt("myscore",points);
			Application.LoadLevel("zombieGameOver");
		}
		else myHealth -= damage;
	}
	
	int minutes;
	int seconds;
	void OnGUI(){
		
		
		GUI.DrawTexture (new Rect(Screen.width/2-25 , Screen.height/2-25,50,50) , aimTexture);

		//GUI.Label (new Rect (10, Screen.height - 100, Screen.width / 4, 100), "RoundBullets : " + curRoundBullets + "\nMaxBullets : " + maxBullets,ammoStyle);
		GUI.Label (new Rect (ammoPosX, ammoPosY, ammoSizeX, ammoSizeY),curRoundBullets+"/"+restBullets,ammoStyle);

		if(myHealth > 80.0f) GUI.Label (new Rect (healthLogoPosX, healthLogoPosY, healthLogoSizeX, healthLogoSizeY),health1);
		else if(myHealth > 60.0f) GUI.Label (new Rect (healthLogoPosX, healthLogoPosY, healthLogoSizeX, healthLogoSizeY),health2);
		else if(myHealth > 40.0f) GUI.Label (new Rect (healthLogoPosX, healthLogoPosY, healthLogoSizeX, healthLogoSizeY),health3);
		else if(myHealth > 20.0f) GUI.Label (new Rect (healthLogoPosX, healthLogoPosY, healthLogoSizeX, healthLogoSizeY),health4);
		else GUI.Label (new Rect (healthLogoPosX, healthLogoPosY, healthLogoSizeX, healthLogoSizeY),health5);

		GUI.Label (new Rect (healthPosX, healthPosY, healthSizeX, healthSizeY),""+myHealth,ammoStyle);

		GUI.DrawTexture (new Rect(gunPicPosX,gunPicPosY,gunPicSizeX,gunPicSizeY),gunPics[currentWeapon]);
		
		for(int i=0; i<curRoundBullets; i++)
		{
			GUI.Label(new Rect(bulletPosX + i*bulletSizeX,bulletPosY , bulletSizeX , bulletSizeY),bullet);
		}

		if(Application.loadedLevelName == "offlineGame")
		{
			minutes = Mathf.FloorToInt(timer / 60F);
			seconds = Mathf.FloorToInt(timer - minutes * 60);
			string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
			GUI.Label(new Rect(10,10,250,100), "Timer : "+niceTime,ammoStyle);
			GUI.Label (new Rect (Screen.width-Screen.width/4, 10, Screen.width/4, 100), "Targets Left : " + targetsLeft,ammoStyle);
		}
		else if(Application.loadedLevelName == "Zombies")
		{
			GUI.Label (new Rect (scorePosX, scorePosY, scoreSizeX, scoreSizeY), "Score " + points,ammoStyle);
		}

		//GUI.Label (new Rect(10,Screen.height-Screen.height/15,Screen.width/2,Screen.height/15),"Supervised by\nDr.Minas Dasygenis",ammoStyle);

	}


}