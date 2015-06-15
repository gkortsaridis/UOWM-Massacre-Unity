using UnityEngine;
using System.Collections;

public class mainMenuGUI : MonoBehaviour {

	private int oflSizeX;
	private int oflSizeY;
	private int oflPosX;
	private int oflPosY;

	private int onlSizeX;
	private int onlSizeY;
	private int onlPosX;
	private int onlPosY;

	private int zomSizeX;
	private int zomSizeY;
	private int zomPosX;
	private int zomPosY;

	private int leadSizeX;
	private int leadSizeY;
	private int leadPosX;
	private int leadPosY;

	private int exitSizeX;
	private int exitSizeY;
	private int exitPosX;
	private int exitPosY;

	private int instrSizeX;
	private int instrSizeY;
	private int instrPosX;
	private int instrPosY;

	private int aboutSizeX;
	private int aboutSizeY;
	private int aboutPosX;
	private int aboutPosY;

	public GUISkin menuSkin;

	// Use this for initialization
	void Start () {
	
		oflSizeX = Screen.width / 4;
		oflSizeY = Screen.height / 10;
		oflPosX = Screen.width / 20;//Screen.width / 2;
		oflPosY = Screen.height / 2;//Screen.height / 2;

		onlSizeX = oflSizeX;
		onlSizeY = oflSizeY;
		onlPosX = oflPosX;
		onlPosY = oflPosY + oflSizeY + Screen.height / 20;

		zomSizeX = oflSizeX;
		zomSizeY = oflSizeY;
		zomPosX = Screen.width/2 - zomSizeX/2;
		zomPosY = oflPosY;

		leadSizeX = oflSizeX;
		leadSizeY = oflSizeY;
		leadPosX = Screen.width - leadSizeX - Screen.width / 20;
		leadPosY = oflPosY;


		exitSizeX = oflSizeX;
		exitSizeY = oflSizeY;
		exitPosX = leadPosX;
		exitPosY = onlPosY;

		instrSizeX = oflSizeX;
		instrSizeY = oflSizeY;
		instrPosX = zomPosX;
		instrPosY = exitPosY;

		aboutSizeX = oflSizeX;
		aboutSizeY = oflSizeY;
		aboutPosX = zomPosX;
		aboutPosY = onlPosY + onlSizeY + Screen.height / 20;



	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		GUI.skin = menuSkin;

		if(GUI.Button(new Rect(oflPosX,oflPosY,oflSizeX,oflSizeY),"Train Offline"))Application.LoadLevel("offlineGame");
		if(GUI.Button(new Rect(onlPosX,onlPosY,onlSizeX,onlSizeY),"Play Online"))Application.LoadLevel("OnlineTest");
		if(GUI.Button(new Rect(zomPosX,zomPosY,zomSizeX,zomSizeY),"Zombies"))Application.LoadLevel("LoadZombies");
		if(GUI.Button(new Rect(leadPosX,leadPosY,leadSizeX,leadSizeY),"Check leaderboard"))Application.LoadLevel("Leaderboard");
		if(GUI.Button(new Rect(instrPosX,instrPosY,instrSizeX,instrSizeY),"Instructions"))Application.LoadLevel("Instructions");
		if(GUI.Button(new Rect(exitPosX,exitPosY,exitSizeX,exitSizeY),"Exit"))Application.Quit();
		if(GUI.Button(new Rect(aboutPosX,aboutPosY,aboutSizeX,aboutSizeY),"About"))Application.LoadLevel("About");
	}
}
