using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {

	public GameObject destination;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Player" || other.gameObject.tag == "zombie")
		{
			other.gameObject.transform.position = destination.transform.position;
		}
	}
}
