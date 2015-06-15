using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class submitScore : MonoBehaviour {

	private int time;
	private bool flag;
	[SerializeField] private Text timeText;
	[SerializeField] private Text debugText;
	[SerializeField] private InputField nameInput;
	[SerializeField] private Button submitButton;
	[SerializeField] private Button backButton;

	private string url = "http://arch.icte.uowm.gr/game/fpsgame.php";

	void Start () {
		flag = true;
		time = PlayerPrefs.GetInt ("timeToSend");
		Debug.Log (time);
		timeText.text = "" + time / 60 + ":" + time % 60;

		submitButton.onClick.AddListener(() => { if(flag)Internet(); });
		backButton.onClick.AddListener(() => { Back(); });

		WWWForm form = new WWWForm();
		form.AddField("list_score", "targets1");
		WWW www = new WWW(url, form);
		
		StartCoroutine(WaitForRequest(www));

	}

	void Back(){Application.LoadLevel ("MainMenu");}

	void Internet(){

		if(nameInput.text != "" && nameInput.text != " ")
		{
			WWWForm form = new WWWForm();
			form.AddField("add_score", "dontCare");
			form.AddField("name", nameInput.text);
			form.AddField("time", time);
			form.AddField("score", "0");
			
			WWW www = new WWW(url, form);
			
			StartCoroutine(WaitForRequestTargets(www));
		}
		else 
		{
			debugText.text = "You need to provide a name!!";
		}

	}
	
	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;
		
		// check for errors
		if (www.error == null)
		{
			Debug.Log("WWW Ok!: " + www.data);
			debugText.text = www.data;
			flag = false;
		} else {
			Debug.Log("WWW Error: "+ www.error);
			debugText.text = www.data;
		}    
	} 

	IEnumerator WaitForRequestTargets(WWW www)
	{
		yield return www;
		
		// check for errors
		if (www.error == null)
		{
			Debug.Log("WWW Ok!: " + www.data);
			if(time <= int.Parse(www.data))
			{
				Reward();
			}

		} else {
			Debug.Log("WWW Error: "+ www.error);
			debugText.text = www.data;
		}    
	} 


	void Reward(){
		WWWForm form = new WWWForm();
		form.AddField("reward_targets", "dontCare");
		WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www));
	}
	
	
	
}
