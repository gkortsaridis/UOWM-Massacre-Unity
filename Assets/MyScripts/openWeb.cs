using UnityEngine;
using System.Collections;

public class openWeb : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void openURL(string url)
	{
		Application.OpenURL (url);
	}
}
