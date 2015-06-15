using UnityEngine;
using System.Collections;

public class roomEnterTrigger : MonoBehaviour {

	private bool flag = false;
	private string url = "http://arch.icte.uowm.gr/game/fpsgame.php";

	[SerializeField] Font ammoFont;
	private GUIStyle ammoStyle;
	[SerializeField] int room;

	void Start(){
		ammoStyle = new GUIStyle ();
		ammoStyle.font = ammoFont;
		ammoStyle.fontSize = Screen.width / 35;
		
	}


	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Player")
		{
			Debug.Log("MD ENTERED!!");

			WWWForm form = new WWWForm();

			if(room == 0) form.AddField("mdRoomEnter", "dontCare");
			else if(room == 1)form.AddField("amphRoomEnter","dontCare");
			WWW www = new WWW(url, form);
			StartCoroutine(WaitForRequest(www));
		}
	}

	void OnTriggerExit(Collider other) {
		if(other.gameObject.tag == "Player")
		{
			timeLeft = 10;
			flag = false;
		}
	}


	private float timeLeft = 10.0f;
	private string data;
	void Update(){

		if (flag) {

			if(timeLeft>0)
			{
				timeLeft -= Time.deltaTime;
			}
			else
			{
				flag = false;
			}
		}
	}
	
	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;
		
		// check for errors
		if (www.error == null)
		{
			Debug.Log("WWW Ok!: " + www.data);
			flag = true;
			data = www.data;
		} else {
			Debug.Log("WWW Error: "+ www.error);
		}    
	} 


	void OnGUI()
	{
		if (flag) {
			GUI.Label(new Rect(Screen.width/10,Screen.height/8,Screen.width/4,Screen.height/4),data,ammoStyle);
		}

	}
}
