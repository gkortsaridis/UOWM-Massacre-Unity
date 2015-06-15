using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ListScores : MonoBehaviour {

	[SerializeField] private Text Scores;
	[SerializeField] private Button BackButton;
	[SerializeField] private Button TargetsButton;
	[SerializeField] private Button ZombiesButton;

	private bool targets , zombies;


	// Use this for initialization
	void Start () {
		Internet ("zombies");
		BackButton.onClick.AddListener(() => { Back(); });
		TargetsButton.onClick.AddListener(() => { Internet("targets"); });
		ZombiesButton.onClick.AddListener(() => { Internet("zombies"); });

	}

	void Back(){Application.LoadLevel ("MainMenu");}

	void Internet(string what){
		string url = "http://arch.icte.uowm.gr/game/fpsgame.php";

		WWWForm form = new WWWForm();
		form.AddField("list_score", what);
		WWW www = new WWW(url, form);
			
		StartCoroutine(WaitForRequest(www));
	}
	
	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;
		
		// check for errors
		if (www.error == null)
		{
			Debug.Log("WWW Ok!: " + www.data);
			fillList(www.data);

		} else {
			Debug.Log("WWW Error: "+ www.error);
			fillList("Cannot Connect to Database");
		}    
	} 

	void fillList(string data)
	{
		Scores.text = "";
		string[] names=data.Split(';');
		Debug.Log (names.Length);

		for (int i=0; i<names.Length; i++) {
			Scores.text += names[i]+"\n";
		}
	}

}
