using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class zombiesGameOver : MonoBehaviour {

	[SerializeField] private Text scoreText;
	[SerializeField] private Text debugText;
	[SerializeField] private InputField nameInput;
	[SerializeField] private Button submitButton;
	[SerializeField] private Button backButton;
	private bool flag;
	int score;

	private string url = "http://arch.icte.uowm.gr/game/fpsgame.php";

	// Use this for initialization
	void Start () {
	
		flag = true;
		score = PlayerPrefs.GetInt ("myscore");
		scoreText.text = "" + score;

		submitButton.onClick.AddListener(() => { if(flag)Internet(); });
		backButton.onClick.AddListener(() => { Back(); });

		WWWForm form = new WWWForm();
		form.AddField("list_score", "zombies1");
		WWW www = new WWW(url, form);
		
		StartCoroutine(WaitForRequestZombie(www));

	}
	
	void Back(){Application.LoadLevel ("MainMenu");}
	
	void Internet(){
	
		if(nameInput.text != "" && nameInput.text != " ")
		{
			WWWForm form = new WWWForm();
			form.AddField("add_score", "dontCare");
			form.AddField("name", nameInput.text);
			form.AddField("time", "999999999999");
			form.AddField("score", score);
			
			WWW www = new WWW(url, form);
			
			StartCoroutine(WaitForRequest(www));
		}
		else 
		{
			debugText.text = "You need to provide a name...";
		}
		
	}

	IEnumerator WaitForRequestZombie(WWW www)
	{
		yield return www;
		
		// check for errors
		if (www.error == null)
		{
			Debug.Log("WWW Ok!: " + www.data);

			if(score >= int.Parse(www.data))
			{
				Reward ();
			}

			
		} else {
			Debug.Log("WWW Error: "+ www.error);
			debugText.text = www.data;
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
			debugText.text = www.error;

		}    
	}   

	void Reward(){
		WWWForm form = new WWWForm();
		form.AddField("reward_zombies", "dontCare");
		WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www));
	}

}
