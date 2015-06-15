using UnityEngine;
using System.Collections;

public class AmmoLoad : MonoBehaviour {

	private bool ammoGui;
	public int whichGun;
	public int points;
	private string[] weaponStats;
	private string[] info;

	private int msgSizeX;
	private int msgSizeY;
	private int msgPosX;
	private int msgPosY;

	public Font ammoFont;
	private GUIStyle ammoStyle;


	// Use this for initialization
	void Start () {
		ammoGui = false;

		ammoStyle = new GUIStyle ();
		ammoStyle.font = ammoFont;
		ammoStyle.fontSize = Screen.width / 35;

		weaponStats = new string[7];
		weaponStats [0] = "60|15|15|15|0";
		weaponStats [1] = "120|30|30|25|1";
		weaponStats [2] = "120|30|30|20|1";
		weaponStats [3] = "130|10|10|16|1";
		weaponStats [4] = "125|25|25|18|1";
		weaponStats [5] = "32|2|2|100|0";
		weaponStats [6] = "20|5|5|200|0";
		

		msgSizeX = Screen.width / 3;
		msgSizeX = Screen.height / 5;
		msgPosX = Screen.width / 2 - Screen.width / 5;
		msgPosY = Screen.height / 4;


	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Player")
		{
			ammoGui = true;
		}
	}

	void OnTriggerExit(Collider other) {
		if(other.gameObject.tag == "Player")
		{
			ammoGui = false;
		}
	}

	void OnGUI(){

		if(ammoGui)
		{
			GUI.Label(new Rect(msgPosX,msgPosY,msgSizeX,msgSizeY),"Press F for more Ammo - "+points+" Points",ammoStyle);

			if(Input.GetKeyUp(KeyCode.F))
			{
				GameObject player = GameObject.FindGameObjectWithTag("Player");
				if(player.GetComponentInChildren<PlayerShootingOffline>().points >= points)
				{
					player.GetComponentInChildren<PlayerShootingOffline>().points -= points;
					player.GetComponentInChildren<PlayerShootingOffline>().weaponStats[whichGun] = weaponStats[whichGun];
					info = weaponStats [whichGun].Split ('|');
					int curRoundBullets = int.Parse (info[2]);
					int maxBullets = int.Parse(info[0]);
					int maxRoundBullets = int.Parse (info [1]);
					int restBullets = maxBullets - maxRoundBullets;
					player.GetComponentInChildren<PlayerShootingOffline>().curRoundBullets = curRoundBullets;
					player.GetComponentInChildren<PlayerShootingOffline>().restBullets = restBullets;
				}
			}
		}
	}
}
